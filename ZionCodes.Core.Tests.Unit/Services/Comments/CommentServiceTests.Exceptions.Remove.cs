using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomCommentId = Guid.NewGuid();
            Guid inputCommentId = randomCommentId;
            SqlException sqlException = GetSqlException();

            var expectedCommentDependencyException =
                new CommentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(inputCommentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Comment> deleteCommentTask =
                this.commentService.RemoveCommentByIdAsync(inputCommentId);

            // then
            await Assert.ThrowsAsync<CommentDependencyException>(() =>
                deleteCommentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCommentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(inputCommentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCommentAsync(It.IsAny<Comment>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomCommentId = Guid.NewGuid();
            Guid inputCommentId = randomCommentId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedCommentException =
                new LockedCommentException(databaseUpdateConcurrencyException);

            var expectedStudentCommentException =
                new CommentDependencyException(lockedCommentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(inputCommentId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Comment> deleteStudentCommentTask =
                this.commentService.RemoveCommentByIdAsync(inputCommentId);

            // then
            await Assert.ThrowsAsync<CommentDependencyException>(() => deleteStudentCommentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentCommentException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(inputCommentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomCommentId = Guid.NewGuid();
            Guid inputCommentId = randomCommentId;
            var exception = new Exception();

            var expectedStudentCommentException =
                new CommentServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(inputCommentId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Comment> deleteStudentCommentTask =
                this.commentService.RemoveCommentByIdAsync(inputCommentId);

            // then
            await Assert.ThrowsAsync<CommentServiceException>(() =>
                deleteStudentCommentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentCommentException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(inputCommentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
