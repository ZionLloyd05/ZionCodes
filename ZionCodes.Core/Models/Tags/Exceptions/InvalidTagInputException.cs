using System;

namespace ZionCodes.Core.Models.Tags.Exceptions
{
    public class InvalidTagInputException : Exception
    {
        public InvalidTagInputException(string parameterName, object parameterValue)
              : base($"Invalid Tag, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
