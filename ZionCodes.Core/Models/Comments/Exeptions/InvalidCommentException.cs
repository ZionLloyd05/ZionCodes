using System;

namespace ZionCodes.Core.Models.Comments.Exceptions
{
    public class InvalidCommentException : Exception
    {
        public InvalidCommentException(string parameterName, object parameterValue)
           : base($"Invalid Comment, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}")
        { }
    }
}
