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

    }
}
