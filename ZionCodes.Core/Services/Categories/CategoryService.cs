using System;
using System.Collections.Generic;
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
        private IList<int> notelist = new List<int>();

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

        public IQueryable<Category> RetrieveAllCategories()
        {
            throw new NotImplementedException();
        }
    }
}
