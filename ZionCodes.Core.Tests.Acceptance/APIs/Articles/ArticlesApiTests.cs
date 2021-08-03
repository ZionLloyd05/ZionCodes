using System;
using System.Threading.Tasks;
using Tynamix.ObjectFiller;
using Xunit;
using ZionCodes.Core.Tests.Acceptance.Brokers;
using ZionCodes.Core.Tests.Acceptance.Models.Articles;
using ZionCodes.Core.Tests.Acceptance.Models.Categories;

namespace ZionCodes.Core.Tests.Acceptance.APIs.Articles
{
    [Collection(nameof(ApiTestCollection))]
    public partial class ArticlesApiTests
    {
        private readonly ApiBroker apiBroker;

        public ArticlesApiTests(ApiBroker articleApiBroker) =>
            this.apiBroker = articleApiBroker;

        private async Task<Article> CreateRandomArticle()
        {
            Category persistedCategory = await PostCategoryAsync();

            Article persistedArticle =
                CreateRandomArticleFiller(persistedCategory.Id).Create();

            return persistedArticle;
        }

        private async ValueTask<Article> PostRandomArticleAsync()
        {
            Article randomArticle = await CreateRandomArticle();
            await this.apiBroker.PostArticleAsync(randomArticle);

            return randomArticle;
        }

        private async ValueTask<Category> PostCategoryAsync()
        {
            Category randomCategory = CreateRandomCategory();
            Category inputCategory = randomCategory;

            return await this.apiBroker.PostCategoryAsync(inputCategory);
        }

        private static Category CreateRandomCategory()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<Category>();

            filler.Setup()
                .OnProperty(assignment => assignment.CreatedBy).Use(posterId)
                .OnProperty(assignment => assignment.UpdatedBy).Use(posterId)
                .OnProperty(assignment => assignment.CreatedDate).Use(now)
                .OnProperty(assignment => assignment.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static Article UpdateArticleRandom(Article article)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Article>();

            filler.Setup()
                .OnProperty(article => article.CategoryId).Use(article.CategoryId)
                .OnProperty(article => article.Id).Use(article.Id)
                .OnProperty(article => article.CreatedBy).Use(article.CreatedBy)
                .OnProperty(article => article.UpdatedBy).Use(article.UpdatedBy)
                .OnProperty(article => article.CreatedDate).Use(article.CreatedDate)
                .OnProperty(article => article.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static Filler<Article> CreateRandomArticleFiller(Guid categoryId)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();

            var filler = new Filler<Article>();

            filler.Setup()
                .OnProperty(article => article.CategoryId).Use(categoryId)
                .OnProperty(article => article.CreatedBy).Use(posterId)
                .OnProperty(article => article.UpdatedBy).Use(posterId)
                .OnProperty(article => article.CreatedDate).Use(now)
                .OnProperty(article => article.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
    }
}
