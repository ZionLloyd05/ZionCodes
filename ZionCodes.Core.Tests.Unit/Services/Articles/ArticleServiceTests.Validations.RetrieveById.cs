using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Articles;
using ZionCodes.Core.Models.Articles.Exceptions;
using ZionCodes.Core.Models.Categories.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.Articles
{
    public partial class ArticleServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            // given
            int randomArticleId = default;
            int inputArticleId = randomArticleId;

            var invalidArticleInputException = new InvalidArticleInputException(
                    parameterName: nameof(Article.Id),
                    parameterValue: inputArticleId);

            var expectedArticleValidationException =
                new ArticleValidationException(invalidArticleInputException);

            // when
            ValueTask<Article> retrieveArticleByIdTask =
                this.articleService.RetrieveArticleByIdAsync(inputArticleId);

            // then
            await Assert.ThrowsAsync<ArticleValidationException>(() =>
                retrieveArticleByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(It.IsAny<int>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageArticleIsNullAndLogItAsync()
        {
            // given
            int randomArticleId = 1;
            int someArticleId = randomArticleId;
            Article invalidStorageArticle = null;
            var notFoundArticleException = new NotFoundArticleException(someArticleId);

            var exceptionArticleValidationException =
                new ArticleValidationException(notFoundArticleException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectArticleByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(invalidStorageArticle);

            // when
            ValueTask<Article> retrieveArticleByIdTask =
                this.articleService.RetrieveArticleByIdAsync(someArticleId);

            // then
            await Assert.ThrowsAsync<ArticleValidationException>(() =>
                retrieveArticleByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(exceptionArticleValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(It.IsAny<int>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
