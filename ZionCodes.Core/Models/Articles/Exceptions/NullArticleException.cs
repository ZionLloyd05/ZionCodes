using System;

namespace ZionCodes.Core.Models.Articles.Exceptions
{
    public class NullArticleException : Exception
    {
        public NullArticleException() :
            base("The article is null.")
        { }

    }
}
