using System;

namespace ZionCodes.Core.Models.Tags.Exceptions
{
    public class LockedTagException : Exception
    {
        public LockedTagException(Exception innerException)
           : base("Locked tag record exception, please try again later.", innerException) { }
    }
}
