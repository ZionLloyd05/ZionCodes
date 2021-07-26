using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Article randomArticle = CreateRandomArticle(dateTime);
            Article inputArticle = randomArticle;
            var sqlException = GetSqlException();

            var expectedArticleDependencyException =
                new ArticleDependencyException(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertArticleAsync(inputArticle))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Article> createArticleTask =
                this.articleService.AddArticleAsync(inputArticle);

            // then
            await Assert.ThrowsAsync<ArticleDependencyException>(() =>
                createArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedArticleDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArticleAsync(inputArticle),
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
            Article randomArticle = CreateRandomArticle(dateTime);
            Article inputArticle = randomArticle;
            var databaseUpdateException = new DbUpdateException();

            var expectedArticleDependencyException =
                new ArticleDependencyException(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertArticleAsync(inputArticle))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Article> createArticleTask =
                this.articleService.AddArticleAsync(inputArticle);

            // then
            await Assert.ThrowsAsync<ArticleDependencyException>(() =>
                createArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArticleAsync(inputArticle),
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
            Article randomArticle = CreateRandomArticle(dateTime);
            Article inputArticle = randomArticle;
            var exception = new Exception();

            var expectedArticleServiceException =
                new ArticleServiceException(exception);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertArticleAsync(inputArticle))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Article> createArticleTask =
                 this.articleService.AddArticleAsync(inputArticle);

            // then
            await Assert.ThrowsAsync<ArticleServiceException>(() =>
                createArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArticleAsync(inputArticle),
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
