using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ZionCodes.Core.Models.Comments;

namespace ZionCodes.Core.Brokers.Storages
{
    public partial class StorageBroker : IStorageBroker
    {
        public DbSet<Comment> Comments { get; set; }

        public async ValueTask<Comment> InsertCommentAsync(Comment comment)
        {
            EntityEntry<Comment> commentEntityEntry = await this.Comments.AddAsync(comment);
            await this.SaveChangesAsync();

            return commentEntityEntry.Entity;
        }

        public ICollection<Comment> SelectAllComments() => this.Comments.ToList();

        public async ValueTask<Comment> SelectCommentByIdAsync(int commentId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Comments.FindAsync(commentId);
        }

        public async ValueTask<Comment> UpdateCommentAsync(Comment comment)
        {
            EntityEntry<Comment> commentEntityEntry = this.Comments.Update(comment);
            await this.SaveChangesAsync();

            return commentEntityEntry.Entity;
        }

        public async ValueTask<Comment> DeleteCommentAsync(Comment comment)
        {
            EntityEntry<Comment> commentEntityEntry = this.Comments.Remove(comment);
            await this.SaveChangesAsync();

            return commentEntityEntry.Entity;
        }
    }
}
