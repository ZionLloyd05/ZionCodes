﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Comments;

namespace ZionCodes.Core.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string CommentsRelativeUrl = "api/comments";

        public async ValueTask<Comment> PostCommentAsync(Comment comment) =>
            await this.apiFactoryClient.PostContentAsync(CommentsRelativeUrl, comment);

        public async ValueTask<Comment> GetCommentByIdAsync(Guid commentId) =>
            await this.apiFactoryClient.GetContentAsync<Comment>($"{CommentsRelativeUrl}/{commentId}");

        public async ValueTask<Comment> DeleteCommentByIdAsync(Guid commentId) =>
            await this.apiFactoryClient.DeleteContentAsync<Comment>($"{CommentsRelativeUrl}/{commentId}");

        public async ValueTask<Comment> PutCommentAsync(Comment comment) =>
            await this.apiFactoryClient.PutContentAsync(CommentsRelativeUrl, comment);

        public async ValueTask<List<Comment>> GetAllCommentsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Comment>>($"{CommentsRelativeUrl}/");
    }
}
