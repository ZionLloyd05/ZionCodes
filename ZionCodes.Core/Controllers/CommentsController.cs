using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZionCodes.Core.Models.Comments;
using ZionCodes.Core.Services.Comments;

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
        public ActionResult<IQueryable<Comment>> GetAllComments() =>
        TryCatchCommentFunction(() =>
        {
            IQueryable storageComment =
                    this.commentService.RetrieveAllComments();

            return Ok(storageComment);
        });

        [HttpGet("{commentId}")]
        public ValueTask<ActionResult<Comment>> GetCommentAsync(Guid commentId) =>
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
        public ValueTask<ActionResult<Comment>> DeleteCommentAsync(Guid commentId) =>
        TryCatchCommentFunction(async () =>
        {
            Comment storageComment =
                    await this.commentService.RemoveCommentByIdAsync(commentId);

            return Ok(storageComment);
        });
    }
}
