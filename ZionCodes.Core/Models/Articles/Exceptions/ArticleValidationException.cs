using System;

namespace ZionCodes.Core.Models.Articles.Exceptions
{
    public class ArticleValidationException : Exception
    {
        public ArticleValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException)
        {

        }
    }
}
