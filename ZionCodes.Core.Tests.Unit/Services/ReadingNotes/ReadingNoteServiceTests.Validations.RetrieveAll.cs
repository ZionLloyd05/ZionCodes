using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Xunit;
using ZionCodes.Core.Models.ReadingNotes;

namespace ZionCodes.Core.Tests.Unit.Services.ReadingNotes
{
    public partial class ReadingNoteServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenReadingNotesWasEmptyAndLogIt()
        {
            // given
            ICollection<ReadingNote> emptyStorageReadingNotes = new List<ReadingNote>().ToList();
            ICollection<ReadingNote> expectedReadingNotes = emptyStorageReadingNotes;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllReadingNotes())
                    .Returns(expectedReadingNotes);

            // when
            ICollection<ReadingNote> actualReadingNote =
                this.readingNoteService.RetrieveAllReadingNotes();

            // then
            actualReadingNote.Should().BeEquivalentTo(expectedReadingNotes);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No reading notes found in storage."));

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllReadingNotes(),
                    Times.Once);


            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
