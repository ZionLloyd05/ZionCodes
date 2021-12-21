using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Comments;

namespace ZionCodes.Core.Services.Comments
{
    public interface ICommentService
    {
        ValueTask<Comment> AddCommentAsync(Comment comment);
        ICollection<Comment> RetrieveAllComments();
        ValueTask<Comment> RetrieveCommentByIdAsync(int commentId);
        ValueTask<Comment> ModifyCommentAsync(Comment comment);
        ValueTask<Comment> RemoveCommentByIdAsync(int commentId);
    }
}
