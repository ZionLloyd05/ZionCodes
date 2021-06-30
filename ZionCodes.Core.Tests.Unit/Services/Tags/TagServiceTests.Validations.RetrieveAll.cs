using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            IQueryable<Tag> emptyStorageTags = new List<Tag>().AsQueryable();
            IQueryable<Tag> expectedTags = emptyStorageTags;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTags())
                    .Returns(expectedTags);

            // when
            IQueryable<Tag> actualTag =
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
