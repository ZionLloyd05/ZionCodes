using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ZionCodes.Core.Models.ReadingNotes;

namespace ZionCodes.Core.Brokers.Storages
{
    public partial class StorageBroker : IStorageBroker
    {
        public DbSet<ReadingNote> ReadingNotes { get; set; }

        public async ValueTask<ReadingNote> InsertReadingNoteAsync(ReadingNote readingNote)
        {
            EntityEntry<ReadingNote> readingNoteEntityEntry = await this.ReadingNotes.AddAsync(readingNote);
            await this.SaveChangesAsync();

            return readingNoteEntityEntry.Entity;
        }

        public ICollection<ReadingNote> SelectAllReadingNotes() => this.ReadingNotes.ToList();

        public async ValueTask<ReadingNote> SelectReadingNoteByIdAsync(int readingNoteId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await ReadingNotes.FindAsync(readingNoteId);
        }

        public async ValueTask<ReadingNote> UpdateReadingNoteAsync(ReadingNote readingNote)
        {
            EntityEntry<ReadingNote> readingNoteEntityEntry = this.ReadingNotes.Update(readingNote);
            await this.SaveChangesAsync();

            return readingNoteEntityEntry.Entity;
        }

        public async ValueTask<ReadingNote> DeleteReadingNoteAsync(ReadingNote readingNote)
        {
            EntityEntry<ReadingNote> readingNoteEntityEntry = this.ReadingNotes.Remove(readingNote);
            await this.SaveChangesAsync();

            return readingNoteEntityEntry.Entity;
        }
    }
}
