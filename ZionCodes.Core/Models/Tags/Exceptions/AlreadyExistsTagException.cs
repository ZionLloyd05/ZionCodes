using System;

namespace ZionCodes.Core.Models.Tags.Exceptions
{
    public class AlreadyExistsTagException : Exception
    {
        public AlreadyExistsTagException(Exception innerException)
            : base("Tag with same id already exist", innerException)
        { }
    }
}
