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
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomTagId = default;
            Guid inputTagId = randomTagId;

            var invalidTagInputException = new InvalidTagInputException(
                    parameterName: nameof(Tag.Id),
                    parameterValue: inputTagId);

            var expectedTagValidationException =
                new TagValidationException(invalidTagInputException);

            // when
            ValueTask<Tag> retrieveTagByIdTask =
                this.tagService.RetrieveTagByIdAsync(inputTagId);

            // then
            await Assert.ThrowsAsync<TagValidationException>(() =>
                retrieveTagByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTagValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageTagIsNullAndLogItAsync()
        {
            // given
            Guid randomTagId = Guid.NewGuid();
            Guid someTagId = randomTagId;
            Tag invalidStorageTag = null;
            var notFoundTagException = new NotFoundTagException(someTagId);

            var exceptionTagValidationException =
                new TagValidationException(notFoundTagException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageTag);

            // when
            ValueTask<Tag> retrieveTagByIdTask =
                this.tagService.RetrieveTagByIdAsync(someTagId);

            // then
            await Assert.ThrowsAsync<TagValidationException>(() =>
                retrieveTagByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(exceptionTagValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
