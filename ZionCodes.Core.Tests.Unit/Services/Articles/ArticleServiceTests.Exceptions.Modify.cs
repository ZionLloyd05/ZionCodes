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
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Article randomArticle = CreateRandomArticle(randomDateTime);
            Article someArticle = randomArticle;
            someArticle.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

            var expectedArticleDependencyException =
                new ArticleDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectArticleByIdAsync(someArticle.Id))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Article> modifyArticleTask =
                this.articleService.ModifyArticleAsync(someArticle);

            // then
            await Assert.ThrowsAsync<ArticleDependencyException>(() =>
                modifyArticleTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(someArticle.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedArticleDependencyException))),
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
            Article someArticle = CreateRandomArticle(randomDateTime);
            someArticle.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var expectedArticleDependencyException =
                new ArticleDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectArticleByIdAsync(someArticle.Id))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Article> modifyArticleTask =
                this.articleService.ModifyArticleAsync(someArticle);

            // then
            await Assert.ThrowsAsync<ArticleDependencyException>(() =>
                modifyArticleTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(someArticle.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleDependencyException))),
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
            Article randomArticle = CreateRandomArticle(randomDateTime);
            Article someArticle = randomArticle;
            someArticle.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedArticleException = new LockedArticleException(databaseUpdateConcurrencyException);

            var expectedArticleDependencyException =
                new ArticleDependencyException(lockedArticleException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectArticleByIdAsync(someArticle.Id))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Article> modifyArticleTask =
                this.articleService.ModifyArticleAsync(someArticle);

            // then
            await Assert.ThrowsAsync<ArticleDependencyException>(() =>
                modifyArticleTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(someArticle.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleDependencyException))),
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
            Article randomArticle = CreateRandomArticle(randomDateTime);
            Article someArticle = randomArticle;
            someArticle.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var serviceException = new Exception();

            var expectedArticleServiceException =
                new ArticleServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectArticleByIdAsync(someArticle.Id))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Article> modifyArticleTask =
                this.articleService.ModifyArticleAsync(someArticle);

            // then
            await Assert.ThrowsAsync<ArticleServiceException>(() =>
                modifyArticleTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(someArticle.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleServiceException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
