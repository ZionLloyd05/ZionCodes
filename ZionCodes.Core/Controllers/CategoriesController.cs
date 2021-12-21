using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZionCodes.Core.Dtos.Generic;
using ZionCodes.Core.Models.Categories;
using ZionCodes.Core.Services.Categories;
using ZionCodes.Core.Utils;

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
        public ActionResult<ICollection<Category>> GetAllCategories([FromQuery] PaginationQuery paginationQuery) =>
        TryCatchCategoryFunction(() =>
        {
            ICollection<Category> storageCategory =
                    this.categoryService.RetrieveAllCategories();

            if (paginationQuery != null)
            {
                PaginationFilter filter = new()
                {
                    PageNumber = paginationQuery.PageNumber,
                    PageSize = paginationQuery.PageSize,
                };

                if (paginationQuery.PageNumber < 1 || paginationQuery.PageSize < 1)
                {
                    return Ok(new PagedResponse<ICollection<Category>>(storageCategory));
                }

                var paginationResponse = PaginationBuilder.CreatePaginatedResponse(filter, storageCategory);

                return Ok(paginationResponse);
            }

            return Ok(storageCategory);
        });

        [HttpGet("{categoryId}")]
        public ValueTask<ActionResult<Category>> GetCategoryAsync(int categoryId) =>
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
        public ValueTask<ActionResult<Category>> DeleteCategoryAsync(int categoryId) =>
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
