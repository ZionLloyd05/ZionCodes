using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using ZionCodes.Core.Brokers.DateTimes;
using ZionCodes.Core.Brokers.Loggings;
using ZionCodes.Core.Brokers.Storages;
using ZionCodes.Core.Models.ReadingNotes;
using ZionCodes.Core.Services.ReadingNotes;

namespace ZionCodes.Core.Tests.Unit.Services.ReadingNotes
{
    public partial class ReadingNoteServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IReadingNoteService readingNoteService;

        public ReadingNoteServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.readingNoteService = new ReadingNoteService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static ReadingNote CreateRandomReadingNote(DateTimeOffset dateTime) =>
            CreateRandomReadingNoteFiller(dateTime).Create();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static ICollection<ReadingNote> CreateRandomReadingNotes(DateTimeOffset dateTime) =>
            CreateRandomReadingNoteFiller(dateTime).Create(GetRandomNumber()).ToList();

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static Filler<ReadingNote> CreateRandomReadingNoteFiller(DateTimeOffset dateTime)
        {
            Filler<ReadingNote> filler = new Filler<ReadingNote>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTime);

            return filler;
        }

        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private Expression<Func<Exception, bool>> SameExceptionAs(Exception exceptionException)
        {
            return actualException =>
                exceptionException.Message == actualException.Message &&
                exceptionException.InnerException.Message == actualException.InnerException.Message;
        }

        public static IEnumerable<object[]> InvalidMinuteCases()
        {
            int randomMoreThanMinuteFromNow = GetRandomNumber();
            int randomMoreThanMinuteBeforeNow = GetNegativeRandomNumber();

            return new List<object[]>
            {
                new object[] { randomMoreThanMinuteFromNow },
                new object[] { randomMoreThanMinuteBeforeNow }
            };
        }

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();
    }
}
