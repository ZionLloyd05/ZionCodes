using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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
            ICollection<ReadingNote> randomReadingNotes =
                CreateRandomReadingNotes(randomDateTime);

            ICollection<ReadingNote> storageReadingNotes =
                randomReadingNotes;

            ICollection<ReadingNote> expectedReadingNotes =
                storageReadingNotes;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllReadingNotes())
                    .Returns(storageReadingNotes);

            // when
            ICollection<ReadingNote> actualReadingNotes =
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
            int randomReadingNoteId = 1;
            int inputReadingNoteId = randomReadingNoteId;
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


        [Fact]
        public async Task ShouldModifyReadingNoteAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            ReadingNote randomReadingNote = CreateRandomReadingNote(randomInputDate);
            ReadingNote inputReadingNote = randomReadingNote;
            ReadingNote afterUpdateStorageReadingNote = inputReadingNote;
            ReadingNote expectedReadingNote = afterUpdateStorageReadingNote;
            ReadingNote beforeUpdateStorageReadingNote = randomReadingNote.DeepClone();
            inputReadingNote.UpdatedDate = randomDate;
            int tagId = inputReadingNote.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReadingNoteByIdAsync(tagId))
                    .ReturnsAsync(beforeUpdateStorageReadingNote);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateReadingNoteAsync(inputReadingNote))
                    .ReturnsAsync(afterUpdateStorageReadingNote);

            // when
            ReadingNote actualReadingNote =
                await this.readingNoteService.ModifyReadingNoteAsync(inputReadingNote);

            // then
            actualReadingNote.Should().BeEquivalentTo(expectedReadingNote);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReadingNoteByIdAsync(tagId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateReadingNoteAsync(inputReadingNote),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldDeleteReadingNoteByIdAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            ReadingNote randomReadingNote = CreateRandomReadingNote(dateTime);
            ReadingNote inputReadingNote = randomReadingNote;
            int inputReadingNoteId = inputReadingNote.Id;
            inputReadingNote.UpdatedBy = inputReadingNote.CreatedBy;
            inputReadingNote.UpdatedDate = inputReadingNote.CreatedDate;
            ReadingNote storageReadingNote = inputReadingNote;
            ReadingNote expectedReadingNote = inputReadingNote;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReadingNoteByIdAsync(inputReadingNoteId))
                    .ReturnsAsync(inputReadingNote);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteReadingNoteAsync(inputReadingNote))
                    .ReturnsAsync(storageReadingNote);

            // when
            ReadingNote actualReadingNote =
                await this.readingNoteService.RemoveReadingNoteByIdAsync(inputReadingNoteId);

            // then
            actualReadingNote.Should().BeEquivalentTo(expectedReadingNote);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReadingNoteByIdAsync(inputReadingNoteId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteReadingNoteAsync(inputReadingNote),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
