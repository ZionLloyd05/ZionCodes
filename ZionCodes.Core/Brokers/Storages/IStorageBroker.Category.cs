using System;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Categories;

namespace ZionCodes.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        public ValueTask<Category> InsertCategoryAsync(Category category);
        public IQueryable<Category> SelectAllCategories();
        public ValueTask<Category> SelectCategoryByIdAsync(Guid categoryId);
        public ValueTask<Category> UpdateCategoryAsync(Category category);
        public ValueTask<Category> DeleteCategoryAsync(Category category);
    }
}
