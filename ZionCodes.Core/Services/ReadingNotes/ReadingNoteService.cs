using System;
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

        public IQueryable<ReadingNote> RetrieveAllReadingNotes() =>
            TryCatch(() =>
            {
                IQueryable<ReadingNote> storageReadingNotes =
                this.storageBroker.SelectAllReadingNotes();

                ValidateStorageReadingNotes(storageReadingNotes);

                return storageReadingNotes;
            });

        public ValueTask<ReadingNote> RetrieveReadingNoteByIdAsync(Guid readingNoteId) =>
            TryCatch(async () =>
            {
                ValidateReadingNoteId(readingNoteId);

                ReadingNote storageReadingNote =
                    await this.storageBroker.SelectReadingNoteByIdAsync(readingNoteId);

                ValidateStorageReadingNote(storageReadingNote, readingNoteId);

                return storageReadingNote;
            });

        public ValueTask<ReadingNote> ModifyReadingNoteAsync(ReadingNote readingNote)
        {
            throw new NotImplementedException();
        }
    }
}