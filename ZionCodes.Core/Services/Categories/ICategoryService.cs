using System;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Categories;

namespace ZionCodes.Core.Services.Categories
{
    public interface ICategoryService
    {
        ValueTask<Category> AddCategoryAsync(Category category);
        IQueryable<Category> RetrieveAllCategories();
        ValueTask<Category> RetrieveCategoryByIdAsync(Guid categoryId);
        ValueTask<Category> ModifyCategoryAsync(Category category);
        ValueTask<Category> RemoveCategoryByIdAsync(Guid categoryId);
    }
}
