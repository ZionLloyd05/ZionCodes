using System;
using System.Linq;
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

        [Fact]
        public void ShouldRetriveAllReadingNotes()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<ReadingNote> randomReadingNotes =
                CreateRandomReadingNotes(randomDateTime);

            IQueryable<ReadingNote> storageReadingNotes =
                randomReadingNotes;

            IQueryable<ReadingNote> expectedReadingNotes =
                storageReadingNotes;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllReadingNotes())
                    .Returns(storageReadingNotes);

            // when
            IQueryable<ReadingNote> actualReadingNotes =
                this.readingNoteService.RetrieveAllReadingNotes();

            // then
            actualReadingNotes.Should().BeEquivalentTo(expectedReadingNotes);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllReadingNotes(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveReadingNoteByIdAsync()
        {
            // given
            Guid randomReadingNoteId = Guid.NewGuid();
            Guid inputReadingNoteId = randomReadingNoteId;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ReadingNote randomReadingNote = CreateRandomReadingNote(randomDateTime);
            ReadingNote storageReadingNote = randomReadingNote;
            ReadingNote expectedReadingNote = storageReadingNote;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReadingNoteByIdAsync(inputReadingNoteId))
                    .ReturnsAsync(storageReadingNote);

            // when
            ReadingNote actualReadingNote =
                await this.readingNoteService.RetrieveReadingNoteByIdAsync(inputReadingNoteId);

            // then
            actualReadingNote.Should().BeEquivalentTo(expectedReadingNote);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReadingNoteByIdAsync(inputReadingNoteId),
                    Times.Once);


            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }


    }
}
