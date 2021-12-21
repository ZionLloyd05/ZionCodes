using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Tags;

namespace ZionCodes.Core.Tests.Unit.Services.Tags
{
    public partial class TagServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenTagsWasEmptyAndLogIt()
        {
            // given
            ICollection<Tag> emptyStorageTags = new List<Tag>().ToList();
            ICollection<Tag> expectedTags = emptyStorageTags;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTags())
                    .Returns(expectedTags);

            // when
            ICollection<Tag> actualTag =
                this.tagService.RetrieveAllTags();

            // then
            actualTag.Should().BeEquivalentTo(expectedTags);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No tags found in storage."));

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTags(),
                    Times.Once);


            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
