using System;

namespace ZionCodes.Core.Models.Categories.Exceptions
{
    public class InvalidArticleInputException : Exception
    {
        public InvalidArticleInputException(string parameterName, object parameterValue)
              : base($"Invalid Article, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
