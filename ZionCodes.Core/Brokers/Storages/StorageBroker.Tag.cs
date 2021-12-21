using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ZionCodes.Core.Models.Tags;

namespace ZionCodes.Core.Brokers.Storages
{
    public partial class StorageBroker : IStorageBroker
    {
        public DbSet<Tag> Tags { get; set; }

        public async ValueTask<Tag> InsertTagAsync(Tag tag)
        {
            EntityEntry<Tag> tagEntityEntry = await this.Tags.AddAsync(tag);
            await this.SaveChangesAsync();

            return tagEntityEntry.Entity;
        }

        public ICollection<Tag> SelectAllTags() => this.Tags.ToList();

        public async ValueTask<Tag> SelectTagByIdAsync(int tagId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Tags.FindAsync(tagId);
        }

        public async ValueTask<Tag> UpdateTagAsync(Tag tag)
        {
            EntityEntry<Tag> tagEntityEntry = this.Tags.Update(tag);
            await this.SaveChangesAsync();

            return tagEntityEntry.Entity;
        }

        public async ValueTask<Tag> DeleteTagAsync(Tag tag)
        {
            EntityEntry<Tag> tagEntityEntry = this.Tags.Remove(tag);
            await this.SaveChangesAsync();

            return tagEntityEntry.Entity;
        }

    }
}