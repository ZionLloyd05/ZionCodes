using System;

namespace ZionCodes.Core.Models.ReadingNotes.Exceptions
{
    public class ReadingNoteServiceException : Exception
    {
        public ReadingNoteServiceException(Exception innerException)
            : base("Service error occurred, contact support.", innerException) { }
    }
}
