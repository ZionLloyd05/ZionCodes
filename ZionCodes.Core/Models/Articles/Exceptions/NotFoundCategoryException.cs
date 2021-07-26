using System;

namespace ZionCodes.Core.Models.Articles.Exceptions
{
    public class NotFoundArticleException : Exception
    {
        public NotFoundArticleException(Guid articleId)
           : base($"Couldn't find article with Id: {articleId}.") { }
    }
}
