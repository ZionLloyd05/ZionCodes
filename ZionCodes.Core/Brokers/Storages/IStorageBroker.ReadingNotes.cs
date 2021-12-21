using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.ReadingNotes;

namespace ZionCodes.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        public ValueTask<ReadingNote> InsertReadingNoteAsync(ReadingNote readingNote);
        public ICollection<ReadingNote> SelectAllReadingNotes();
        public ValueTask<ReadingNote> SelectReadingNoteByIdAsync(int readingNoteId);
        public ValueTask<ReadingNote> UpdateReadingNoteAsync(ReadingNote readingNote);
        public ValueTask<ReadingNote> DeleteReadingNoteAsync(ReadingNote readingNote);
    }
}
