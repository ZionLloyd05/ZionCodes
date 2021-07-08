using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZionCodes.Core.Models.Categories;
using ZionCodes.Core.Services.Categories;

namespace ZionCodes.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseController<Category>
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpPost]
        public ValueTask<ActionResult<Category>> PostCategoryAsync(Category category) =>
        TryCatchCategoryFunction(async () =>
        {
            Category persistedCategory =
                    await this.categoryService.AddCategoryAsync(category);

            return Ok(persistedCategory);
        });

        [HttpGet]
        public ActionResult<IQueryable<Category>> GetAllCategories() =>
        TryCatchCategoryFunction(() =>
        {
            IQueryable storageCategory =
                    this.categoryService.RetrieveAllCategories();

            return Ok(storageCategory);
        });

        [HttpGet("{categoryId}")]
        public ValueTask<ActionResult<Category>> GetCategoryAsync(Guid categoryId) =>
        TryCatchCategoryFunction(async () =>
        {
            Category storageCategory =
                   await this.categoryService.RetrieveCategoryByIdAsync(categoryId);

            return Ok(storageCategory);
        });

        [HttpPut]
        public ValueTask<ActionResult<Category>> PutCategoryAsync(Category category) =>
        TryCatchCategoryFunction(async () =>
        {
            Category registeredCategory =
                    await this.categoryService.ModifyCategoryAsync(category);

            return Ok(registeredCategory);
        });


        [HttpDelete("{categoryId}")]
        public ValueTask<ActionResult<Category>> DeleteCategoryAsync(Guid categoryId) =>
        TryCatchCategoryFunction(async () =>
        {
            Category storageCategory =
                    await this.categoryService.RemoveCategoryByIdAsync(categoryId);

            return Ok(storageCategory);
        });


        #region HelperMethods
        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
        #endregion

    }
}
