using System;

namespace ZionCodes.Core.Models.Comments.Exceptions
{
    public class NotFoundCommentException : Exception
    {
        public NotFoundCommentException(Guid commentId)
           : base($"Couldn't find comment with Id: {commentId}.") { }
    }
}
