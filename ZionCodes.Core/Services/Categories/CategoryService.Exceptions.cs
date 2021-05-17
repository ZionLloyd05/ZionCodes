using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Categories;
using ZionCodes.Core.Models.Categories.Exceptions;

namespace ZionCodes.Core.Services.Categories
{
    public partial class CategoryService
    {
        private delegate ValueTask<Category> ReturningCategoryFunction();

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
        }

        private CategoryValidationException CreateAndLogValidationException(Exception exception)
        {
            var CategoryValidationException = new CategoryValidationException(exception);
            this.loggingBroker.LogError(CategoryValidationException);

            return CategoryValidationException;
        }
    }
}
