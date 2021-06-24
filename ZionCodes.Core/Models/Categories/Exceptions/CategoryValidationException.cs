using System;

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
