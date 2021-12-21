using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Comments;
using ZionCodes.Core.Models.Comments.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.Comments
{
    public partial class CommentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            int randomCommentId = default;
            int inputCommentId = randomCommentId;

            var invalidCommentInputException = new InvalidCommentException(
                parameterName: nameof(Comment.Id),
                parameterValue: inputCommentId);

            var expectedCommentValidationException =
                new CommentValidationException(invalidCommentInputException);

            // when
            ValueTask<Comment> deleteCommentTask =
               this.commentService.RemoveCommentByIdAsync(inputCommentId);

            // then
            await Assert.ThrowsAsync<CommentValidationException>(() => deleteCommentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCommentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<int>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCommentAsync(It.IsAny<Comment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenStorageCommentIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Comment randomComment = CreateRandomComment(dateTime);
            int inputCommentId = randomComment.Id;
            Comment inputComment = randomComment;
            Comment nullStorageComment = null;

            var notFoundCommentException = new NotFoundCommentException(inputCommentId);

            var expectedCommentValidationException =
                new CommentValidationException(notFoundCommentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(inputCommentId))
                    .ReturnsAsync(nullStorageComment);

            // when
            ValueTask<Comment> actualCommentTask =
                this.commentService.RemoveCommentByIdAsync(inputCommentId);

            // then
            await Assert.ThrowsAsync<CommentValidationException>(() => actualCommentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCommentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(inputCommentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCommentAsync(It.IsAny<Comment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
