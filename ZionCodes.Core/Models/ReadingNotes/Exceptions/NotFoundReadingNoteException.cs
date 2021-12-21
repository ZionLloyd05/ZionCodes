using System;

namespace ZionCodes.Core.Models.ReadingNotes.Exceptions
{
    public class NotFoundReadingNoteException : Exception
    {
        public NotFoundReadingNoteException(int readingNoteId)
           : base($"Couldn't find readingNote with Id: {readingNoteId}.") { }
    }
}
