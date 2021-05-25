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
            ValidateCategoryAuditFieldsOnCreate(category);
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

        private bool IsInvalid(Guid input) => input == default;
        private bool IsInvalid(DateTimeOffset input) => input == default;

        private void ValidateCategoryAuditFieldsOnCreate(Category category)
        {
            switch (category)
            {
                case { } when IsInvalid(input: category.CreatedBy):
                    throw new InvalidCategoryException(
                        parameterName: nameof(category.CreatedBy),
                        parameterValue: category.CreatedBy);

                case { } when IsInvalid(input: category.UpdatedBy):
                    throw new InvalidCategoryException(
                        parameterName: nameof(category.UpdatedBy),
                        parameterValue: category.UpdatedBy);

                case { } when IsInvalid(input: category.CreatedDate):
                    throw new InvalidCategoryException(
                        parameterName: nameof(category.CreatedDate),
                        parameterValue: category.CreatedDate);

                case { } when IsInvalid(input: category.UpdatedDate):
                    throw new InvalidCategoryException(
                        parameterName: nameof(category.UpdatedDate),
                        parameterValue: category.UpdatedDate);

                case { } when category.UpdatedDate != category.CreatedDate:
                    throw new InvalidCategoryException(
                        parameterName: nameof(category.UpdatedDate),
                        parameterValue: category.UpdatedDate);
            }
        }
    }
}
