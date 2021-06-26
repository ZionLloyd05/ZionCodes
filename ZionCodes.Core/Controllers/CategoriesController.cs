using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using ZionCodes.Core.Models.Categories;
using ZionCodes.Core.Models.Categories.Exceptions;
using ZionCodes.Core.Services.Categories;

namespace ZionCodes.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : RESTFulController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<Category>> PostCategoryAsync(Category category)
        {
            try
            {
                Category persistedCategory =
                    await this.categoryService.AddCategoryAsync(category);

                return Ok(persistedCategory);
            }
            catch (CategoryValidationException categoryValidationException)
                when (categoryValidationException.InnerException is AlreadyExistsCategoryException)
            {
                string innerMessage = GetInnerMessage(categoryValidationException);

                return Conflict(innerMessage);
            }
            catch (CategoryValidationException categoryValidationException)
            {
                string innerMessage = GetInnerMessage(categoryValidationException);

                return BadRequest(innerMessage);
            }
            catch (CategoryDependencyException categoryDependencyException)
            {
                return Problem(categoryDependencyException.Message);
            }
            catch (CategoryServiceException categoryServiceException)
            {
                return Problem(categoryServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Category>> GetAllCategories()
        {
            try
            {
                IQueryable storageCategory =
                    this.categoryService.RetrieveAllCategories();

                return Ok(storageCategory);
            }
            catch (CategoryDependencyException categoryDependencyException)
            {
                return Problem(categoryDependencyException.Message);
            }
            catch (CategoryServiceException categoryServiceException)
            {
                return Problem(categoryServiceException.Message);
            }
        }

        [HttpGet("{categoryId}")]
        public async ValueTask<ActionResult<Category>> GetCategoryAsync(Guid categoryId)
        {
            try
            {
                Category storageCategory =
                    await this.categoryService.RetrieveCategoryByIdAsync(categoryId);

                return Ok(storageCategory);
            }
            catch (CategoryValidationException categoryValidationException)
                when (categoryValidationException.InnerException is NotFoundCategoryException)
            {
                string innerMessage = GetInnerMessage(categoryValidationException);

                return NotFound(innerMessage);
            }
            catch (CategoryValidationException categoryValidationException)
            {
                string innerMessage = GetInnerMessage(categoryValidationException);

                return BadRequest(innerMessage);
            }
            catch (CategoryDependencyException categoryDependencyException)
            {
                return Problem(categoryDependencyException.Message);
            }
            catch (CategoryServiceException categoryServiceException)
            {
                return Problem(categoryServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Category>> PutCategoryAsync(Category category)
        {
            try
            {
                Category registeredCategory =
                    await this.categoryService.ModifyCategoryAsync(category);

                return Ok(registeredCategory);
            }
            catch (CategoryValidationException categoryValidationException)
                when (categoryValidationException.InnerException is NotFoundCategoryException)
            {
                string innerMessage = GetInnerMessage(categoryValidationException);

                return NotFound(innerMessage);
            }
            catch (CategoryValidationException categoryValidationException)
            {
                string innerMessage = GetInnerMessage(categoryValidationException);

                return BadRequest(innerMessage);
            }
            catch (CategoryDependencyException categoryDependencyException)
                when (categoryDependencyException.InnerException is LockedCategoryException)
            {
                string innerMessage = GetInnerMessage(categoryDependencyException);

                return Locked(innerMessage);
            }
            catch (CategoryDependencyException categoryDependencyException)
            {
                return Problem(categoryDependencyException.Message);
            }
            catch (CategoryServiceException categoryServiceException)
            {
                return Problem(categoryServiceException.Message);
            }
        }


        [HttpDelete("{categoryId}")]
        public async ValueTask<ActionResult<Category>> DeleteCategoryAsync(Guid categoryId)
        {
            try
            {
                Category storageCategory =
                    await this.categoryService.RemoveCategoryByIdAsync(categoryId);

                return Ok(storageCategory);
            }
            catch (CategoryValidationException categoryValidationException)
                when (categoryValidationException.InnerException is NotFoundCategoryException)
            {
                string innerMessage = GetInnerMessage(categoryValidationException);

                return NotFound(innerMessage);
            }
            catch (CategoryValidationException categoryValidationException)
            {
                string innerMessage = GetInnerMessage(categoryValidationException);

                return BadRequest(categoryValidationException);
            }
            catch (CategoryDependencyException categoryDependencyException)
               when (categoryDependencyException.InnerException is LockedCategoryException)
            {
                string innerMessage = GetInnerMessage(categoryDependencyException);

                return Locked(innerMessage);
            }
            catch (CategoryDependencyException categoryDependencyException)
            {
                return Problem(categoryDependencyException.Message);
            }
            catch (CategoryServiceException categoryServiceException)
            {
                return Problem(categoryServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
