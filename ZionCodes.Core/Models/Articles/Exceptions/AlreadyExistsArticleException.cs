using System;

namespace ZionCodes.Core.Models.Articles.Exceptions
{
    public class AlreadyExistsArticleException : Exception
    {
        public AlreadyExistsArticleException(Exception innerException)
            : base("Article with same id already exist", innerException)
        { }
    }
}
