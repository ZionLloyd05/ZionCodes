using System;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.ReadingNotes;

namespace ZionCodes.Core.Services.ReadingNotes
{
    public interface IReadingNoteService
    {
        ValueTask<ReadingNote> AddReadingNoteAsync(ReadingNote readingNote);
        IQueryable<ReadingNote> RetrieveAllReadingNotes();
        ValueTask<ReadingNote> RetrieveReadingNoteByIdAsync(Guid readingNoteId);
        ValueTask<ReadingNote> ModifyReadingNoteAsync(ReadingNote readingNote);
        ValueTask<ReadingNote> RemoveReadingNoteByIdAsync(Guid readingNoteId);
    }
}
