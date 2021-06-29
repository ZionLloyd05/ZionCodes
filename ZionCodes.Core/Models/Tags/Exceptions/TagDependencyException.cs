using System;

namespace ZionCodes.Core.Models.Tags.Exceptions
{
    public class TagDependencyException : Exception
    {
        public TagDependencyException(Exception innerException)
           : base("Service dependency error occurred, category support.", innerException) { }
    }
}
