using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using Xunit;
using ZionCodes.Core.Models.ReadingNotes;
using ZionCodes.Core.Models.ReadingNotes.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.ReadingNotes
{
    public partial class ReadingNoteServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenReadingNoteIsNullAndLogItAsync()
        {
            // given
            ReadingNote randomReadingNote = null;
            ReadingNote nullReadingNote = randomReadingNote;
            var nullReadingNoteException = new NullReadingNoteException();

            var expectedReadingNoteValidationException =
                new ReadingNoteValidationException(nullReadingNoteException);

            // when
            ValueTask<ReadingNote> createReadingNoteTask =
                this.readingNoteService.AddReadingNoteAsync(nullReadingNote);

            // then
            await Assert.ThrowsAsync<ReadingNoteValidationException>(() =>
                createReadingNoteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedReadingNoteValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertReadingNoteAsync(It.IsAny<ReadingNote>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenReadingNoteIdIsInvalidAndLogItAsync()
        {
            //given
            DateTimeOffset dateTime = GetRandomDateTime();
            ReadingNote randomReadingNote = CreateRandomReadingNote(dateTime);
            ReadingNote inputReadingNote = randomReadingNote;
            inputReadingNote.Id = default;

            var invalidReadingNoteInputException = new InvalidReadingNoteException(
                parameterName: nameof(ReadingNote.Id),
                parameterValue: inputReadingNote.Id);

            var expectedReadingNoteValidationException =
                new ReadingNoteValidationException(invalidReadingNoteInputException);

            // when
            ValueTask<ReadingNote> registerReadingNoteTask =
                this.readingNoteService.AddReadingNoteAsync(inputReadingNote);

            // then
            await Assert.ThrowsAsync<ReadingNoteValidationException>(() =>
                registerReadingNoteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedReadingNoteValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertReadingNoteAsync(It.IsAny<ReadingNote>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReadingNoteAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ReadingNote randomReadingNote = CreateRandomReadingNote(dateTime);
            ReadingNote alreadyExistsReadingNote = randomReadingNote;
            alreadyExistsReadingNote.UpdatedBy = alreadyExistsReadingNote.CreatedBy;

            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsReadingNoteException =
                new AlreadyExistsReadingNoteException(duplicateKeyException);

            var expectedReadingNoteValidationException =
                new ReadingNoteValidationException(alreadyExistsReadingNoteException);

            //this.dateTimeBrokerMock.Setup(broker =>
            //    broker.GetCurrentDateTime())
            //        .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertReadingNoteAsync(alreadyExistsReadingNote))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<ReadingNote> createReadingNoteTask =
                this.readingNoteService.AddReadingNoteAsync(alreadyExistsReadingNote);

            // then
            await Assert.ThrowsAsync<ReadingNoteValidationException>(() =>
                createReadingNoteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedReadingNoteValidationException))),
                    Times.Once);

            //this.dateTimeBrokerMock.Verify(broker =>
            //    broker.GetCurrentDateTime(),
            //        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertReadingNoteAsync(alreadyExistsReadingNote),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
