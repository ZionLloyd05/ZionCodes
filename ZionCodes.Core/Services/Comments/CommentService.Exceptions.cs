using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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
    }
}
