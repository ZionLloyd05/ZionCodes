using System;

namespace ZionCodes.Core.Models.Articles.Exceptions
{
    public class InvalidArticleException : Exception
    {
        public InvalidArticleException(string parameterName, object parameterValue)
           : base($"Invalid Article, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}")
        { }
    }
}
