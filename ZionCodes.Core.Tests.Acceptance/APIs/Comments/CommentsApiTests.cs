using System;
using System.Threading.Tasks;
using Tynamix.ObjectFiller;
using Xunit;
using ZionCodes.Core.Models.Comments;
using ZionCodes.Core.Tests.Acceptance.Brokers;
using ZionCodes.Core.Tests.Acceptance.Models.Articles;
using ZionCodes.Core.Tests.Acceptance.Models.Categories;

namespace ZionCodes.Core.Tests.Acceptance.APIs.Comments
{
    [Collection(nameof(ApiTestCollection))]
    public partial class CommentsApiTests
    {
        private readonly ApiBroker apiBroker;

        public CommentsApiTests(ApiBroker commentApiBroker) =>
            this.apiBroker = commentApiBroker;

        private async ValueTask<Category> PostCategoryAsync()
        {
            Category randomCategory = CreateRandomCategory();
            Category inputCategory = randomCategory;

            return await this.apiBroker.PostCategoryAsync(inputCategory);
        }

        private async ValueTask<Article> PostRandomArticleAsync()
        {
            Category category = await PostCategoryAsync();

            Article article = CreateRandomArticleFiller(category.Id).Create();

            return await this.apiBroker.PostArticleAsync(article);
        }

        private Comment CreateRandomComment(int articleId) =>
             CreateRandomCommentFiller(articleId).Create();

        private async ValueTask<Comment> PostRandomCommentAsync()
        {
            Article article = await PostRandomArticleAsync();
            Comment randomComment = CreateRandomComment(article.Id);
            await this.apiBroker.PostCommentAsync(randomComment);

            return randomComment;
        }


        private static Filler<Article> CreateRandomArticleFiller(int categoryId)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            int posterId = 1;

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

        private static Category CreateRandomCategory()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            int posterId = 1;
            var filler = new Filler<Category>();

            filler.Setup()
                .OnProperty(assignment => assignment.CreatedBy).Use(posterId)
                .OnProperty(assignment => assignment.UpdatedBy).Use(posterId)
                .OnProperty(assignment => assignment.CreatedDate).Use(now)
                .OnProperty(assignment => assignment.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static Comment UpdateCommentRandom(Comment comment)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Comment>();

            filler.Setup()
                .OnProperty(comment => comment.ArticleId).Use(comment.ArticleId)
                .OnProperty(comment => comment.Id).Use(comment.Id)
                .OnProperty(comment => comment.CreatedDate).Use(comment.CreatedDate)
                .OnProperty(comment => comment.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static Filler<Comment> CreateRandomCommentFiller(int articleId)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            //   Article randomArticle = await PostRandomArticleAsync();
            int posterId = 1;

            var filler = new Filler<Comment>();

            filler.Setup()
                .OnProperty(comment => comment.ArticleId).Use(articleId)
                .OnProperty(comment => comment.CreatedDate).Use(now)
                .OnProperty(comment => comment.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
    }
}
