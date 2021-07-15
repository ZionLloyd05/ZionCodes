using System;

namespace ZionCodes.Core.Models.ReadingNotes.Exceptions
{
    public class LockedReadingNoteException : Exception
    {
        public LockedReadingNoteException(Exception innerException)
           : base("Locked reading note record exception, please try again later.", innerException) { }
    }
}
