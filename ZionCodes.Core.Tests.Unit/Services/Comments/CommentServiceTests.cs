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
using ZionCodes.Core.Models.Comments;
using ZionCodes.Core.Services.Comments;

namespace ZionCodes.Core.Tests.Unit.Services.Comments
{
    public partial class CommentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ICommentService commentService;

        public CommentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.commentService = new CommentService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Comment CreateRandomComment(DateTimeOffset dateTime) =>
            CreateRandomCommentFiller(dateTime).Create();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static ICollection<Comment> CreateRandomComments(DateTimeOffset dateTime) =>
            CreateRandomCommentFiller(dateTime).Create(GetRandomNumber()).ToList();

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static Filler<Comment> CreateRandomCommentFiller(DateTimeOffset dateTime)
        {
            Filler<Comment> filler = new Filler<Comment>();

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