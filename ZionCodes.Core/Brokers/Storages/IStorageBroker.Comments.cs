using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Comments;

namespace ZionCodes.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        public ValueTask<Comment> InsertCommentAsync(Comment comment);
        public ICollection<Comment> SelectAllComments();
        public ValueTask<Comment> SelectCommentByIdAsync(int commentId);
        public ValueTask<Comment> UpdateCommentAsync(Comment comment);
        public ValueTask<Comment> DeleteCommentAsync(Comment comment);
    }
}
