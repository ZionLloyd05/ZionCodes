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
        public async void ShouldThrowValidationExceptionOnModifyWhenArticleIsNullAndLogItAsync()
        {
            // given
            Article randomArticle = null;
            Article nullArticle = randomArticle;
            var nullArticleException = new NullArticleException();

            var expectedArticleValidationException =
                new ArticleValidationException(nullArticleException);

            // when
            ValueTask<Article> modifyArticleTask =
                this.articleService.ModifyArticleAsync(nullArticle);

            // then
            await Assert.ThrowsAsync<ArticleValidationException>(() =>
                modifyArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Article randomArticle = CreateRandomArticle(dateTime);
            Article inputArticle = randomArticle;
            inputArticle.Id = default;

            var invalidArticleInputException = new InvalidArticleException(
                parameterName: nameof(Article.Id),
                parameterValue: inputArticle.Id);

            var expectedArticleValidationException =
                new ArticleValidationException(invalidArticleInputException);

            // when
            ValueTask<Article> modifyArticleTask =
                this.articleService.ModifyArticleAsync(inputArticle);

            // then
            await Assert.ThrowsAsync<ArticleValidationException>(() =>
                modifyArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenArticleBodyIsInvalidAndLogItAsync(
           string invalidArticleBody)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Article randomArticle = CreateRandomArticle(dateTime);
            Article invalidArticle = randomArticle;
            invalidArticle.Content = invalidArticleBody;

            var invalidArticleException = new InvalidArticleException(
               parameterName: nameof(Article.Content),
               parameterValue: invalidArticle.Content);

            var expectedArticleValidationException =
                new ArticleValidationException(invalidArticleException);

            // when
            ValueTask<Article> modifyArticleTask =
                this.articleService.ModifyArticleAsync(invalidArticle);

            // then
            await Assert.ThrowsAsync<ArticleValidationException>(() =>
                modifyArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Article randomArticle = CreateRandomArticle(dateTime);
            Article inputArticle = randomArticle;
            inputArticle.UpdatedDate = default;

            var invalidArticleException = new InvalidArticleException(
                parameterName: nameof(Article.UpdatedDate),
                parameterValue: inputArticle.UpdatedDate);

            var expectedArticleValidationException =
                new ArticleValidationException(invalidArticleException);

            // when
            ValueTask<Article> modifyArticleTask =
                this.articleService.ModifyArticleAsync(inputArticle);

            // then
            await Assert.ThrowsAsync<ArticleValidationException>(() =>
                modifyArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
