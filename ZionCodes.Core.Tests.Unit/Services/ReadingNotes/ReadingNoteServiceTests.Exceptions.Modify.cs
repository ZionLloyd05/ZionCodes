using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using ZionCodes.Core.Models.ReadingNotes;
using ZionCodes.Core.Models.ReadingNotes.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.ReadingNotes
{
    public partial class ReadingNoteServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ReadingNote randomReadingNote = CreateRandomReadingNote(randomDateTime);
            ReadingNote someReadingNote = randomReadingNote;
            someReadingNote.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

            var expectedReadingNoteDependencyException =
                new ReadingNoteDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReadingNoteByIdAsync(someReadingNote.Id))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ReadingNote> modifyReadingNoteTask =
                this.readingNoteService.ModifyReadingNoteAsync(someReadingNote);

            // then
            await Assert.ThrowsAsync<ReadingNoteDependencyException>(() =>
                modifyReadingNoteTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReadingNoteByIdAsync(someReadingNote.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedReadingNoteDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ReadingNote someReadingNote = CreateRandomReadingNote(randomDateTime);
            someReadingNote.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var expectedReadingNoteDependencyException =
                new ReadingNoteDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectReadingNoteByIdAsync(someReadingNote.Id))
                    .ThrowsAsync(databaseUpdateException);

            //this.dateTimeBrokerMock.Setup(broker =>
            //    broker.GetCurrentDateTime())
            //        .Returns(randomDateTime);

            // when
            ValueTask<ReadingNote> modifyReadingNoteTask =
                this.readingNoteService.ModifyReadingNoteAsync(someReadingNote);

            // then
            await Assert.ThrowsAsync<ReadingNoteDependencyException>(() =>
                modifyReadingNoteTask.AsTask());

            //this.dateTimeBrokerMock.Verify(broker =>
            //    broker.GetCurrentDateTime(),
            //        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectReadingNoteByIdAsync(someReadingNote.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedReadingNoteDependencyException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
