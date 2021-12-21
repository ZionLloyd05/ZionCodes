using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ZionCodes.Core.Models.Categories;

namespace ZionCodes.Core.Brokers.Storages
{
    public partial class StorageBroker : IStorageBroker
    {
        public DbSet<Category> Categories { get; set; }

        public async ValueTask<Category> InsertCategoryAsync(Category category)
        {
            EntityEntry<Category> categoryEntityEntry = await this.Categories.AddAsync(category);
            await this.SaveChangesAsync();

            return categoryEntityEntry.Entity;
        }

        public ICollection<Category> SelectAllCategories() => this.Categories.ToList();

        public async ValueTask<Category> SelectCategoryByIdAsync(int categoryId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Categories.FindAsync(categoryId);
        }

        public async ValueTask<Category> UpdateCategoryAsync(Category category)
        {
            EntityEntry<Category> categoryEntityEntry = this.Categories.Update(category);
            await this.SaveChangesAsync();

            return categoryEntityEntry.Entity;
        }

        public async ValueTask<Category> DeleteCategoryAsync(Category category)
        {
            EntityEntry<Category> categoryEntityEntry = this.Categories.Remove(category);
            await this.SaveChangesAsync();

            return categoryEntityEntry.Entity;
        }

    }
}