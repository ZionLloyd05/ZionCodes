using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Articles;

namespace ZionCodes.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        public ValueTask<Article> InsertArticleAsync(Article article);
        public IQueryable<Article> SelectAllArticles();
        public ValueTask<Article> SelectArticleByIdAsync(Guid articleId);
        public ValueTask<Article> UpdateArticleAsync(Article article);
        public ValueTask<Article> DeleteArticleAsync(Article article);
    }
}
