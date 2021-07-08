using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ZionCodes.Core.Models.Comments;
using ZionCodes.Core.Models.Comments.Exceptions;

namespace ZionCodes.Core.Services.Comments
{
    public partial class CommentService
    {
        private delegate ValueTask<Comment> ReturningCommentFunction();

        private async ValueTask<Comment> TryCatch(
            ReturningCommentFunction returningCommentFunction)
        {
            try
            {
                return await returningCommentFunction();
            }
            catch (NullCommentException nullCommentException)
            {
                throw CreateAndLogValidationException(nullCommentException);
            }
            catch (InvalidCommentException invalidCommentException)
            {
                throw CreateAndLogValidationException(invalidCommentException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsCategoryException =
                    new AlreadyExistsCommentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsCategoryException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }


        private CommentValidationException CreateAndLogValidationException(Exception exception)
        {
            var CommentValidationException = new CommentValidationException(exception);
            this.loggingBroker.LogError((Exception)CommentValidationException);

            return CommentValidationException;
        }

        private CommentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var CommentDependencyException = new CommentDependencyException(exception);
            this.loggingBroker.LogCritical(CommentDependencyException);

            return CommentDependencyException;
        }

        private CommentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var commentDependencyException = new CommentDependencyException(exception);
            this.loggingBroker.LogError(commentDependencyException);

            return commentDependencyException;
        }

        private CommentServiceException CreateAndLogServiceException(Exception exception)
        {
            var commentServiceException = new CommentServiceException(exception);
            this.loggingBroker.LogError(commentServiceException);

            return commentServiceException;
        }
    }
}
