﻿using System;
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
            ValidateReadingNoteIsNull(readingNote);
            return this.storageBroker.InsertReadingNoteAsync(readingNote);
        });
    }
}