using System;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Articles.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.Articles
{
    public partial class ArticleServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllArticlesWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedArticleDependencyException =
                new ArticleDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllArticles())
                    .Throws(sqlException);

            // when . then
            Assert.Throws<ArticleDependencyException>(() =>
                this.articleService.RetrieveAllArticles());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllArticles(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedArticleDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllArticlesWhenExceptionOccursAndLogIt()
        {
            // given
            var exception = new Exception();

            var expectedArticleServiceException =
                new ArticleServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllArticles())
                    .Throws(exception);

            // when . then
            Assert.Throws<ArticleServiceException>(() =>
                this.articleService.RetrieveAllArticles());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllArticles(),
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
