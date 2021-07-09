using System;

namespace ZionCodes.Core.Models.Comments.Exceptions
{
    public class InvalidCommentInputException : Exception
    {
        public InvalidCommentInputException(string parameterName, object parameterValue)
              : base($"Invalid Comment, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
