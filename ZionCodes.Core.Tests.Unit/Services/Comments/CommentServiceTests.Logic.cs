using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Comments;

namespace ZionCodes.Core.Tests.Unit.Services.Comments
{
    public partial class CommentServiceTests
    {
        [Fact]
        public async Task ShouldAddCommentAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            Comment randomComment = CreateRandomComment(dateTime);
            Comment inputComment = randomComment;
            inputComment.UpdatedDate = inputComment.CreatedDate;
            Comment expectedComment = inputComment;

            //this.dateTimeBrokerMock.Setup(broker =>
            //    broker.GetCurrentDateTime())
            //        .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCommentAsync(inputComment))
                    .ReturnsAsync(expectedComment);

            //when
            Comment actualComment =
                await this.commentService.AddCommentAsync(inputComment);

            //then
            actualComment.Should().BeEquivalentTo(expectedComment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCommentAsync(inputComment),
                    Times.Once);

            //this.dateTimeBrokerMock.Verify(broker =>
            //    broker.GetCurrentDateTime(),
            //        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
