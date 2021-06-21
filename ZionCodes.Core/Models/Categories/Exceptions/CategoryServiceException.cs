using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZionCodes.Core.Models.Categories.Exceptions
{
    public class CategoryServiceException : Exception
    {
        public CategoryServiceException(Exception innerException)
            : base("Service error occurred, contact support.", innerException) { }
    }
}
