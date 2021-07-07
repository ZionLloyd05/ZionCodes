using System;

namespace ZionCodes.Core.Models.Comments.Exceptions
{
    public class CommentValidationException : Exception
    {
        public CommentValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException)
        {

        }
    }
}
