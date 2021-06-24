using System;

namespace ZionCodes.Core.Models.Categories.Exceptions
{
    public class CategoryServiceException : Exception
    {
        public CategoryServiceException(Exception innerException)
            : base("Service error occurred, contact support.", innerException) { }
    }
}
