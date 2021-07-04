using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using ZionCodes.Core.Models.Tags;
using ZionCodes.Core.Models.Tags.Exceptions;
using ZionCodes.Core.Services.Tags;

namespace ZionCodes.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : RESTFulController
    {
        private readonly ITagService tagService;

        public TagsController(ITagService tagService)
        {
            this.tagService = tagService;
        }


        [HttpPost]
        public async ValueTask<ActionResult<Tag>> PostTagAsync(Tag tag)
        {
            try
            {
                Tag persistedTag =
                    await this.tagService.AddTagAsync(tag);

                return Ok(persistedTag);
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
            {
                return Problem(tagDependencyException.Message);
            }
            catch (TagServiceException tagServiceException)
            {
                return Problem(tagServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Tag>> GetAllCategories()
        {
            try
            {
                IQueryable storageTag =
                    this.tagService.RetrieveAllTags();

                return Ok(storageTag);
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

        [HttpGet("{tagId}")]
        public async ValueTask<ActionResult<Tag>> GetTagAsync(Guid tagId)
        {
            try
            {
                Tag storageTag =
                    await this.tagService.RetrieveTagByIdAsync(tagId);

                return Ok(storageTag);
            }
            catch (TagValidationException tagValidationException)
                when (tagValidationException.InnerException is NotFoundTagException)
            {
                string innerMessage = GetInnerMessage(tagValidationException);

                return NotFound(innerMessage);
            }
            catch (TagValidationException tagValidationException)
            {
                string innerMessage = GetInnerMessage(tagValidationException);

                return BadRequest(innerMessage);
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

        [HttpPut]
        public async ValueTask<ActionResult<Tag>> PutTagAsync(Tag tag)
        {
            try
            {
                Tag registeredTag =
                    await this.tagService.ModifyTagAsync(tag);

                return Ok(registeredTag);
            }
            catch (TagValidationException tagValidationException)
                when (tagValidationException.InnerException is NotFoundTagException)
            {
                string innerMessage = GetInnerMessage(tagValidationException);

                return NotFound(innerMessage);
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

        [HttpDelete("{tagId}")]
        public async ValueTask<ActionResult<Tag>> DeleteTagAsync(Guid tagId)
        {
            try
            {
                Tag storageTag =
                    await this.tagService.RemoveTagByIdAsync(tagId);

                return Ok(storageTag);
            }
            catch (TagValidationException tagValidationException)
                when (tagValidationException.InnerException is NotFoundTagException)
            {
                string innerMessage = GetInnerMessage(tagValidationException);

                return NotFound(innerMessage);
            }
            catch (TagValidationException tagValidationException)
            {
                string innerMessage = GetInnerMessage(tagValidationException);

                return BadRequest(tagValidationException);
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

        #region HelperMethods
        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
        #endregion
    }
}
