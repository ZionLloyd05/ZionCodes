using System;
using System.Linq;
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
            ValidateCategoryPropertiesOnCreate(category);
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
            if (categoryId == default)
            {
                throw new InvalidCategoryException(
                    parameterName: nameof(Category.Id),
                    parameterValue: categoryId);
            }
        }

        private void ValidateStorageCategory(Category storageCategory, Guid categoryId)
        {
            if (storageCategory == null)
            {
                throw new NotFoundCategoryException(categoryId);
            }
        }

        private void ValidateCategoryOnModify(Category category)
        {
            ValidateCategoryIsNull(category);
        }

        private void ValidateCategoryId(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
                throw new InvalidCategoryInputException(
                    parameterName: nameof(Category.Id),
                    parameterValue: categoryId);
            }
        }

        private void ValidateCategoryPropertiesOnCreate(Category category)
        {
            switch (category)
            {
                case { } when IsInvalid(category.Title):
                    throw new InvalidCategoryException(
                        parameterName: nameof(Category.Title),
                        parameterValue: category.Title);
            }
        }

        private void ValidateStorageCategories(IQueryable<Category> storageCategories)
        {
            if (storageCategories.Count() == 0)
            {
                this.loggingBroker.LogWarning("No categories found in storage.");
            }
        }

        private bool IsInvalid(Guid input) => input == default;
        private bool IsInvalid(string categoryTitle) => String.IsNullOrWhiteSpace(categoryTitle);
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

                case { } when IsDateNotRecent(category.CreatedDate):
                    throw new InvalidCategoryException(
                        parameterName: nameof(Category.CreatedDate),
                        parameterValue: category.CreatedDate);
            }
        }

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }
    }
}
