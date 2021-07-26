using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Articles;
using ZionCodes.Core.Models.Articles.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.Articles
{
    public partial class ArticleServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomArticleId = default;
            Guid inputArticleId = randomArticleId;

            var invalidArticleInputException = new InvalidArticleException(
                parameterName: nameof(Article.Id),
                parameterValue: inputArticleId);

            var expectedArticleValidationException =
                new ArticleValidationException(invalidArticleInputException);

            // when
            ValueTask<Article> deleteArticleTask =
               this.articleService.RemoveArticleByIdAsync(inputArticleId);

            // then
            await Assert.ThrowsAsync<ArticleValidationException>(() => deleteArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteArticleAsync(It.IsAny<Article>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenStorageArticleIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Article randomArticle = CreateRandomArticle(dateTime);
            Guid inputArticleId = randomArticle.Id;
            Article inputArticle = randomArticle;
            Article nullStorageArticle = null;

            var notFoundArticleException = new NotFoundArticleException(inputArticleId);

            var expectedArticleValidationException =
                new ArticleValidationException(notFoundArticleException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectArticleByIdAsync(inputArticleId))
                    .ReturnsAsync(nullStorageArticle);

            // when
            ValueTask<Article> actualArticleTask =
                this.articleService.RemoveArticleByIdAsync(inputArticleId);

            // then
            await Assert.ThrowsAsync<ArticleValidationException>(() => actualArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(inputArticleId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteArticleAsync(It.IsAny<Article>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
