using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Comments;

namespace ZionCodes.Core.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string CommentsRelativeUrl = "api/comments";

        public async ValueTask<Comment> PostCommentAsync(Comment tag) =>
            await this.apiFactoryClient.PostContentAsync(CommentsRelativeUrl, tag);

        public async ValueTask<Comment> GetCommentByIdAsync(Guid tagId) =>
            await this.apiFactoryClient.GetContentAsync<Comment>($"{CommentsRelativeUrl}/{tagId}");

        public async ValueTask<Comment> DeleteCommentByIdAsync(Guid tagId) =>
            await this.apiFactoryClient.DeleteContentAsync<Comment>($"{CommentsRelativeUrl}/{tagId}");

        public async ValueTask<Comment> PutCommentAsync(Comment tag) =>
            await this.apiFactoryClient.PutContentAsync(CommentsRelativeUrl, tag);

        public async ValueTask<List<Comment>> GetAllCommentsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Comment>>($"{CommentsRelativeUrl}/");
    }
}
