using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ZionCodes.Core.Models.Articles;
using ZionCodes.Core.Models.Articles.Exceptions;
using ZionCodes.Core.Models.Categories.Exceptions;

namespace ZionCodes.Core.Services.Articles
{
    public partial class ArticleService
    {
        private delegate ValueTask<Article> ReturningArticleFunction();
        private delegate IQueryable<Article> ReturningQueryableArticleFunction();

        private async ValueTask<Article> TryCatch(
            ReturningArticleFunction returningArticleFunction)
        {
            try
            {
                return await returningArticleFunction();
            }
            catch (NullArticleException nullArticleException)
            {
                throw CreateAndLogValidationException(nullArticleException);
            }
            catch (InvalidArticleException invalidArticleException)
            {
                throw CreateAndLogValidationException(invalidArticleException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (NotFoundArticleException notFoundArticleException)
            {
                throw CreateAndLogValidationException(notFoundArticleException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsArticleException =
                    new AlreadyExistsArticleException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsArticleException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedArticleException = new LockedArticleException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedArticleException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (InvalidArticleInputException invalidArticleInputException)
            {
                throw CreateAndLogValidationException(invalidArticleInputException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private IQueryable<Article> TryCatch
            (ReturningQueryableArticleFunction returningQueryableArticleFunction)
        {
            try
            {
                return returningQueryableArticleFunction();
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

        private ArticleValidationException CreateAndLogValidationException(Exception exception)
        {
            var ArticleValidationException = new ArticleValidationException(exception);
            this.loggingBroker.LogError((Exception)ArticleValidationException);

            return ArticleValidationException;
        }

        private ArticleDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var categoryDependencyException = new ArticleDependencyException(exception);
            this.loggingBroker.LogError(categoryDependencyException);

            return categoryDependencyException;
        }

        private ArticleDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var categoryDependencyException = new ArticleDependencyException(exception);
            this.loggingBroker.LogCritical(categoryDependencyException);

            return categoryDependencyException;
        }

        private ArticleServiceException CreateAndLogServiceException(Exception exception)
        {
            var categoryServiceException = new ArticleServiceException(exception);
            this.loggingBroker.LogError(categoryServiceException);

            return categoryServiceException;
        }
    }
}
