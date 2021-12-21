using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Brokers.DateTimes;
using ZionCodes.Core.Brokers.Loggings;
using ZionCodes.Core.Brokers.Storages;
using ZionCodes.Core.Models.ReadingNotes;

namespace ZionCodes.Core.Services.ReadingNotes
{
    public partial class ReadingNoteService : IReadingNoteService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ReadingNoteService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker
            )
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ReadingNote> AddReadingNoteAsync(ReadingNote readingNote) =>
        TryCatch(() =>
        {
            ValidateReadingNoteOnCreate(readingNote);
            return this.storageBroker.InsertReadingNoteAsync(readingNote);
        });

        public ICollection<ReadingNote> RetrieveAllReadingNotes() =>
            TryCatch(() =>
            {
                ICollection<ReadingNote> storageReadingNotes =
                this.storageBroker.SelectAllReadingNotes();

                ValidateStorageReadingNotes(storageReadingNotes);

                return storageReadingNotes;
            });

        public ValueTask<ReadingNote> RetrieveReadingNoteByIdAsync(int readingNoteId) =>
            TryCatch(async () =>
            {
                ValidateReadingNoteId(readingNoteId);

                ReadingNote storageReadingNote =
                    await this.storageBroker.SelectReadingNoteByIdAsync(readingNoteId);

                ValidateStorageReadingNote(storageReadingNote, readingNoteId);

                return storageReadingNote;
            });

        public ValueTask<ReadingNote> ModifyReadingNoteAsync(ReadingNote readingNote) =>
            TryCatch(async () =>
            {
                ValidateReadingNoteOnModify(readingNote);
                ReadingNote maybeReadingNote =
                await this.storageBroker.SelectReadingNoteByIdAsync(readingNote.Id);

                return await this.storageBroker.UpdateReadingNoteAsync(readingNote);
            });

        public ValueTask<ReadingNote> RemoveReadingNoteByIdAsync(int readingNoteId) =>
            TryCatch(async () =>
            {
                ValidateReadingNoteIdIsNull(readingNoteId);

                ReadingNote storageReadingNote =
                await this.storageBroker.SelectReadingNoteByIdAsync(readingNoteId);

                ValidateStorageReadingNote(storageReadingNote, readingNoteId);

                return await this.storageBroker.DeleteReadingNoteAsync(storageReadingNote);
            });
    }
}