using System;

namespace ZionCodes.Core.Models.Comments.Exceptions
{
    public class AlreadyExistsCommentException : Exception
    {
        public AlreadyExistsCommentException(Exception innerException)
            : base("Comment with same id already exist", innerException)
        { }
    }
}
