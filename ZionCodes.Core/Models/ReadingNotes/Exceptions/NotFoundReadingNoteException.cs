using System;

namespace ZionCodes.Core.Models.ReadingNotes.Exceptions
{
    public class NotFoundReadingNoteException : Exception
    {
        public NotFoundReadingNoteException(Guid readingNoteId)
           : base($"Couldn't find readingNote with Id: {readingNoteId}.") { }
    }
}
