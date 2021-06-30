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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Tag randomTag = CreateRandomTag(randomDateTime);
            Tag someTag = randomTag;
            someTag.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

            var expectedTagDependencyException =
                new TagDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(someTag.Id))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Tag> modifyTagTask =
                this.tagService.ModifyTagAsync(someTag);

            // then
            await Assert.ThrowsAsync<TagDependencyException>(() =>
                modifyTagTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(someTag.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedTagDependencyException))),
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
            Tag someTag = CreateRandomTag(randomDateTime);
            someTag.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var expectedTagDependencyException =
                new TagDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(someTag.Id))
                    .ThrowsAsync(databaseUpdateException);

            //this.dateTimeBrokerMock.Setup(broker =>
            //    broker.GetCurrentDateTime())
            //        .Returns(randomDateTime);

            // when
            ValueTask<Tag> modifyTagTask =
                this.tagService.ModifyTagAsync(someTag);

            // then
            await Assert.ThrowsAsync<TagDependencyException>(() =>
                modifyTagTask.AsTask());

            //this.dateTimeBrokerMock.Verify(broker =>
            //    broker.GetCurrentDateTime(),
            //        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(someTag.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTagDependencyException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Tag randomTag = CreateRandomTag(randomDateTime);
            Tag someTag = randomTag;
            someTag.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedTagException = new LockedTagException(databaseUpdateConcurrencyException);

            var expectedTagDependencyException =
                new TagDependencyException(lockedTagException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(someTag.Id))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Tag> modifyTagTask =
                this.tagService.ModifyTagAsync(someTag);

            // then
            await Assert.ThrowsAsync<TagDependencyException>(() =>
                modifyTagTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(someTag.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTagDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Tag randomTag = CreateRandomTag(randomDateTime);
            Tag someTag = randomTag;
            someTag.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var serviceException = new Exception();

            var expectedTagServiceException =
                new TagServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(someTag.Id))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Tag> modifyTagTask =
                this.tagService.ModifyTagAsync(someTag);

            // then
            await Assert.ThrowsAsync<TagServiceException>(() =>
                modifyTagTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(someTag.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTagServiceException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
