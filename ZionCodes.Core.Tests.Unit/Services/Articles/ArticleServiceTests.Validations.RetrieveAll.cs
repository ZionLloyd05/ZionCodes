using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Articles;

namespace ZionCodes.Core.Tests.Unit.Services.Articles
{
    public partial class ArticleServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenArticlesWasEmptyAndLogIt()
        {
            // given
            IQueryable<Article> emptyStorageArticles = new List<Article>().AsQueryable();
            IQueryable<Article> expectedArticles = emptyStorageArticles;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllArticles())
                    .Returns(expectedArticles);

            // when
            IQueryable<Article> actualArticle =
                this.articleService.RetrieveAllArticles();

            // then
            actualArticle.Should().BeEquivalentTo(expectedArticles);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No articles found in storage."));

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllArticles(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
