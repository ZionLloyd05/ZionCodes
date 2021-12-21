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
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomReadingNoteId = 1;
            int inputReadingNoteId = randomReadingNoteId;
            SqlException sqlException = GetSqlException();

            var expectedReadingNoteDependencyException =
                new ReadingNoteDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReadingNoteByIdAsync(inputReadingNoteId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ReadingNote> deleteReadingNoteTask =
                this.readingNoteService.RemoveReadingNoteByIdAsync(inputReadingNoteId);

            // then
            await Assert.ThrowsAsync<ReadingNoteDependencyException>(() =>
                deleteReadingNoteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedReadingNoteDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReadingNoteByIdAsync(inputReadingNoteId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteReadingNoteAsync(It.IsAny<ReadingNote>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            int randomReadingNoteId = 1;
            int inputReadingNoteId = randomReadingNoteId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedReadingNoteException =
                new LockedReadingNoteException(databaseUpdateConcurrencyException);

            var expectedReadingNoteException =
                new ReadingNoteDependencyException(lockedReadingNoteException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReadingNoteByIdAsync(inputReadingNoteId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ReadingNote> deleteReadingNoteTask =
                this.readingNoteService.RemoveReadingNoteByIdAsync(inputReadingNoteId);

            // then
            await Assert.ThrowsAsync<ReadingNoteDependencyException>(() => deleteReadingNoteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedReadingNoteException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReadingNoteByIdAsync(inputReadingNoteId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            int randomReadingNoteId = 1;
            int inputReadingNoteId = randomReadingNoteId;
            var exception = new Exception();

            var expectedReadingNoteException =
                new ReadingNoteServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReadingNoteByIdAsync(inputReadingNoteId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<ReadingNote> deleteReadingNoteTask =
                this.readingNoteService.RemoveReadingNoteByIdAsync(inputReadingNoteId);

            // then
            await Assert.ThrowsAsync<ReadingNoteServiceException>(() =>
                deleteReadingNoteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedReadingNoteException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReadingNoteByIdAsync(inputReadingNoteId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
