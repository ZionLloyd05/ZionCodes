using System;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.ReadingNotes;

namespace ZionCodes.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        public ValueTask<ReadingNote> InsertReadingNoteAsync(ReadingNote readingNote);
        public IQueryable<ReadingNote> SelectAllReadingNotes();
        public ValueTask<ReadingNote> SelectReadingNoteByIdAsync(Guid readingNoteId);
        public ValueTask<ReadingNote> UpdateReadingNoteAsync(ReadingNote readingNote);
        public ValueTask<ReadingNote> DeleteReadingNoteAsync(ReadingNote readingNote);
    }
}
