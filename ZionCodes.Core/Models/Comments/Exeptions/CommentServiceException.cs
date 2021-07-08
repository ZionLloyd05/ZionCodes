using System;

namespace ZionCodes.Core.Models.Comments.Exceptions
{
    public class CommentServiceException : Exception
    {
        public CommentServiceException(Exception innerException)
            : base("Service error occurred, contact support.", innerException) { }
    }
}
