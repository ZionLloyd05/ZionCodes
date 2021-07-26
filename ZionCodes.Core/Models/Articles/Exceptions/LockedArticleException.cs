using System;

namespace ZionCodes.Core.Models.Articles.Exceptions
{
    public class LockedArticleException : Exception
    {
        public LockedArticleException(Exception innerException)
           : base("Locked article record exception, please try again later.", innerException) { }
    }
}
