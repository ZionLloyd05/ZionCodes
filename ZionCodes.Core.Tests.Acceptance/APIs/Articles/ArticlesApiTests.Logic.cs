using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using RESTFulSense.Exceptions;
using Xunit;
using ZionCodes.Core.Tests.Acceptance.Models.Articles;

namespace ZionCodes.Core.Tests.Acceptance.APIs.Articles
{
    public partial class ArticlesApiTests
    {
        [Fact]
        public async Task ShouldPostArticleAsync()
        {
            // given
            Article randomArticle = await CreateRandomArticle();
            Article inputArticle = randomArticle;
            Article expectedArticle = inputArticle;

            // when 
            await this.apiBroker.PostArticleAsync(inputArticle);

            Article actualArticle =
                 await this.apiBroker.GetArticleByIdAsync(inputArticle.Id);

            var calculatedCreatedDate = actualArticle.CreatedDate.AddHours(actualArticle.CreatedDate.Offset.Hours);
            var calculatedUpdatedDate = actualArticle.CreatedDate.AddHours(actualArticle.UpdatedDate.Offset.Hours);

            actualArticle.CreatedDate =
                DateTimeOffset.FromUnixTimeSeconds(calculatedCreatedDate.ToUnixTimeSeconds());
            actualArticle.UpdatedDate =
                DateTimeOffset.FromUnixTimeSeconds(calculatedUpdatedDate.ToUnixTimeSeconds());

            // then
            actualArticle.Id.Should().Be(expectedArticle.Id);
            actualArticle.Title.Should().Be(expectedArticle.Title);
            actualArticle.Content.Should().Be(expectedArticle.Content);
            actualArticle.CreatedBy.Should().Be(expectedArticle.CreatedBy);
            actualArticle.UpdatedBy.Should().Be(expectedArticle.UpdatedBy);
            actualArticle.CreatedDate.Date.Should().Be(expectedArticle.CreatedDate.Date);
            actualArticle.UpdatedDate.Date.Should().Be(expectedArticle.UpdatedDate.Date);

            await this.apiBroker.DeleteArticleByIdAsync(actualArticle.Id);
        }

        [Fact]
        public async Task ShouldPutArticleAsync()
        {
            // given
            Article randomArticle = await PostRandomArticleAsync();
            Article modifiedArticle = UpdateArticleRandom(randomArticle);

            // when
            await this.apiBroker.PutArticleAsync(modifiedArticle);

            Article actualArticle =
                await this.apiBroker.GetArticleByIdAsync(randomArticle.Id);

            // then
            actualArticle.Id.Should().Be(modifiedArticle.Id);
            actualArticle.Title.Should().Be(modifiedArticle.Title);
            actualArticle.Content.Should().Be(modifiedArticle.Content);
            actualArticle.CreatedBy.Should().Be(modifiedArticle.CreatedBy);
            actualArticle.UpdatedBy.Should().Be(modifiedArticle.UpdatedBy);
            actualArticle.CreatedDate.Date.Should().Be(modifiedArticle.CreatedDate.Date);
            actualArticle.UpdatedDate.Date.Should().Be(modifiedArticle.UpdatedDate.Date);

            await this.apiBroker.DeleteArticleByIdAsync(actualArticle.Id);
        }

        [Fact]
        public async Task ShouldGetAllArticlesAsync()
        {
            //given
            var randomArticles = new List<Article>();

            for (var i = 0; i <= GetRandomNumber(); i++)
            {
                randomArticles.Add(await PostRandomArticleAsync());
            }

            List<Article> inputedArticles = randomArticles;
            List<Article> expectedArticles = inputedArticles.ToList();

            //when 
            List<Article> actualArticles = await this.apiBroker.GetAllArticlesAsync();

            //then
            foreach (var expectedArticle in expectedArticles)
            {
                Article actualArticle = actualArticles.Single(article => article.Id == expectedArticle.Id);

                actualArticle.Id.Should().Be(expectedArticle.Id);
                actualArticle.Title.Should().Be(expectedArticle.Title);
                actualArticle.Content.Should().Be(expectedArticle.Content);
                actualArticle.CreatedBy.Should().Be(expectedArticle.CreatedBy);
                actualArticle.UpdatedBy.Should().Be(expectedArticle.UpdatedBy);
                actualArticle.CreatedDate.Date.Should().Be(expectedArticle.CreatedDate.Date);
                actualArticle.UpdatedDate.Date.Should().Be(expectedArticle.UpdatedDate.Date);

                await this.apiBroker.DeleteArticleByIdAsync(actualArticle.Id);
            }
        }

        [Fact]
        public async Task ShouldDeleteArticleAsync()
        {
            //given
            Article randomArticle = await PostRandomArticleAsync();
            Article inputArticle = randomArticle;
            Article expectedArticle = inputArticle;

            //when
            Article deletedArticle =
                await this.apiBroker.DeleteArticleByIdAsync(inputArticle.Id);

            ValueTask<Article> getArticleByIdTask =
                this.apiBroker.DeleteArticleByIdAsync(inputArticle.Id);

            // then
            deletedArticle.Id.Should().Be(expectedArticle.Id);
            deletedArticle.Title.Should().Be(expectedArticle.Title);
            deletedArticle.Content.Should().Be(expectedArticle.Content);
            deletedArticle.CreatedBy.Should().Be(expectedArticle.CreatedBy);
            deletedArticle.UpdatedBy.Should().Be(expectedArticle.UpdatedBy);
            deletedArticle.CreatedDate.Date.Should().Be(expectedArticle.CreatedDate.Date);
            deletedArticle.UpdatedDate.Date.Should().Be(expectedArticle.UpdatedDate.Date);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getArticleByIdTask.AsTask());
        }
    }
}
