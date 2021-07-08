using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Comment randomComment = CreateRandomComment(dateTime);
            Comment inputComment = randomComment;
            var sqlException = GetSqlException();

            var expectedCommentDependencyException =
                new CommentDependencyException(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCommentAsync(inputComment))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Comment> createCommentTask =
                this.commentService.AddCommentAsync(inputComment);

            // then
            await Assert.ThrowsAsync<CommentDependencyException>(() =>
                createCommentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCommentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCommentAsync(inputComment),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Comment randomComment = CreateRandomComment(dateTime);
            Comment inputComment = randomComment;
            var databaseUpdateException = new DbUpdateException();

            var expectedCommentDependencyException =
                new CommentDependencyException(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCommentAsync(inputComment))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Comment> createCommentTask =
                this.commentService.AddCommentAsync(inputComment);

            // then
            await Assert.ThrowsAsync<CommentDependencyException>(() =>
                createCommentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCommentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCommentAsync(inputComment),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Comment randomComment = CreateRandomComment(dateTime);
            Comment inputComment = randomComment;
            var exception = new Exception();

            var expectedCommentServiceException =
                new CommentServiceException(exception);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCommentAsync(inputComment))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Comment> createCommentTask =
                 this.commentService.AddCommentAsync(inputComment);

            // then
            await Assert.ThrowsAsync<CommentServiceException>(() =>
                createCommentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCommentServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCommentAsync(inputComment),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
