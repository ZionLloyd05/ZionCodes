using System;

namespace ZionCodes.Core.Models.Categories.Exceptions
{
    public class InvalidCategoryInputException : Exception
    {
        public InvalidCategoryInputException(string parameterName, object parameterValue)
              : base($"Invalid Fee, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
