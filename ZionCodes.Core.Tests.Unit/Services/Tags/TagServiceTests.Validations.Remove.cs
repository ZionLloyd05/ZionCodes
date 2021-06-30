using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Tags;
using ZionCodes.Core.Models.Tags.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.Tags
{
    public partial class TagServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomTagId = default;
            Guid inputTagId = randomTagId;

            var invalidTagInputException = new InvalidTagException(
                parameterName: nameof(Tag.Id),
                parameterValue: inputTagId);

            var expectedTagValidationException =
                new TagValidationException(invalidTagInputException);

            // when
            ValueTask<Tag> deleteTagTask =
               this.tagService.RemoveTagByIdAsync(inputTagId);

            // then
            await Assert.ThrowsAsync<TagValidationException>(() => deleteTagTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTagValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTagAsync(It.IsAny<Tag>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenStorageTagIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Tag randomTag = CreateRandomTag(dateTime);
            Guid inputTagId = randomTag.Id;
            Tag inputTag = randomTag;
            Tag nullStorageTag = null;

            var notFoundTagException = new NotFoundTagException(inputTagId);

            var expectedTagValidationException =
                new TagValidationException(notFoundTagException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(inputTagId))
                    .ReturnsAsync(nullStorageTag);

            // when
            ValueTask<Tag> actualTagTask =
                this.tagService.RemoveTagByIdAsync(inputTagId);

            // then
            await Assert.ThrowsAsync<TagValidationException>(() => actualTagTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTagValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(inputTagId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTagAsync(It.IsAny<Tag>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
