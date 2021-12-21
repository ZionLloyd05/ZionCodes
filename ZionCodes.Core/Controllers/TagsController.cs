using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZionCodes.Core.Dtos.Generic;
using ZionCodes.Core.Models.Tags;
using ZionCodes.Core.Services.Tags;
using ZionCodes.Core.Utils;

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
        public ActionResult<ICollection<Tag>> GetAllTags([FromQuery] PaginationQuery paginationQuery) =>
        TryCatchTagFunction(() =>
        {
            ICollection<Tag> storageTag =
                    this.tagService.RetrieveAllTags();


            if (paginationQuery != null)
            {
                PaginationFilter filter = new()
                {
                    PageNumber = paginationQuery.PageNumber,
                    PageSize = paginationQuery.PageSize,
                };

                if (paginationQuery.PageNumber < 1 || paginationQuery.PageSize < 1)
                {
                    return Ok(new PagedResponse<ICollection<Tag>>(storageTag));
                }

                var paginationResponse = PaginationBuilder.CreatePaginatedResponse(filter, storageTag);

                return Ok(paginationResponse);
            }

            return Ok(storageTag);
        });

        [HttpGet("{tagId}")]
        public ValueTask<ActionResult<Tag>> GetTagAsync(int tagId) =>
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
        public ValueTask<ActionResult<Tag>> DeleteTagAsync(int tagId) =>
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
