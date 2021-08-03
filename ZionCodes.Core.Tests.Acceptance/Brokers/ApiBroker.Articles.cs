using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZionCodes.Core.Tests.Acceptance.Models.Articles;

namespace ZionCodes.Core.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string ArticlesRelativeUrl = "api/articles";

        public async ValueTask<Article> PostArticleAsync(Article article) =>
            await this.apiFactoryClient.PostContentAsync(ArticlesRelativeUrl, article);

        public async ValueTask<Article> GetArticleByIdAsync(Guid articleId) =>
            await this.apiFactoryClient.GetContentAsync<Article>($"{ArticlesRelativeUrl}/{articleId}");

        public async ValueTask<Article> DeleteArticleByIdAsync(Guid articleId) =>
            await this.apiFactoryClient.DeleteContentAsync<Article>($"{ArticlesRelativeUrl}/{articleId}");

        public async ValueTask<Article> PutArticleAsync(Article article) =>
            await this.apiFactoryClient.PutContentAsync(ArticlesRelativeUrl, article);

        public async ValueTask<List<Article>> GetAllArticlesAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Article>>($"{ArticlesRelativeUrl}/");
    }
}
