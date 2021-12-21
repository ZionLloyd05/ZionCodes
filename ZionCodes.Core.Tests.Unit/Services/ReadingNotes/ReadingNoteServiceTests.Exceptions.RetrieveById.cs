using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using ZionCodes.Core.Models.ReadingNotes;
using ZionCodes.Core.Models.ReadingNotes.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.ReadingNotes
{
    public partial class ReadingNoteServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomReadingNoteId = 1;
            int inputReadingNoteId = randomReadingNoteId;
            SqlException sqlException = GetSqlException();

            var exceptionReadingNoteDependencyException =
                new ReadingNoteDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReadingNoteByIdAsync(inputReadingNoteId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ReadingNote> retrieveReadingNoteByIdTask =
                this.readingNoteService.RetrieveReadingNoteByIdAsync(inputReadingNoteId);

            // then
            await Assert.ThrowsAsync<ReadingNoteDependencyException>(() =>
                retrieveReadingNoteByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(exceptionReadingNoteDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReadingNoteByIdAsync(inputReadingNoteId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            int someReadingNoteId = 1;
            var databaseUpdateException = new DbUpdateException();

            var expectedReadingNoteDependencyException =
                new ReadingNoteDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReadingNoteByIdAsync(It.IsAny<int>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ReadingNote> retrieveByIdReadingNoteTask =
                this.readingNoteService.RetrieveReadingNoteByIdAsync(someReadingNoteId);

            // then
            await Assert.ThrowsAsync<ReadingNoteDependencyException>(() =>
                retrieveByIdReadingNoteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedReadingNoteDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReadingNoteByIdAsync(It.IsAny<int>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
         ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            int someReadingNoteId = 1;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedReadingNoteException =
                new LockedReadingNoteException(databaseUpdateConcurrencyException);

            var expectedReadingNoteDependencyException =
                new ReadingNoteDependencyException(lockedReadingNoteException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReadingNoteByIdAsync(It.IsAny<int>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ReadingNote> retrieveByIdReadingNoteTask =
                this.readingNoteService.RetrieveReadingNoteByIdAsync(someReadingNoteId);

            // then
            await Assert.ThrowsAsync<ReadingNoteDependencyException>(() =>
                retrieveByIdReadingNoteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedReadingNoteDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReadingNoteByIdAsync(It.IsAny<int>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            int someReadingNoteId = 1;
            var exception = new Exception();

            var expectedReadingNoteServiceException =
                new ReadingNoteServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReadingNoteByIdAsync(It.IsAny<int>()))
                    .ThrowsAsync(exception);

            // when
            ValueTask<ReadingNote> retrieveByIdReadingNoteTask =
                this.readingNoteService.RetrieveReadingNoteByIdAsync(someReadingNoteId);

            // then
            await Assert.ThrowsAsync<ReadingNoteServiceException>(() =>
                retrieveByIdReadingNoteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedReadingNoteServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReadingNoteByIdAsync(It.IsAny<int>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
