using System;

namespace ZionCodes.Core.Models.Tags.Exceptions
{
    public class TagValidationException : Exception
    {
        public TagValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException)
        {

        }
    }
}
