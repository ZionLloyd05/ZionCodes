using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Tags;

namespace ZionCodes.Core.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string TagsRelativeUrl = "api/tags";

        public async ValueTask<Tag> PostTagAsync(Tag tag) =>
            await this.apiFactoryClient.PostContentAsync(TagsRelativeUrl, tag);

        public async ValueTask<Tag> GetTagByIdAsync(Guid tagId) =>
            await this.apiFactoryClient.GetContentAsync<Tag>($"{TagsRelativeUrl}/{tagId}");

        public async ValueTask<Tag> DeleteTagByIdAsync(Guid tagId) =>
            await this.apiFactoryClient.DeleteContentAsync<Tag>($"{TagsRelativeUrl}/{tagId}");

        public async ValueTask<Tag> PutTagAsync(Tag tag) =>
            await this.apiFactoryClient.PutContentAsync(TagsRelativeUrl, tag);

        public async ValueTask<List<Tag>> GetAllTagsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Tag>>($"{TagsRelativeUrl}/");
    }
}
