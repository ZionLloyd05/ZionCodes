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
        public async void ShouldThrowValidationExceptionOnModifyWhenReadingNoteIsNullAndLogItAsync()
        {
            // given
            ReadingNote randomReadingNote = null;
            ReadingNote nullReadingNote = randomReadingNote;
            var nullReadingNoteException = new NullReadingNoteException();

            var expectedReadingNoteValidationException =
                new ReadingNoteValidationException(nullReadingNoteException);

            // when
            ValueTask<ReadingNote> modifyReadingNoteTask =
                this.readingNoteService.ModifyReadingNoteAsync(nullReadingNote);

            // then
            await Assert.ThrowsAsync<ReadingNoteValidationException>(() =>
                modifyReadingNoteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedReadingNoteValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReadingNoteByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ReadingNote randomReadingNote = CreateRandomReadingNote(dateTime);
            ReadingNote inputReadingNote = randomReadingNote;
            inputReadingNote.UpdatedDate = default;

            var invalidReadingNoteException = new InvalidReadingNoteException(
                parameterName: nameof(ReadingNote.UpdatedDate),
                parameterValue: inputReadingNote.UpdatedDate);

            var expectedReadingNoteValidationException =
                new ReadingNoteValidationException(invalidReadingNoteException);

            // when
            ValueTask<ReadingNote> modifyReadingNoteTask =
                this.readingNoteService.ModifyReadingNoteAsync(inputReadingNote);

            // then
            await Assert.ThrowsAsync<ReadingNoteValidationException>(() =>
                modifyReadingNoteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedReadingNoteValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReadingNoteByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
