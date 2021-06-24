using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ZionCodes.Core.Models.Categories;
using ZionCodes.Core.Models.Categories.Exceptions;

namespace ZionCodes.Core.Services.Categories
{
    public partial class CategoryService
    {
        private delegate ValueTask<Category> ReturningCategoryFunction();
        private delegate IQueryable<Category> ReturningQueryableCategoryFunction();

        private async ValueTask<Category> TryCatch(
            ReturningCategoryFunction returningCategoryFunction)
        {
            try
            {
                return await returningCategoryFunction();
            }
            catch (NullCategoryException nullCategoryException)
            {
                throw CreateAndLogValidationException(nullCategoryException);
            }
            catch (InvalidCategoryException invalidCategoryException)
            {
                throw CreateAndLogValidationException(invalidCategoryException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (NotFoundCategoryException notFoundCategoryException)
            {
                throw CreateAndLogValidationException(notFoundCategoryException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsCategoryException =
                    new AlreadyExistsCategoryException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsCategoryException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedCategoryException = new LockedCategoryException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedCategoryException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (InvalidCategoryInputException invalidCategoryInputException)
            {
                throw CreateAndLogValidationException(invalidCategoryInputException);
            }
          
        }

        private IQueryable<Category> TryCatch
            (ReturningQueryableCategoryFunction returningQueryableCategoryFunction)
        {
            try
            {
                return returningQueryableCategoryFunction();
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

        private CategoryValidationException CreateAndLogValidationException(Exception exception)
        {
            var CategoryValidationException = new CategoryValidationException(exception);
            this.loggingBroker.LogError(CategoryValidationException);

            return CategoryValidationException;
        }

        private CategoryDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var categoryDependencyException = new CategoryDependencyException(exception);
            this.loggingBroker.LogError(categoryDependencyException);

            return categoryDependencyException;
        }

        private CategoryDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var categoryDependencyException = new CategoryDependencyException(exception);
            this.loggingBroker.LogCritical(categoryDependencyException);

            return categoryDependencyException;
        }

        private CategoryServiceException CreateAndLogServiceException(Exception exception)
        {
            var categoryServiceException = new CategoryServiceException(exception);
            this.loggingBroker.LogError(categoryServiceException);

            return categoryServiceException;
        }
    }
}
