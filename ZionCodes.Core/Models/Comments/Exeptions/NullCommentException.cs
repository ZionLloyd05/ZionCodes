using System;

namespace ZionCodes.Core.Models.Comments.Exceptions
{
    public class NullCommentException : Exception
    {
        public NullCommentException() :
            base("The comment is null.")
        { }

    }
}
