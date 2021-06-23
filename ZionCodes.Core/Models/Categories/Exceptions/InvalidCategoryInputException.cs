using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
