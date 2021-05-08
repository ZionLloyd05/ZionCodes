﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ZionCodes.Core.Models.Categories;

namespace ZionCodes.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Category> Categories { get; set; }

        public async ValueTask<Category> InsertCategoryAsync(Category category)
        {
            EntityEntry<Category> categoryEntityEntry = await this.Categories.AddAsync(category);
            await this.SaveChangesAsync();

            return categoryEntityEntry.Entity;
        }

        public IQueryable<Category> SelectAllCategories() => this.Categories.AsQueryable();

        public async ValueTask<Category> SelectCategoryById(Guid categoryId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Categories.FindAsync(categoryId);
        }

        public async ValueTask<Category> UpdateCategory(Category category) 
        {
            EntityEntry<Category> categoryEntityEntry = this.Categories.Update(category);
            await this.SaveChangesAsync();

            return categoryEntityEntry.Entity;
        }

        public async ValueTask<Category> DeleteCategory(Category category)
        {
            EntityEntry<Category> categoryEntityEntry = this.Categories.Remove(category);
            await this.SaveChangesAsync();

            return categoryEntityEntry.Entity;
        }
    }
}