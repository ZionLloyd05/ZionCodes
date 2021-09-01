using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using ZionCodes.Core.Models.Articles.Exceptions;
using ZionCodes.Core.Models.Categories.Exceptions;
using ZionCodes.Core.Models.Comments.Exceptions;
using ZionCodes.Core.Models.Tags.Exceptions;
using ZionCodes.Web.Api.Models.Users.Exceptions;

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

        #region CommentsControllerFunction
        protected async ValueTask<ActionResult<T>> TryCatchCommentFunction(
            ReturningSingleControllerFunction returningControllerFunction)
        {
            try
            {
                return await returningControllerFunction();
            }
            catch (CommentValidationException commentValidationException)
                when (commentValidationException.InnerException is NotFoundCommentException)
            {
                string innerMessage = GetInnerMessage(commentValidationException);

                return NotFound(innerMessage);
            }
            catch (CommentValidationException commentValidationException)
                when (commentValidationException.InnerException is AlreadyExistsCommentException)
            {
                string innerMessage = GetInnerMessage(commentValidationException);

                return Conflict(innerMessage);
            }
            catch (CommentValidationException commentValidationException)
            {
                string innerMessage = GetInnerMessage(commentValidationException);

                return BadRequest(innerMessage);
            }
            catch (CommentDependencyException commentDependencyException)
                when (commentDependencyException.InnerException is LockedCommentException)
            {
                string innerMessage = GetInnerMessage(commentDependencyException);

                return Locked(innerMessage);
            }
            catch (CommentDependencyException commentDependencyException)
            {
                return Problem(commentDependencyException.Message);
            }
            catch (CommentServiceException commentServiceException)
            {
                return Problem(commentServiceException.Message);
            }

        }

        protected ActionResult<IQueryable<T>> TryCatchCommentFunction(
            ReturningMultipleControllerFunction returningControllerFunction)
        {
            try
            {
                return returningControllerFunction();
            }
            catch (CommentDependencyException commentDependencyException)
            {
                return Problem(commentDependencyException.Message);
            }
            catch (CommentServiceException commentServiceException)
            {
                return Problem(commentServiceException.Message);
            }
        }
        #endregion

        #region ArticlesControllerFunction
        protected async ValueTask<ActionResult<T>> TryCatchArticleFunction(
            ReturningSingleControllerFunction returningControllerFunction)
        {
            try
            {
                return await returningControllerFunction();
            }
            catch (ArticleValidationException articleValidationException)
                when (articleValidationException.InnerException is NotFoundArticleException)
            {
                string innerMessage = GetInnerMessage(articleValidationException);

                return NotFound(innerMessage);
            }
            catch (ArticleValidationException articleValidationException)
                when (articleValidationException.InnerException is AlreadyExistsArticleException)
            {
                string innerMessage = GetInnerMessage(articleValidationException);

                return Conflict(innerMessage);
            }
            catch (ArticleValidationException articleValidationException)
            {
                string innerMessage = GetInnerMessage(articleValidationException);

                return BadRequest(innerMessage);
            }
            catch (ArticleDependencyException articleDependencyException)
                when (articleDependencyException.InnerException is LockedArticleException)
            {
                string innerMessage = GetInnerMessage(articleDependencyException);

                return Locked(innerMessage);
            }
            catch (ArticleDependencyException articleDependencyException)
            {
                return Problem(articleDependencyException.Message);
            }
            catch (ArticleServiceException articleServiceException)
            {
                return Problem(articleServiceException.Message);
            }

        }

        protected ActionResult<IQueryable<T>> TryCatchArticleFunction(
            ReturningMultipleControllerFunction returningControllerFunction)
        {
            try
            {
                return returningControllerFunction();
            }
            catch (ArticleDependencyException articleDependencyException)
            {
                return Problem(articleDependencyException.Message);
            }
            catch (ArticleServiceException articleServiceException)
            {
                return Problem(articleServiceException.Message);
            }
        }
        #endregion

        #region UsersControllerFunction
        protected async ValueTask<ActionResult<T>> TryCatchUserFunction(
            ReturningSingleControllerFunction returningControllerFunction)
        {
            try
            {
                return await returningControllerFunction();
            }
            catch (UserValidationException userValidationException)
                when (userValidationException.InnerException is NotFoundUserException)
            {
                string innerMessage = GetInnerMessage(userValidationException);

                return NotFound(innerMessage);
            }
            catch (UserValidationException userValidationException)
                when (userValidationException.InnerException is AlreadyExistsUserException)
            {
                string innerMessage = GetInnerMessage(userValidationException);

                return Conflict(innerMessage);
            }
            catch (UserValidationException userValidationException)
            {
                string innerMessage = GetInnerMessage(userValidationException);

                return BadRequest(innerMessage);
            }
            catch (UserDependencyException userDependencyException)
                when (userDependencyException.InnerException is LockedUserException)
            {
                string innerMessage = GetInnerMessage(userDependencyException);

                return Locked(innerMessage);
            }
            catch (UserDependencyException userDependencyException)
            {
                return Problem(userDependencyException.Message);
            }
            catch (UserServiceException userServiceException)
            {
                return Problem(userServiceException.Message);
            }

        }

        protected ActionResult<IQueryable<T>> TryCatchUserFunction(
            ReturningMultipleControllerFunction returningControllerFunction)
        {
            try
            {
                return returningControllerFunction();
            }
            catch (UserDependencyException userDependencyException)
            {
                return Problem(userDependencyException.Message);
            }
            catch (UserServiceException userServiceException)
            {
                return Problem(userServiceException.Message);
            }
        }
        #endregion

        #region HelperMethods
        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
        #endregion
    }
}
