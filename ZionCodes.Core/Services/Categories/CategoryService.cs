using System;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Brokers.DateTimes;
using ZionCodes.Core.Brokers.Loggings;
using ZionCodes.Core.Brokers.Storages;
using ZionCodes.Core.Models.Categories;

namespace ZionCodes.Core.Services.Categories
{
    public partial class CategoryService : ICategoryService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public CategoryService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker
            )
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }


        public ValueTask<Category> AddCategoryAsync(Category category) =>
            TryCatch(async () =>
            {
                ValidateCategoryOnCreate(category);

                return await this.storageBroker.InsertCategoryAsync(category);
            });

        public IQueryable<Category> RetrieveAllCategories() =>
            TryCatch(() =>
            {
                IQueryable<Category> storageCategories = this.storageBroker.SelectAllCategories();

                ValidateStorageCategories(storageCategories);

                return storageCategories;
            });

        public ValueTask<Category> RetrieveCategoryByIdAsync(Guid categoryId) =>
            TryCatch(async () =>
            {
                ValidateCategoryId(categoryId);
                Category storageCategory =
                    await this.storageBroker.SelectCategoryByIdAsync(categoryId);

                ValidateStorageCategory(storageCategory, categoryId);

                return storageCategory;
            });

        public ValueTask<Category> ModifyCategoryAsync(Category category) =>
            TryCatch(async () =>
            {
                ValidateCategoryOnModify(category);
                ValidateCategoryIdIsNull(category.Id);
                Category maybeCategory =
                        await this.storageBroker.SelectCategoryByIdAsync(category.Id);

                return await this.storageBroker.UpdateCategoryAsync(category);
            });

        public ValueTask<Category> RemoveCategoryByIdAsync(Guid categoryId) =>
            TryCatch(async () =>
            {
                ValidateCategoryIdIsNull(categoryId);
                Category storageCategory =
                await this.storageBroker.SelectCategoryByIdAsync(categoryId);

                ValidateStorageCategory(storageCategory, categoryId);

                return await this.storageBroker.DeleteCategoryAsync(storageCategory);
            });

    }
}
