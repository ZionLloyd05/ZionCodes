using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using RESTFulSense.Exceptions;
using Xunit;
using ZionCodes.Core.Models.Comments;
using ZionCodes.Core.Tests.Acceptance.Models.Articles;

namespace ZionCodes.Core.Tests.Acceptance.APIs.Comments
{
    public partial class CommentsApiTests
    {
        [Fact]
        public async Task ShouldPostCommentAsync()
        {
            // given
            Article randomArticle = await PostRandomArticleAsync();
            Comment randomComment = CreateRandomComment(randomArticle.Id);
            Comment inputComment = randomComment;
            Comment expectedComment = inputComment;

            // when 
            await this.apiBroker.PostCommentAsync(inputComment);

            Comment actualComment =
                 await this.apiBroker.GetCommentByIdAsync(inputComment.Id);

            // then
            actualComment.Id.Should().Be(expectedComment.Id);
            actualComment.Body.Should().Be(expectedComment.Body);
            actualComment.Upvote.Should().Be(expectedComment.Upvote);
            actualComment.Downvote.Should().Be(expectedComment.Downvote);
            actualComment.ParentCommentId.Should().Be(expectedComment.ParentCommentId);
            actualComment.CreatedDate.Date.Should().Be(expectedComment.CreatedDate.Date);
            actualComment.UpdatedDate.Date.Should().Be(expectedComment.UpdatedDate.Date);

            await this.apiBroker.DeleteCommentByIdAsync(actualComment.Id);
        }

        [Fact]
        public async Task ShouldPutCommentAsync()
        {
            // given
            Comment randomComment = await PostRandomCommentAsync();
            Comment modifiedComment = UpdateCommentRandom(randomComment);

            // when
            await this.apiBroker.PutCommentAsync(modifiedComment);

            Comment actualComment =
                await this.apiBroker.GetCommentByIdAsync(randomComment.Id);

            // then
            actualComment.Id.Should().Be(modifiedComment.Id);
            actualComment.Body.Should().Be(modifiedComment.Body);
            actualComment.Upvote.Should().Be(modifiedComment.Upvote);
            actualComment.Downvote.Should().Be(modifiedComment.Downvote);
            actualComment.ParentCommentId.Should().Be(modifiedComment.ParentCommentId);
            actualComment.CreatedDate.Date.Should().Be(modifiedComment.CreatedDate.Date);
            actualComment.UpdatedDate.Date.Should().Be(modifiedComment.UpdatedDate.Date);

            await this.apiBroker.DeleteCommentByIdAsync(actualComment.Id);
        }

        [Fact]
        public async Task ShouldGetAllCommentsAsync()
        {
            //given
            var randomComments = new List<Comment>();

            for (var i = 0; i <= GetRandomNumber(); i++)
            {
                randomComments.Add(await PostRandomCommentAsync());
            }

            List<Comment> inputedComments = randomComments;
            List<Comment> expectedComments = inputedComments.ToList();

            //when 
            List<Comment> actualComments = await this.apiBroker.GetAllCommentsAsync();

            //then
            foreach (var expectedComment in expectedComments)
            {
                Comment actualComment = actualComments.Single(Comment => Comment.Id == expectedComment.Id);

                actualComment.Id.Should().Be(expectedComment.Id);
                actualComment.Body.Should().Be(expectedComment.Body);
                actualComment.Upvote.Should().Be(expectedComment.Upvote);
                actualComment.Downvote.Should().Be(expectedComment.Downvote);
                actualComment.ParentCommentId.Should().Be(expectedComment.ParentCommentId);
                actualComment.CreatedDate.Date.Should().Be(expectedComment.CreatedDate.Date);
                actualComment.UpdatedDate.Date.Should().Be(expectedComment.UpdatedDate.Date);

                await this.apiBroker.DeleteCommentByIdAsync(actualComment.Id);
            }
        }

        [Fact]
        public async Task ShouldDeleteCommentAsync()
        {
            //given
            Comment randomComment = await PostRandomCommentAsync();
            Comment inputComment = randomComment;
            Comment expectedComment = inputComment;

            //when
            Comment deletedComment =
                await this.apiBroker.DeleteCommentByIdAsync(inputComment.Id);

            ValueTask<Comment> getCommentByIdTask =
                this.apiBroker.DeleteCommentByIdAsync(inputComment.Id);

            // then
            deletedComment.Id.Should().Be(expectedComment.Id);
            deletedComment.Body.Should().Be(expectedComment.Body);
            deletedComment.Upvote.Should().Be(expectedComment.Upvote);
            deletedComment.Downvote.Should().Be(expectedComment.Downvote);
            deletedComment.ParentCommentId.Should().Be(expectedComment.ParentCommentId);
            deletedComment.CreatedDate.Date.Should().Be(expectedComment.CreatedDate.Date);
            deletedComment.UpdatedDate.Date.Should().Be(expectedComment.UpdatedDate.Date);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getCommentByIdTask.AsTask());
        }
    }
}
