using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
