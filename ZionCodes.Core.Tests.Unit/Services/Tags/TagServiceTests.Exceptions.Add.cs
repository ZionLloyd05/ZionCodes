using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Tag randomTag = CreateRandomTag(dateTime);
            Tag inputTag = randomTag;
            inputTag.UpdatedBy = inputTag.CreatedBy;
            var sqlException = GetSqlException();

            var expectedTagDependencyException =
                new TagDependencyException(sqlException);

            //this.dateTimeBrokerMock.Setup(broker =>
            //    broker.GetCurrentDateTime())
            //        .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTagAsync(inputTag))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Tag> createTagTask =
                this.tagService.AddTagAsync(inputTag);

            // then
            await Assert.ThrowsAsync<TagDependencyException>(() =>
                createTagTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedTagDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTagAsync(inputTag),
                    Times.Once);

            //this.dateTimeBrokerMock.Verify(broker =>
            //    broker.GetCurrentDateTime(),
            //        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Tag randomTag = CreateRandomTag(dateTime);
            Tag inputTag = randomTag;
            inputTag.UpdatedBy = inputTag.CreatedBy;
            var databaseUpdateException = new DbUpdateException();

            var expectedTagDependencyException =
                new TagDependencyException(databaseUpdateException);

            //this.dateTimeBrokerMock.Setup(broker =>
            //    broker.GetCurrentDateTime())
            //        .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTagAsync(inputTag))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Tag> createTagTask =
                this.tagService.AddTagAsync(inputTag);

            // then
            await Assert.ThrowsAsync<TagDependencyException>(() =>
                createTagTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTagDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTagAsync(inputTag),
                    Times.Once);

            //this.dateTimeBrokerMock.Verify(broker =>
            //    broker.GetCurrentDateTime(),
            //        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
