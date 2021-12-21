using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Articles;

namespace ZionCodes.Core.Tests.Unit.Services.Articles
{
    public partial class ArticleServiceTests
    {
        [Fact]
        public async Task ShouldAddArticleAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            Article randomArticle = CreateRandomArticle(dateTime);
            Article inputArticle = randomArticle;
            inputArticle.UpdatedDate = inputArticle.CreatedDate;
            Article expectedArticle = inputArticle;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertArticleAsync(inputArticle))
                    .ReturnsAsync(expectedArticle);

            //when
            Article actualArticle =
                await this.articleService.AddArticleAsync(inputArticle);

            //then
            actualArticle.Should().BeEquivalentTo(expectedArticle);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArticleAsync(inputArticle),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetriveAllArticles()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ICollection<Article> randomArticles =
                CreateRandomArticles(randomDateTime);

            ICollection<Article> storageArticles =
                randomArticles;

            ICollection<Article> expectedArticles =
                storageArticles;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllArticles())
                    .Returns(storageArticles);

            // when
            ICollection<Article> actualArticles =
                this.articleService.RetrieveAllArticles();

            // then
            actualArticles.Should().BeEquivalentTo(expectedArticles);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllArticles(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveArticleByIdAsync()
        {
            // given
            int randomArticleId = 1;
            int inputArticleId = randomArticleId;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Article randomArticle = CreateRandomArticle(randomDateTime);
            Article storageArticle = randomArticle;
            Article expectedArticle = storageArticle;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectArticleByIdAsync(inputArticleId))
                    .ReturnsAsync(storageArticle);

            // when
            Article actualArticle =
                await this.articleService.RetrieveArticleByIdAsync(inputArticleId);

            // then
            actualArticle.Should().BeEquivalentTo(expectedArticle);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(inputArticleId),
                    Times.Once);


            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldModifyArticleAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Article randomArticle = CreateRandomArticle(randomInputDate);
            Article inputArticle = randomArticle;
            Article afterUpdateStorageArticle = inputArticle;
            Article expectedArticle = afterUpdateStorageArticle;
            Article beforeUpdateStorageArticle = randomArticle.DeepClone();
            inputArticle.UpdatedDate = randomDate;
            int articleId = inputArticle.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectArticleByIdAsync(articleId))
                    .ReturnsAsync(beforeUpdateStorageArticle);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateArticleAsync(inputArticle))
                    .ReturnsAsync(afterUpdateStorageArticle);

            // when
            Article actualArticle =
                await this.articleService.ModifyArticleAsync(inputArticle);

            // then
            actualArticle.Should().BeEquivalentTo(expectedArticle);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(articleId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateArticleAsync(inputArticle),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldDeleteArticleByIdAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            Article randomArticle = CreateRandomArticle(dateTime);
            Article inputArticle = randomArticle;
            int inputArticleId = inputArticle.Id;
            inputArticle.UpdatedDate = inputArticle.CreatedDate;
            Article storageArticle = inputArticle;
            Article expectedArticle = inputArticle;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectArticleByIdAsync(inputArticleId))
                    .ReturnsAsync(inputArticle);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteArticleAsync(inputArticle))
                    .ReturnsAsync(storageArticle);

            // when
            Article actualArticle =
                await this.articleService.RemoveArticleByIdAsync(inputArticleId);

            // then
            actualArticle.Should().BeEquivalentTo(expectedArticle);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectArticleByIdAsync(inputArticleId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteArticleAsync(inputArticle),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
