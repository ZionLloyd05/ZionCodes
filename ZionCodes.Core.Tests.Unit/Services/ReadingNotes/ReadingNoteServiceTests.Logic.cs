using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using ZionCodes.Core.Models.ReadingNotes;

namespace ZionCodes.Core.Tests.Unit.Services.ReadingNotes
{
    public partial class ReadingNoteServiceTests
    {
        [Fact]
        public async Task ShouldAddReadingNoteAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            ReadingNote randomReadingNote = CreateRandomReadingNote(dateTime);
            ReadingNote inputReadingNote = randomReadingNote;
            inputReadingNote.UpdatedBy = inputReadingNote.CreatedBy;
            inputReadingNote.UpdatedDate = inputReadingNote.CreatedDate;
            ReadingNote expectedReadingNote = inputReadingNote;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertReadingNoteAsync(inputReadingNote))
                    .ReturnsAsync(expectedReadingNote);

            //when
            ReadingNote actualReadingNote =
                await this.readingNoteService.AddReadingNoteAsync(inputReadingNote);

            //then
            actualReadingNote.Should().BeEquivalentTo(expectedReadingNote);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertReadingNoteAsync(inputReadingNote),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
