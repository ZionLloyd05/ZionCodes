using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Categories;

namespace ZionCodes.Core.Services.Categories
{
    public interface ICategoryService
    {
        ValueTask<Category> AddCategoryAsync(Category category);
        ICollection<Category> RetrieveAllCategories();
        ValueTask<Category> RetrieveCategoryByIdAsync(int categoryId);
        ValueTask<Category> ModifyCategoryAsync(Category category);
        ValueTask<Category> RemoveCategoryByIdAsync(int categoryId);
    }
}
