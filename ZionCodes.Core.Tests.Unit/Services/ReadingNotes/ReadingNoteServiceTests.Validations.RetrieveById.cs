using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ZionCodes.Core.Models.ReadingNotes;
using ZionCodes.Core.Models.ReadingNotes.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.ReadingNotes
{
    public partial class ReadingNoteServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            // given
            int randomReadingNoteId = default;
            int inputReadingNoteId = randomReadingNoteId;

            var invalidReadingNoteInputException = new InvalidReadingNoteInputException(
                    parameterName: nameof(ReadingNote.Id),
                    parameterValue: inputReadingNoteId);

            var expectedReadingNoteValidationException =
                new ReadingNoteValidationException(invalidReadingNoteInputException);

            // when
            ValueTask<ReadingNote> retrieveReadingNoteByIdTask =
                this.readingNoteService.RetrieveReadingNoteByIdAsync(inputReadingNoteId);

            // then
            await Assert.ThrowsAsync<ReadingNoteValidationException>(() =>
                retrieveReadingNoteByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedReadingNoteValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReadingNoteByIdAsync(It.IsAny<int>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageReadingNoteIsNullAndLogItAsync()
        {
            // given
            int randomReadingNoteId = 1;
            int someReadingNoteId = randomReadingNoteId;
            ReadingNote invalidStorageReadingNote = null;
            var notFoundReadingNoteException = new NotFoundReadingNoteException(someReadingNoteId);

            var exceptionReadingNoteValidationException =
                new ReadingNoteValidationException(notFoundReadingNoteException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReadingNoteByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(invalidStorageReadingNote);

            // when
            ValueTask<ReadingNote> retrieveReadingNoteByIdTask =
                this.readingNoteService.RetrieveReadingNoteByIdAsync(someReadingNoteId);

            // then
            await Assert.ThrowsAsync<ReadingNoteValidationException>(() =>
                retrieveReadingNoteByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(exceptionReadingNoteValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReadingNoteByIdAsync(It.IsAny<int>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
