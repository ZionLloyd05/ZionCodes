using System;

namespace ZionCodes.Core.Models.Comments.Exceptions
{
    public class NotFoundCommentException : Exception
    {
        public NotFoundCommentException(int commentId)
           : base($"Couldn't find comment with Id: {commentId}.") { }
    }
}
