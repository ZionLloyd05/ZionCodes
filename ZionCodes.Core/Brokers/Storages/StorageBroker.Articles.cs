using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ZionCodes.Core.Models.Articles;

namespace ZionCodes.Core.Brokers.Storages
{
    public partial class StorageBroker : IStorageBroker
    {
        public DbSet<Article> Articles { get; set; }

        public async ValueTask<Article> InsertArticleAsync(Article article)
        {
            EntityEntry<Article> articleEntityEntry = await this.Articles.AddAsync(article);
            await this.SaveChangesAsync();

            return articleEntityEntry.Entity;
        }

        public IQueryable<Article> SelectAllArticles() => this.Articles.AsQueryable();

        public async ValueTask<Article> SelectArticleByIdAsync(Guid articleId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Articles.FindAsync(articleId);
        }

        public async ValueTask<Article> UpdateArticleAsync(Article article)
        {
            EntityEntry<Article> articleEntityEntry = this.Articles.Update(article);
            await this.SaveChangesAsync();

            return articleEntityEntry.Entity;
        }

        public async ValueTask<Article> DeleteArticleAsync(Article article)
        {
            EntityEntry<Article> articleEntityEntry = this.Articles.Remove(article);
            await this.SaveChangesAsync();

            return articleEntityEntry.Entity;
        }
    }
}
