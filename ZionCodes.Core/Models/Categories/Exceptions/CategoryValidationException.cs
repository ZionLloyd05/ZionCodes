using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZionCodes.Core.Models.Categories.Exceptions
{
    public class CategoryValidationException : Exception
    {
        public CategoryValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException)
        {

        }
    }
}
