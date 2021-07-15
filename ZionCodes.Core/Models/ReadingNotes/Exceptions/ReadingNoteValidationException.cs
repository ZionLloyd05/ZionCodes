using System;

namespace ZionCodes.Core.Models.ReadingNotes.Exceptions
{
    public class ReadingNoteValidationException : Exception
    {
        public ReadingNoteValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException)
        {

        }
    }
}
