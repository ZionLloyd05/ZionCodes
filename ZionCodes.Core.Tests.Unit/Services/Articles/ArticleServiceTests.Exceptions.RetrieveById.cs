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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomArticleId = 1;
            int inputArticleId = randomArticleId;
            SqlException sqlException = GetSqlException();

            var exceptionArticleDependencyException =
                new ArticleDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectArticleByIdAsync(inputArticleId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Article> retrieveArticleByIdTask =
                this.articleService.RetrieveArticleByIdAsync(inputArticleId);

            // then
            await Assert.ThrowsAsync<ArticleDependencyException>(() =>
                retrieveArticleByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(exceptionArticleDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(inputArticleId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            int someArticleId = 1;
            var databaseUpdateException = new DbUpdateException();

            var expectedArticleDependencyException =
                new ArticleDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectArticleByIdAsync(It.IsAny<int>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Article> retrieveByIdArticleTask =
                this.articleService.RetrieveArticleByIdAsync(someArticleId);

            // then
            await Assert.ThrowsAsync<ArticleDependencyException>(() =>
                retrieveByIdArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(It.IsAny<int>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            int someArticleId = 1;
            var exception = new Exception();

            var expectedArticleServiceException =
                new ArticleServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectArticleByIdAsync(It.IsAny<int>()))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Article> retrieveByIdArticleTask =
                this.articleService.RetrieveArticleByIdAsync(someArticleId);

            // then
            await Assert.ThrowsAsync<ArticleServiceException>(() =>
                retrieveByIdArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(It.IsAny<int>()),
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
            int someArticleId = 1;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedArticleException =
                new LockedArticleException(databaseUpdateConcurrencyException);

            var expectedArticleDependencyException =
                new ArticleDependencyException(lockedArticleException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectArticleByIdAsync(It.IsAny<int>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Article> retrieveByIdArticleTask =
                this.articleService.RetrieveArticleByIdAsync(someArticleId);

            // then
            await Assert.ThrowsAsync<ArticleDependencyException>(() =>
                retrieveByIdArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(It.IsAny<int>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
