﻿using System.Threading.Tasks;
using ZionCodes.Core.Models.Comments;

namespace ZionCodes.Core.Services.Comments
{
    public interface ICommentService
    {
        ValueTask<Comment> AddCommentAsync(Comment comment);
    }
}
