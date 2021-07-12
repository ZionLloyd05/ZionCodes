using System;

namespace ZionCodes.Core.Models.ReadingNotes.Exceptions
{
    public class NullReadingNoteException : Exception
    {
        public NullReadingNoteException() :
            base("The reading note is null.")
        { }

    }
}
