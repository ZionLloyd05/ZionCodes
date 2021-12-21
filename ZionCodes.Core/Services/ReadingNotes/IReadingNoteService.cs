using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.ReadingNotes;

namespace ZionCodes.Core.Services.ReadingNotes
{
    public interface IReadingNoteService
    {
        ValueTask<ReadingNote> AddReadingNoteAsync(ReadingNote readingNote);
        ICollection<ReadingNote> RetrieveAllReadingNotes();
        ValueTask<ReadingNote> RetrieveReadingNoteByIdAsync(int readingNoteId);
        ValueTask<ReadingNote> ModifyReadingNoteAsync(ReadingNote readingNote);
        ValueTask<ReadingNote> RemoveReadingNoteByIdAsync(int readingNoteId);
    }
}
