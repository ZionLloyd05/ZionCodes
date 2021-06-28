using System;

namespace ZionCodes.Core.Models.Tags.Exceptions
{
    public class NullTagException : Exception
    {
        public NullTagException() :
            base("The tag is null.")
        { }

    }
}
