using System;

namespace ZionCodes.Core.Models.Tags.Exceptions
{
    public class TagServiceException : Exception
    {
        public TagServiceException(Exception innerException)
            : base("Service error occurred, contact support.", innerException) { }
    }
}
