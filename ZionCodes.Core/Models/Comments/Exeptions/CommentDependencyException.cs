using System;

namespace ZionCodes.Core.Models.Comments.Exceptions
{
    public class CommentDependencyException : Exception
    {
        public CommentDependencyException(Exception innerException)
           : base("Service dependency error occurred, category support.", innerException) { }
    }
}
