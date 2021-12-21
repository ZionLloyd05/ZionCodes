using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Articles;
using ZionCodes.Core.Models.Articles.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.Articles
{
    public partial class ArticleServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomArticleId = 1;
            int inputArticleId = randomArticleId;
            SqlException sqlException = GetSqlException();

            var expectedArticleDependencyException =
                new ArticleDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectArticleByIdAsync(inputArticleId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Article> deleteArticleTask =
                this.articleService.RemoveArticleByIdAsync(inputArticleId);

            // then
            await Assert.ThrowsAsync<ArticleDependencyException>(() =>
                deleteArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedArticleDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(inputArticleId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteArticleAsync(It.IsAny<Article>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            int randomArticleId = 1;
            int inputArticleId = randomArticleId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedArticleException =
                new LockedArticleException(databaseUpdateConcurrencyException);

            var expectedStudentArticleException =
                new ArticleDependencyException(lockedArticleException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectArticleByIdAsync(inputArticleId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Article> deleteStudentArticleTask =
                this.articleService.RemoveArticleByIdAsync(inputArticleId);

            // then
            await Assert.ThrowsAsync<ArticleDependencyException>(() => deleteStudentArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentArticleException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(inputArticleId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            int randomArticleId = 1;
            int inputArticleId = randomArticleId;
            var exception = new Exception();

            var expectedStudentArticleException =
                new ArticleServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectArticleByIdAsync(inputArticleId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Article> deleteStudentArticleTask =
                this.articleService.RemoveArticleByIdAsync(inputArticleId);

            // then
            await Assert.ThrowsAsync<ArticleServiceException>(() =>
                deleteStudentArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentArticleException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(inputArticleId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
