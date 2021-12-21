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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomCommentId = 1;
            int inputCommentId = randomCommentId;
            SqlException sqlException = GetSqlException();

            var exceptionCommentDependencyException =
                new CommentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(inputCommentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Comment> retrieveCommentByIdTask =
                this.commentService.RetrieveCommentByIdAsync(inputCommentId);

            // then
            await Assert.ThrowsAsync<CommentDependencyException>(() =>
                retrieveCommentByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(exceptionCommentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(inputCommentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            int someCommentId = 1;
            var databaseUpdateException = new DbUpdateException();

            var expectedCommentDependencyException =
                new CommentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<int>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Comment> retrieveByIdCommentTask =
                this.commentService.RetrieveCommentByIdAsync(someCommentId);

            // then
            await Assert.ThrowsAsync<CommentDependencyException>(() =>
                retrieveByIdCommentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCommentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<int>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            int someCommentId = 1;
            var exception = new Exception();

            var expectedCommentServiceException =
                new CommentServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<int>()))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Comment> retrieveByIdCommentTask =
                this.commentService.RetrieveCommentByIdAsync(someCommentId);

            // then
            await Assert.ThrowsAsync<CommentServiceException>(() =>
                retrieveByIdCommentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCommentServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<int>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
          ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            int someCommentId = 1;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedCommentException =
                new LockedCommentException(databaseUpdateConcurrencyException);

            var expectedCommentDependencyException =
                new CommentDependencyException(lockedCommentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<int>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Comment> retrieveByIdCommentTask =
                this.commentService.RetrieveCommentByIdAsync(someCommentId);

            // then
            await Assert.ThrowsAsync<CommentDependencyException>(() =>
                retrieveByIdCommentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCommentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<int>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
