using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Comments;

namespace ZionCodes.Core.Tests.Unit.Services.Comments
{
    public partial class CommentServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenCommentsWasEmptyAndLogIt()
        {
            // given
            ICollection<Comment> emptyStorageComments = new List<Comment>().ToList();
            ICollection<Comment> expectedComments = emptyStorageComments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllComments())
                    .Returns(expectedComments);

            // when
            ICollection<Comment> actualComment =
                this.commentService.RetrieveAllComments();

            // then
            actualComment.Should().BeEquivalentTo(expectedComments);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No comments found in storage."));

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllComments(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
