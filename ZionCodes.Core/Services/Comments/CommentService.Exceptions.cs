using System;
using System.Linq;
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
        private delegate IQueryable<Comment> ReturningQueryableCommentFunction();

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
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedCommentException = new LockedCommentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedCommentException);
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
                var alreadyExistsCommentException =
                    new AlreadyExistsCommentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsCommentException);
            }
            catch (NotFoundCommentException notFoundCommentException)
            {
                throw CreateAndLogValidationException(notFoundCommentException);
            }
            catch (InvalidCommentInputException invalidCommentInputException)
            {
                throw CreateAndLogValidationException(invalidCommentInputException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private IQueryable<Comment> TryCatch
           (ReturningQueryableCommentFunction returningQueryableCommentFunction)
        {
            try
            {
                return returningQueryableCommentFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
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
