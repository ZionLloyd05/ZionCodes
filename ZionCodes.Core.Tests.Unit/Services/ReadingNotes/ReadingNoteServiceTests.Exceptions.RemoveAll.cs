using System;
using Moq;
using Xunit;
using ZionCodes.Core.Models.ReadingNotes.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.ReadingNotes
{
    public partial class ReadingNoteServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllReadingNotesWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedReadingNoteDependencyException =
                new ReadingNoteDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllReadingNotes())
                    .Throws(sqlException);

            // when . then
            Assert.Throws<ReadingNoteDependencyException>(() =>
                this.readingNoteService.RetrieveAllReadingNotes());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllReadingNotes(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedReadingNoteDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllReadingNotesWhenExceptionOccursAndLogIt()
        {
            // given
            var exception = new Exception();

            var expectedReadingNoteServiceException =
                new ReadingNoteServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllReadingNotes())
                    .Throws(exception);

            // when . then
            Assert.Throws<ReadingNoteServiceException>(() =>
                this.readingNoteService.RetrieveAllReadingNotes());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllReadingNotes(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedReadingNoteServiceException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
