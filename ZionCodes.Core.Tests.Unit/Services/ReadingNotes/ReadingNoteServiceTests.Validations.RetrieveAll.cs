using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public void ShouldLogWarningOnRetrieveAllWhenReadingNotesWasEmptyAndLogIt()
        {
            // given
            IQueryable<ReadingNote> emptyStorageReadingNotes = new List<ReadingNote>().AsQueryable();
            IQueryable<ReadingNote> expectedReadingNotes = emptyStorageReadingNotes;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllReadingNotes())
                    .Returns(expectedReadingNotes);

            // when
            IQueryable<ReadingNote> actualReadingNote =
                this.readingNoteService.RetrieveAllReadingNotes();

            // then
            actualReadingNote.Should().BeEquivalentTo(expectedReadingNotes);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No tags found in storage."));

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
