using System;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Tags.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.Tags
{
    public partial class TagServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllTagsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedTagDependencyException =
                new TagDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTags())
                    .Throws(sqlException);

            // when . then
            Assert.Throws<TagDependencyException>(() =>
                this.tagService.RetrieveAllTags());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTags(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedTagDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllTagsWhenExceptionOccursAndLogIt()
        {
            // given
            var exception = new Exception();

            var expectedTagServiceException =
                new TagServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTags())
                    .Throws(exception);

            // when . then
            Assert.Throws<TagServiceException>(() =>
                this.tagService.RetrieveAllTags());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTags(),
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
