using System;
using System.Linq;
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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

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

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetriveAllComments()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<Comment> randomComments =
                CreateRandomComments(randomDateTime);

            IQueryable<Comment> storageComments =
                randomComments;

            IQueryable<Comment> expectedComments =
                storageComments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllComments())
                    .Returns(storageComments);

            // when
            IQueryable<Comment> actualComments =
                this.commentService.RetrieveAllComments();

            // then
            actualComments.Should().BeEquivalentTo(expectedComments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllComments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveCommentByIdAsync()
        {
            // given
            Guid randomCommentId = Guid.NewGuid();
            Guid inputCommentId = randomCommentId;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Comment randomComment = CreateRandomComment(randomDateTime);
            Comment storageComment = randomComment;
            Comment expectedComment = storageComment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(inputCommentId))
                    .ReturnsAsync(storageComment);

            // when
            Comment actualComment =
                await this.commentService.RetrieveCommentByIdAsync(inputCommentId);

            // then
            actualComment.Should().BeEquivalentTo(expectedComment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(inputCommentId),
                    Times.Once);


            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
