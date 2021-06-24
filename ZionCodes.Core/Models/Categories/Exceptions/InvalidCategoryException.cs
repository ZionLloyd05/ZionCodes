using System;

namespace ZionCodes.Core.Models.Categories.Exceptions
{
    public class InvalidCategoryException : Exception
    {
        public InvalidCategoryException(string parameterName, object parameterValue)
           : base($"Invalid Category, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}")
        { }
    }
}
