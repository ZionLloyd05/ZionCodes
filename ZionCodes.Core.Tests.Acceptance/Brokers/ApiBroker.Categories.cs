using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZionCodes.Core.Tests.Acceptance.Models.Categories;

namespace ZionCodes.Core.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string CategorysRelativeUrl = "api/categories";

        public async ValueTask<Category> PostCategoryAsync(Category category) =>
            await this.apiFactoryClient.PostContentAsync(CategorysRelativeUrl, category);

        public async ValueTask<Category> GetCategoryByIdAsync(Guid categoryId) =>
            await this.apiFactoryClient.GetContentAsync<Category>($"{CategorysRelativeUrl}/{categoryId}");

        public async ValueTask<Category> DeleteCategoryByIdAsync(Guid categoryId) =>
            await this.apiFactoryClient.DeleteContentAsync<Category>($"{CategorysRelativeUrl}/{categoryId}");

        public async ValueTask<Category> PutCategoryAsync(Category category) =>
            await this.apiFactoryClient.PutContentAsync(CategorysRelativeUrl, category);

        public async ValueTask<List<Category>> GetAllCategoriesAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Category>>($"{CategorysRelativeUrl}/");
    }
}
