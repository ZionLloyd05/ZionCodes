using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Comments;
using ZionCodes.Core.Models.Comments.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.Comments
{
    public partial class CommentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Comment randomComment = CreateRandomComment(randomDateTime);
            Comment someComment = randomComment;
            someComment.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

            var expectedCommentDependencyException =
                new CommentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(someComment.Id))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(someComment);

            // then
            await Assert.ThrowsAsync<CommentDependencyException>(() =>
                modifyCommentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(someComment.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCommentDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Comment someComment = CreateRandomComment(randomDateTime);
            someComment.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var expectedCommentDependencyException =
                new CommentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(someComment.Id))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(someComment);

            // then
            await Assert.ThrowsAsync<CommentDependencyException>(() =>
                modifyCommentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(someComment.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCommentDependencyException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Comment randomComment = CreateRandomComment(randomDateTime);
            Comment someComment = randomComment;
            someComment.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedCommentException = new LockedCommentException(databaseUpdateConcurrencyException);

            var expectedCommentDependencyException =
                new CommentDependencyException(lockedCommentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(someComment.Id))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(someComment);

            // then
            await Assert.ThrowsAsync<CommentDependencyException>(() =>
                modifyCommentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(someComment.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCommentDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Comment randomComment = CreateRandomComment(randomDateTime);
            Comment someComment = randomComment;
            someComment.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var serviceException = new Exception();

            var expectedCommentServiceException =
                new CommentServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(someComment.Id))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(someComment);

            // then
            await Assert.ThrowsAsync<CommentServiceException>(() =>
                modifyCommentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(someComment.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCommentServiceException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
