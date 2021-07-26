using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Articles;
using ZionCodes.Core.Models.Articles.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.Articles
{
    public partial class ArticleServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenArticleIsNullAndLogItAsync()
        {
            // given
            Article randomArticle = null;
            Article nullArticle = randomArticle;
            var nullArticleException = new NullArticleException();

            var expectedArticleValidationException =
                new ArticleValidationException(nullArticleException);

            // when
            ValueTask<Article> createArticleTask =
                this.articleService.AddArticleAsync(nullArticle);

            // then
            await Assert.ThrowsAsync<ArticleValidationException>(() =>
                createArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArticleAsync(It.IsAny<Article>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenArticleIdIsInvalidAndLogItAsync()
        {
            //given
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
            ValueTask<Article> registerArticleTask =
                this.articleService.AddArticleAsync(inputArticle);

            // then
            await Assert.ThrowsAsync<ArticleValidationException>(() =>
                registerArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArticleAsync(It.IsAny<Article>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Article randomArticle = CreateRandomArticle(dateTime);
            Article inputArticle = randomArticle;
            inputArticle.CreatedDate = default;

            var invalidArticleException = new InvalidArticleException(
                parameterName: nameof(Article.CreatedDate),
                parameterValue: inputArticle.CreatedDate);

            var expectedArticleValidationException =
                new ArticleValidationException(invalidArticleException);

            // when
            ValueTask<Article> createArticleTask =
                this.articleService.AddArticleAsync(inputArticle);

            // then
            await Assert.ThrowsAsync<ArticleValidationException>(() =>
                createArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArticleAsync(It.IsAny<Article>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedDateIsInvalidAndLogItAsync()
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
            ValueTask<Article> createArticleTask =
                this.articleService.AddArticleAsync(inputArticle);

            // then
            await Assert.ThrowsAsync<ArticleValidationException>(() =>
                createArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArticleAsync(It.IsAny<Article>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Article randomArticle = CreateRandomArticle(dateTime);
            Article inputArticle = randomArticle;
            inputArticle.UpdatedDate = GetRandomDateTime();

            var invalidArticleValidationException = new InvalidArticleException(
                parameterName: nameof(Article.UpdatedDate),
                parameterValue: inputArticle.UpdatedDate);

            var expectedArticleValidationException =
                new ArticleValidationException(invalidArticleValidationException);

            // when
            ValueTask<Article> createArticleTask =
                this.articleService.AddArticleAsync(inputArticle);

            // then
            await Assert.ThrowsAsync<ArticleValidationException>(() =>
                createArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArticleAsync(It.IsAny<Article>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Article randomArticle = CreateRandomArticle(dateTime);
            Article inputArticle = randomArticle;
            inputArticle.CreatedDate = dateTime.AddMinutes(minutes);
            inputArticle.UpdatedDate = inputArticle.CreatedDate;

            var invalidArticleValidationException = new InvalidArticleException(
                parameterName: nameof(Article.CreatedDate),
                parameterValue: inputArticle.CreatedDate);

            var expectedArticleValidationException =
                new ArticleValidationException(invalidArticleValidationException);

            // when 
            ValueTask<Article> createArticleTask =
                this.articleService.AddArticleAsync(inputArticle);

            // then
            await Assert.ThrowsAsync<ArticleValidationException>(() =>
                createArticleTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArticleAsync(It.IsAny<Article>()),
                    Times.Never);


            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenArticleAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Article randomArticle = CreateRandomArticle(dateTime);
            Article alreadyExistsArticle = randomArticle;

            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsArticleException =
                new AlreadyExistsArticleException(duplicateKeyException);

            var expectedArticleValidationException =
                new ArticleValidationException(alreadyExistsArticleException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertArticleAsync(alreadyExistsArticle))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Article> createArticleTask =
                this.articleService.AddArticleAsync(alreadyExistsArticle);

            // then
            await Assert.ThrowsAsync<ArticleValidationException>(() =>
                createArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArticleAsync(alreadyExistsArticle),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddWhenArticleTitleIsInvalidAndLogItAsync
            (string invalidArticleTitle)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Article randomArticle = CreateRandomArticle(dateTime);
            Article invalidArticle = randomArticle;
            invalidArticle.Content = invalidArticleTitle;

            var invalidArticleException = new InvalidArticleException(
                parameterName: nameof(Article.Content),
                parameterValue: invalidArticle.Content);

            var expectedArticleValidationException =
                new ArticleValidationException(invalidArticleException);

            // when
            ValueTask<Article> createArticleTask =
                this.articleService.AddArticleAsync(invalidArticle);

            // then
            await Assert.ThrowsAsync<ArticleValidationException>(() =>
                createArticleTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedArticleValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArticleAsync(It.IsAny<Article>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
