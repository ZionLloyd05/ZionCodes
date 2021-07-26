using System;

namespace ZionCodes.Core.Models.Articles.Exceptions
{
    public class ArticleServiceException : Exception
    {
        public ArticleServiceException(Exception innerException)
            : base("Service error occurred, contact support.", innerException) { }
    }
}
