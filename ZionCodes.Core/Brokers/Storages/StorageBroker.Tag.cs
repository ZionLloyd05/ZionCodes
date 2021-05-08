using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ZionCodes.Core.Models.Tags;

namespace ZionCodes.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Tag> Tags { get; set; }

        public async ValueTask<Tag> InsertTagAsync(Tag Tag)
        {
            EntityEntry<Tag> TagEntityEntry = await this.Tags.AddAsync(Tag);
            await this.SaveChangesAsync();

            return TagEntityEntry.Entity;
        }

        public IQueryable<Tag> SelectAllTags() => this.Tags.AsQueryable();

        public async ValueTask<Tag> SelectTagByIdAsync(Guid TagId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Tags.FindAsync(TagId);
        }

        public async ValueTask<Tag> UpdateTagAsync(Tag Tag)
        {
            EntityEntry<Tag> TagEntityEntry = this.Tags.Update(Tag);
            await this.SaveChangesAsync();

            return TagEntityEntry.Entity;
        }

        public async ValueTask<Tag> DeleteTagAsync(Tag Tag)
        {
            EntityEntry<Tag> TagEntityEntry = this.Tags.Remove(Tag);
            await this.SaveChangesAsync();

            return TagEntityEntry.Entity;
        }
    }
}
