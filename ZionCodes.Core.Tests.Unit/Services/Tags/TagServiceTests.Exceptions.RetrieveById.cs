using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Tags;
using ZionCodes.Core.Models.Tags.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.Tags
{
    public partial class TagServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomTagId = 1;
            int inputTagId = randomTagId;
            SqlException sqlException = GetSqlException();

            var exceptionTagDependencyException =
                new TagDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(inputTagId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Tag> retrieveTagByIdTask =
                this.tagService.RetrieveTagByIdAsync(inputTagId);

            // then
            await Assert.ThrowsAsync<TagDependencyException>(() =>
                retrieveTagByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(exceptionTagDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(inputTagId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            int someTagId = 1;
            var databaseUpdateException = new DbUpdateException();

            var expectedTagDependencyException =
                new TagDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(It.IsAny<int>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Tag> retrieveByIdTagTask =
                this.tagService.RetrieveTagByIdAsync(someTagId);

            // then
            await Assert.ThrowsAsync<TagDependencyException>(() =>
                retrieveByIdTagTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTagDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(It.IsAny<int>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
         ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            int someTagId = 1;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedTagException =
                new LockedTagException(databaseUpdateConcurrencyException);

            var expectedTagDependencyException =
                new TagDependencyException(lockedTagException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(It.IsAny<int>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Tag> retrieveByIdTagTask =
                this.tagService.RetrieveTagByIdAsync(someTagId);

            // then
            await Assert.ThrowsAsync<TagDependencyException>(() =>
                retrieveByIdTagTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTagDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(It.IsAny<int>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            int someTagId = 1;
            var exception = new Exception();

            var expectedTagServiceException =
                new TagServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(It.IsAny<int>()))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Tag> retrieveByIdTagTask =
                this.tagService.RetrieveTagByIdAsync(someTagId);

            // then
            await Assert.ThrowsAsync<TagServiceException>(() =>
                retrieveByIdTagTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTagServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(It.IsAny<int>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
