using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;
using ZionCodes.Core.Models.ReadingNotes;
using ZionCodes.Core.Models.ReadingNotes.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.ReadingNotes
{
    public partial class ReadingNoteServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomReadingNoteId = Guid.NewGuid();
            Guid inputReadingNoteId = randomReadingNoteId;
            SqlException sqlException = GetSqlException();

            var expectedReadingNoteDependencyException =
                new ReadingNoteDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReadingNoteByIdAsync(inputReadingNoteId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ReadingNote> deleteReadingNoteTask =
                this.readingNoteService.RemoveReadingNoteByIdAsync(inputReadingNoteId);

            // then
            await Assert.ThrowsAsync<ReadingNoteDependencyException>(() =>
                deleteReadingNoteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedReadingNoteDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReadingNoteByIdAsync(inputReadingNoteId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteReadingNoteAsync(It.IsAny<ReadingNote>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
