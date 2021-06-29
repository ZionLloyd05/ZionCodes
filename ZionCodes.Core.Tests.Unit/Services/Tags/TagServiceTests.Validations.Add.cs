using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Categories.Exceptions;
using ZionCodes.Core.Models.Tags;
using ZionCodes.Core.Models.Tags.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.Tags
{
    public partial class TagServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenTagIsNullAndLogItAsync()
        {
            // given
            Tag randomTag = null;
            Tag nullTag = randomTag;
            var nullTagException = new NullTagException();

            var expectedTagValidationException =
                new TagValidationException(nullTagException);

            // when
            ValueTask<Tag> createTagTask =
                this.tagService.AddTagAsync(nullTag);

            // then
            await Assert.ThrowsAsync<TagValidationException>(() =>
                createTagTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTagValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTagAsync(It.IsAny<Tag>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenTagIdIsInvalidAndLogItAsync()
        {
            //given
            DateTimeOffset dateTime = GetRandomDateTime();
            Tag randomTag = CreateRandomTag(dateTime);
            Tag inputTag = randomTag;
            inputTag.Id = default;

            var invalidTagInputException = new InvalidTagException(
                parameterName: nameof(Tag.Id),
                parameterValue: inputTag.Id);

            var expectedTagValidationException =
                new TagValidationException(invalidTagInputException);

            // when
            ValueTask<Tag> registerTagTask =
                this.tagService.AddTagAsync(inputTag);

            // then
            await Assert.ThrowsAsync<TagValidationException>(() =>
                registerTagTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTagValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTagAsync(It.IsAny<Tag>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
