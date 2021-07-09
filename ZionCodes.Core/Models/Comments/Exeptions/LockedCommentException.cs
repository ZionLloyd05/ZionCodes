using System;

namespace ZionCodes.Core.Models.Comments.Exceptions
{
    public class LockedCommentException : Exception
    {
        public LockedCommentException(Exception innerException)
           : base("Locked comment record exception, please try again later.", innerException) { }
    }
}
