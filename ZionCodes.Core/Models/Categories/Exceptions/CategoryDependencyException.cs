using System;

namespace ZionCodes.Core.Models.Categories.Exceptions
{
    public class CategoryDependencyException : Exception
    {
        public CategoryDependencyException(Exception innerException)
           : base("Service dependency error occurred, category support.", innerException) { }
    }
}
