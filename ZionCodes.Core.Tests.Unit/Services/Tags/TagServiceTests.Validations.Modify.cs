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
        public async void ShouldThrowValidationExceptionOnModifyWhenTagIsNullAndLogItAsync()
        {
            // given
            Tag randomTag = null;
            Tag nullTag = randomTag;
            var nullTagException = new NullTagException();

            var expectedTagValidationException =
                new TagValidationException(nullTagException);

            // when
            ValueTask<Tag> modifyTagTask =
                this.tagService.ModifyTagAsync(nullTag);

            // then
            await Assert.ThrowsAsync<TagValidationException>(() =>
                modifyTagTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTagValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
