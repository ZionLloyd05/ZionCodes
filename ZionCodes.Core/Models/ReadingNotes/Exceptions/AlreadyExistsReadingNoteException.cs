using System;

namespace ZionCodes.Core.Models.ReadingNotes.Exceptions
{
    public class AlreadyExistsReadingNoteException : Exception
    {
        public AlreadyExistsReadingNoteException(Exception innerException)
            : base("Reading note with same id already exist", innerException)
        { }
    }
}
