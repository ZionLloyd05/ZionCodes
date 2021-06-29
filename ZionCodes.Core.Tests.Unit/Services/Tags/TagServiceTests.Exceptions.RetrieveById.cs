using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Guid randomTagId = Guid.NewGuid();
            Guid inputTagId = randomTagId;
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
            Guid someTagId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedTagDependencyException =
                new TagDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(It.IsAny<Guid>()))
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
                broker.SelectTagByIdAsync(It.IsAny<Guid>()),
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
            Guid someTagId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedTagException =
                new LockedTagException(databaseUpdateConcurrencyException);

            var expectedTagDependencyException =
                new TagDependencyException(lockedTagException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(It.IsAny<Guid>()))
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
                broker.SelectTagByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someTagId = Guid.NewGuid();
            var exception = new Exception();

            var expectedTagServiceException =
                new TagServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(It.IsAny<Guid>()))
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
                broker.SelectTagByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
