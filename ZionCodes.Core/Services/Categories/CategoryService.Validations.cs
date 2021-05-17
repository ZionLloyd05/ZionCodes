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
        private void ValidateCategoryOnCreate(Category category)
        {
            ValidateCategoryIsNull(category);
            ValidateCategoryIdIsNull(category.Id);
        }

        private void ValidateCategoryIsNull(Category category)
        {
            if (category is null)
            {
                throw new NullCategoryException();
            }
        }

        private void ValidateCategoryIdIsNull(Guid categoryId)
        {
            if(categoryId == default)
            {
                throw new InvalidCategoryException(
                    parameterName: nameof(Category.Id),
                    parameterValue: categoryId);
            }
        }
    }
}
