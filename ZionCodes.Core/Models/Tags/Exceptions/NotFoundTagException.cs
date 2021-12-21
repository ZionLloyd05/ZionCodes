using System;

namespace ZionCodes.Core.Models.Tags.Exceptions
{
    public class NotFoundTagException : Exception
    {
        public NotFoundTagException(int tagId)
           : base($"Couldn't find tag with Id: {tagId}.") { }
    }
}
