using System;

namespace ZionCodes.Core.Models.Tags.Exceptions
{
    public class InvalidTagException : Exception
    {
        public InvalidTagException(string parameterName, object parameterValue)
           : base($"Invalid Tag, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}")
        { }
    }
}
