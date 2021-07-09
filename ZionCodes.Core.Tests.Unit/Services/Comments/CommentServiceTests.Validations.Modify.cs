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
        public async void ShouldThrowValidationExceptionOnModifyWhenCommentIsNullAndLogItAsync()
        {
            // given
            Comment randomComment = null;
            Comment nullComment = randomComment;
            var nullCommentException = new NullCommentException();

            var expectedCommentValidationException =
                new CommentValidationException(nullCommentException);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(nullComment);

            // then
            await Assert.ThrowsAsync<CommentValidationException>(() =>
                modifyCommentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCommentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Comment randomComment = CreateRandomComment(dateTime);
            Comment inputComment = randomComment;
            inputComment.Id = default;

            var invalidCommentInputException = new InvalidCommentException(
                parameterName: nameof(Comment.Id),
                parameterValue: inputComment.Id);

            var expectedCommentValidationException =
                new CommentValidationException(invalidCommentInputException);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(inputComment);

            // then
            await Assert.ThrowsAsync<CommentValidationException>(() =>
                modifyCommentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCommentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenCommentBodyIsInvalidAndLogItAsync(
           string invalidCommentBody)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Comment randomComment = CreateRandomComment(dateTime);
            Comment invalidComment = randomComment;
            invalidComment.Body = invalidCommentBody;

            var invalidCommentException = new InvalidCommentException(
               parameterName: nameof(Comment.Body),
               parameterValue: invalidComment.Body);

            var expectedCommentValidationException =
                new CommentValidationException(invalidCommentException);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(invalidComment);

            // then
            await Assert.ThrowsAsync<CommentValidationException>(() =>
                modifyCommentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCommentValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Comment randomComment = CreateRandomComment(dateTime);
            Comment inputComment = randomComment;
            inputComment.UpdatedDate = default;

            var invalidCommentException = new InvalidCommentException(
                parameterName: nameof(Comment.UpdatedDate),
                parameterValue: inputComment.UpdatedDate);

            var expectedCommentValidationException =
                new CommentValidationException(invalidCommentException);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(inputComment);

            // then
            await Assert.ThrowsAsync<CommentValidationException>(() =>
                modifyCommentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCommentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
