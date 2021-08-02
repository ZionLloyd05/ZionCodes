using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ZionCodes.Core.Models.ArticleComments;

namespace ZionCodes.Core.Brokers.Storages
{
    public partial class StorageBroker : IStorageBroker
    {
        public DbSet<ArticleComment> ArticleComments { get; set; }

        public async ValueTask<ArticleComment> InsertArticleCommentAsync(ArticleComment ArticleComment)
        {
            EntityEntry<ArticleComment> ArticleCommentEntityEntry = await this.ArticleComments.AddAsync(ArticleComment);
            await this.SaveChangesAsync();

            return ArticleCommentEntityEntry.Entity;
        }

        public IQueryable<ArticleComment> SelectAllArticleComments() => this.ArticleComments.AsQueryable();

        public async ValueTask<ArticleComment> SelectArticleCommentByIdAsync(Guid ArticleCommentId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await ArticleComments.FindAsync(ArticleCommentId);
        }

        public async ValueTask<ArticleComment> UpdateArticleCommentAsync(ArticleComment ArticleComment)
        {
            EntityEntry<ArticleComment> ArticleCommentEntityEntry = this.ArticleComments.Update(ArticleComment);
            await this.SaveChangesAsync();

            return ArticleCommentEntityEntry.Entity;
        }

        public async ValueTask<ArticleComment> DeleteArticleCommentAsync(ArticleComment ArticleComment)
        {
            EntityEntry<ArticleComment> ArticleCommentEntityEntry = this.ArticleComments.Remove(ArticleComment);
            await this.SaveChangesAsync();

            return ArticleCommentEntityEntry.Entity;
        }
    }
}
