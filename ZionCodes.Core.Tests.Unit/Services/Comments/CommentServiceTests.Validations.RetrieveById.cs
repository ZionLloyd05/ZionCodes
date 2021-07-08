using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomCommentId = default;
            Guid inputCommentId = randomCommentId;

            var invalidCommentInputException = new InvalidCommentInputException(
                    parameterName: nameof(Comment.Id),
                    parameterValue: inputCommentId);

            var expectedCommentValidationException =
                new CommentValidationException(invalidCommentInputException);

            // when
            ValueTask<Comment> retrieveCommentByIdTask =
                this.commentService.RetrieveCommentByIdAsync(inputCommentId);

            // then
            await Assert.ThrowsAsync<CommentValidationException>(() =>
                retrieveCommentByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCommentValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageCommentIsNullAndLogItAsync()
        {
            // given
            Guid randomCommentId = Guid.NewGuid();
            Guid someCommentId = randomCommentId;
            Comment invalidStorageComment = null;
            var notFoundCommentException = new NotFoundCommentException(someCommentId);

            var exceptionCommentValidationException =
                new CommentValidationException(notFoundCommentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageComment);

            // when
            ValueTask<Comment> retrieveCommentByIdTask =
                this.commentService.RetrieveCommentByIdAsync(someCommentId);

            // then
            await Assert.ThrowsAsync<CommentValidationException>(() =>
                retrieveCommentByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(exceptionCommentValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
