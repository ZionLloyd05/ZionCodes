using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Brokers.DateTimes;
using ZionCodes.Core.Brokers.Loggings;
using ZionCodes.Core.Brokers.Storages;
using ZionCodes.Core.Models.Articles;

namespace ZionCodes.Core.Services.Articles
{
    public partial class ArticleService : IArticleService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ArticleService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker
            )
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }


        public ValueTask<Article> AddArticleAsync(Article article) =>
            TryCatch(async () =>
            {
                ValidateArticleOnCreate(article);

                return await this.storageBroker.InsertArticleAsync(article);
            });

        public ICollection<Article> RetrieveAllArticles() =>
            TryCatch(() =>
            {
                ICollection<Article> storageArticles = this.storageBroker.SelectAllArticles();

                ValidateStorageArticles(storageArticles);

                return storageArticles;
            });

        public ValueTask<Article> RetrieveArticleByIdAsync(int articleId) =>
            TryCatch(async () =>
            {
                ValidateArticleId(articleId);
                Article storageArticle =
                    await this.storageBroker.SelectArticleByIdAsync(articleId);

                ValidateStorageArticle(storageArticle, articleId);

                return storageArticle;
            });

        public ValueTask<Article> ModifyArticleAsync(Article article) =>
            TryCatch(async () =>
            {
                ValidateArticleOnModify(article);
                ValidateArticleIdIsNull(article.Id);
                Article maybeArticle =
                        await this.storageBroker.SelectArticleByIdAsync(article.Id);

                return await this.storageBroker.UpdateArticleAsync(article);
            });

        public ValueTask<Article> RemoveArticleByIdAsync(int articleId) =>
            TryCatch(async () =>
            {
                ValidateArticleIdIsNull(articleId);
                Article storageArticle =
                await this.storageBroker.SelectArticleByIdAsync(articleId);

                ValidateStorageArticle(storageArticle, articleId);

                return await this.storageBroker.DeleteArticleAsync(storageArticle);
            });

    }
}
