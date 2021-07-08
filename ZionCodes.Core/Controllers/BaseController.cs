using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using ZionCodes.Core.Models.Categories.Exceptions;
using ZionCodes.Core.Models.Tags.Exceptions;

namespace ZionCodes.Core.Controllers
{
    public class BaseController<T> : RESTFulController
    {
        protected delegate ValueTask<ActionResult<T>> ReturningSingleControllerFunction();
        protected delegate ActionResult<IQueryable<T>> ReturningMultipleControllerFunction();

        #region CategoriesControllerFunction
        protected async ValueTask<ActionResult<T>> TryCatchCategoryFunction(
            ReturningSingleControllerFunction returningControllerFunction)
        {
            try
            {
                return await returningControllerFunction();
            }
            catch (CategoryValidationException categoryValidationException)
                when (categoryValidationException.InnerException is NotFoundCategoryException)
            {
                string innerMessage = GetInnerMessage(categoryValidationException);

                return NotFound(innerMessage);
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

        protected ActionResult<IQueryable<T>> TryCatchCategoryFunction(
            ReturningMultipleControllerFunction returningControllerFunction)
        {
            try
            {
                return returningControllerFunction();
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
        #endregion

        #region TagsControllerFunction
        protected async ValueTask<ActionResult<T>> TryCatchTagFunction(
            ReturningSingleControllerFunction returningControllerFunction)
        {
            try
            {
                return await returningControllerFunction();
            }
            catch (TagValidationException tagValidationException)
                when (tagValidationException.InnerException is NotFoundTagException)
            {
                string innerMessage = GetInnerMessage(tagValidationException);

                return NotFound(innerMessage);
            }
            catch (TagValidationException tagValidationException)
                when (tagValidationException.InnerException is AlreadyExistsTagException)
            {
                string innerMessage = GetInnerMessage(tagValidationException);

                return Conflict(innerMessage);
            }
            catch (TagValidationException tagValidationException)
            {
                string innerMessage = GetInnerMessage(tagValidationException);

                return BadRequest(innerMessage);
            }
            catch (TagDependencyException tagDependencyException)
                when (tagDependencyException.InnerException is LockedTagException)
            {
                string innerMessage = GetInnerMessage(tagDependencyException);

                return Locked(innerMessage);
            }
            catch (TagDependencyException tagDependencyException)
            {
                return Problem(tagDependencyException.Message);
            }
            catch (TagServiceException tagServiceException)
            {
                return Problem(tagServiceException.Message);
            }

        }

        protected ActionResult<IQueryable<T>> TryCatchTagFunction(
            ReturningMultipleControllerFunction returningControllerFunction)
        {
            try
            {
                return returningControllerFunction();
            }
            catch (TagDependencyException tagDependencyException)
            {
                return Problem(tagDependencyException.Message);
            }
            catch (TagServiceException tagServiceException)
            {
                return Problem(tagServiceException.Message);
            }
        }
        #endregion

        #region HelperMethods
        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
        #endregion
    }
}
