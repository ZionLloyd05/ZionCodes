using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using RESTFulSense.Exceptions;
using Xunit;
using ZionCodes.Core.Models.Comments;

namespace ZionCodes.Core.Tests.Acceptance.APIs.Comments
{
    public partial class CommentsApiTests
    {
        [Fact]
        public async Task ShouldPostCommentAsync()
        {
            // given
            Comment randomComment = CreateRandomComment();
            Comment inputComment = randomComment;
            Comment expectedComment = inputComment;

            // when 
            await this.apiBroker.PostCommentAsync(inputComment);

            Comment actualComment =
                 await this.apiBroker.GetCommentByIdAsync(inputComment.Id);

            // then
            actualComment.Should().BeEquivalentTo(expectedComment);
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
            actualComment.Should().BeEquivalentTo(modifiedComment);
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
            foreach (var expectedcalendar in expectedComments)
            {
                Comment actualComment = actualComments.Single(calendar => calendar.Id == expectedcalendar.Id);

                actualComment.Should().BeEquivalentTo(expectedcalendar);
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
            deletedComment.Should().BeEquivalentTo(expectedComment);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getCommentByIdTask.AsTask());
        }
    }
}
