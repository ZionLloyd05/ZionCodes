using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Comments.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.Comments
{
    public partial class CommentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllCommentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedCommentDependencyException =
                new CommentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllComments())
                    .Throws(sqlException);

            // when . then
            Assert.Throws<CommentDependencyException>(() =>
                this.commentService.RetrieveAllComments());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllComments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCommentDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllCommentsWhenExceptionOccursAndLogIt()
        {
            // given
            var exception = new Exception();

            var expectedCommentServiceException =
                new CommentServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllComments())
                    .Throws(exception);

            // when . then
            Assert.Throws<CommentServiceException>(() =>
                this.commentService.RetrieveAllComments());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllComments(),
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
