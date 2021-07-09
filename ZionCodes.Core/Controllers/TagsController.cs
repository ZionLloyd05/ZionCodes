using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZionCodes.Core.Models.Tags;
using ZionCodes.Core.Services.Tags;

namespace ZionCodes.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : BaseController<Tag>
    {
        private readonly ITagService tagService;

        public TagsController(ITagService tagService)
        {
            this.tagService = tagService;
        }


        [HttpPost]
        public ValueTask<ActionResult<Tag>> PostTagAsync(Tag tag) =>
        TryCatchTagFunction(async () =>
        {
            Tag persistedTag =
                    await this.tagService.AddTagAsync(tag);

            return Ok(persistedTag);
        });

        [HttpGet]
        public ActionResult<IQueryable<Tag>> GetAllTags() =>
        TryCatchTagFunction(() =>
        {
            IQueryable storageTag =
                    this.tagService.RetrieveAllTags();

            return Ok(storageTag);
        });

        [HttpGet("{tagId}")]
        public ValueTask<ActionResult<Tag>> GetTagAsync(Guid tagId) =>
        TryCatchTagFunction(async () =>
        {
            Tag storageTag =
                   await this.tagService.RetrieveTagByIdAsync(tagId);

            return Ok(storageTag);
        });

        [HttpPut]
        public ValueTask<ActionResult<Tag>> PutTagAsync(Tag tag) =>
        TryCatchTagFunction(async () =>
        {
            Tag registeredTag =
                    await this.tagService.ModifyTagAsync(tag);

            return Ok(registeredTag);
        });


        [HttpDelete("{tagId}")]
        public ValueTask<ActionResult<Tag>> DeleteTagAsync(Guid tagId) =>
        TryCatchTagFunction(async () =>
        {
            Tag storageTag =
                    await this.tagService.RemoveTagByIdAsync(tagId);

            return Ok(storageTag);
        });

        #region HelperMethods
        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
        #endregion
    }
}
