using System;

namespace ZionCodes.Core.Models.Articles.Exceptions
{
    public class ArticleDependencyException : Exception
    {
        public ArticleDependencyException(Exception innerException)
           : base("Service dependency error occurred, article support.", innerException) { }
    }
}
