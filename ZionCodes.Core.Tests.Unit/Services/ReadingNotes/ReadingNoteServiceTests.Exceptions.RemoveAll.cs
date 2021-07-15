using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ZionCodes.Core.Models.ReadingNotes.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.ReadingNotes
{
    public partial class ReadingNoteServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllReadingNotesWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedReadingNoteDependencyException =
                new ReadingNoteDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllReadingNotes())
                    .Throws(sqlException);

            // when . then
            Assert.Throws<ReadingNoteDependencyException>(() =>
                this.readingNoteService.RetrieveAllReadingNotes());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllReadingNotes(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedReadingNoteDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
