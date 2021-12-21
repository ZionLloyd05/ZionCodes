using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZionCodes.Core.Dtos.Generic;
using ZionCodes.Core.Models.Comments;
using ZionCodes.Core.Services.Comments;
using ZionCodes.Core.Utils;

namespace ZionCodes.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : BaseController<Comment>
    {
        private readonly ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }


        [HttpPost]
        public ValueTask<ActionResult<Comment>> PostCommentAsync(Comment comment) =>
        TryCatchCommentFunction(async () =>
        {
            Comment persistedComment =
                    await this.commentService.AddCommentAsync(comment);

            return Ok(persistedComment);
        });

        [HttpGet]
        public ActionResult<ICollection<Comment>> GetAllComments([FromQuery] PaginationQuery paginationQuery) =>
        TryCatchCommentFunction(() =>
        {
            ICollection<Comment> storageComment =
                    this.commentService.RetrieveAllComments();

            if (paginationQuery != null)
            {
                PaginationFilter filter = new()
                {
                    PageNumber = paginationQuery.PageNumber,
                    PageSize = paginationQuery.PageSize,
                };

                if (paginationQuery.PageNumber < 1 || paginationQuery.PageSize < 1)
                {
                    return Ok(new PagedResponse<ICollection<Comment>>(storageComment));
                }

                var paginationResponse = PaginationBuilder.CreatePaginatedResponse(filter, storageComment);

                return Ok(paginationResponse);
            }

            return Ok(storageComment);
        });

        [HttpGet("{commentId}")]
        public ValueTask<ActionResult<Comment>> GetCommentAsync(int commentId) =>
        TryCatchCommentFunction(async () =>
        {
            Comment storageComment =
                   await this.commentService.RetrieveCommentByIdAsync(commentId);

            return Ok(storageComment);
        });

        [HttpPut]
        public ValueTask<ActionResult<Comment>> PutCommentAsync(Comment comment) =>
        TryCatchCommentFunction(async () =>
        {
            Comment registeredComment =
                    await this.commentService.ModifyCommentAsync(comment);

            return Ok(registeredComment);
        });


        [HttpDelete("{commentId}")]
        public ValueTask<ActionResult<Comment>> DeleteCommentAsync(int commentId) =>
        TryCatchCommentFunction(async () =>
        {
            Comment storageComment =
                    await this.commentService.RemoveCommentByIdAsync(commentId);

            return Ok(storageComment);
        });
    }
}
