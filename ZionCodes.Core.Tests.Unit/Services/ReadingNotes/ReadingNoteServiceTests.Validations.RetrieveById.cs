using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Guid randomReadingNoteId = default;
            Guid inputReadingNoteId = randomReadingNoteId;

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
                broker.SelectReadingNoteByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
