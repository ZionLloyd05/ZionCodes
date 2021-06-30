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
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomTagId = Guid.NewGuid();
            Guid inputTagId = randomTagId;
            SqlException sqlException = GetSqlException();

            var expectedTagDependencyException =
                new TagDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(inputTagId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Tag> deleteTagTask =
                this.tagService.RemoveTagByIdAsync(inputTagId);

            // then
            await Assert.ThrowsAsync<TagDependencyException>(() =>
                deleteTagTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedTagDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(inputTagId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTagAsync(It.IsAny<Tag>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomTagId = Guid.NewGuid();
            Guid inputTagId = randomTagId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedTagException =
                new LockedTagException(databaseUpdateConcurrencyException);

            var expectedTagException =
                new TagDependencyException(lockedTagException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(inputTagId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Tag> deleteTagTask =
                this.tagService.RemoveTagByIdAsync(inputTagId);

            // then
            await Assert.ThrowsAsync<TagDependencyException>(() => deleteTagTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTagException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(inputTagId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomTagId = Guid.NewGuid();
            Guid inputTagId = randomTagId;
            var exception = new Exception();

            var expectedTagException =
                new TagServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(inputTagId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Tag> deleteTagTask =
                this.tagService.RemoveTagByIdAsync(inputTagId);

            // then
            await Assert.ThrowsAsync<TagServiceException>(() =>
                deleteTagTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTagException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(inputTagId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
