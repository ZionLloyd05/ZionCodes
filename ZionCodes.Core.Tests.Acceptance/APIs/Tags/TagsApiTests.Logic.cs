using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using RESTFulSense.Exceptions;
using Xunit;
using ZionCodes.Core.Models.Tags;

namespace ZionCodes.Core.Tests.Acceptance.APIs.Tags
{
    public partial class TagsApiTests
    {
        [Fact]
        public async Task ShouldPostTagAsync()
        {
            // given
            Tag randomTag = CreateRandomTag();
            Tag inputTag = randomTag;
            Tag expectedTag = inputTag;

            // when 
            await this.apiBroker.PostTagAsync(inputTag);

            Tag actualTag =
                 await this.apiBroker.GetTagByIdAsync(inputTag.Id);

            // then
            actualTag.Should().BeEquivalentTo(expectedTag);
            await this.apiBroker.DeleteTagByIdAsync(actualTag.Id);
        }

        [Fact]
        public async Task ShouldPutTagAsync()
        {
            // given
            Tag randomTag = await PostRandomTagAsync();
            Tag modifiedTag = UpdateTagRandom(randomTag);

            // when
            await this.apiBroker.PutTagAsync(modifiedTag);

            Tag actualTag =
                await this.apiBroker.GetTagByIdAsync(randomTag.Id);

            // then
            actualTag.Should().BeEquivalentTo(modifiedTag);
            await this.apiBroker.DeleteTagByIdAsync(actualTag.Id);
        }

        [Fact]
        public async Task ShouldGetAllTagsAsync()
        {
            //given
            var randomTags = new List<Tag>();

            for (var i = 0; i <= GetRandomNumber(); i++)
            {
                randomTags.Add(await PostRandomTagAsync());
            }

            List<Tag> inputedTags = randomTags;
            List<Tag> expectedTags = inputedTags.ToList();

            //when 
            List<Tag> actualTags = await this.apiBroker.GetAllTagsAsync();

            //then
            foreach (var expectedcalendar in expectedTags)
            {
                Tag actualTag = actualTags.Single(calendar => calendar.Id == expectedcalendar.Id);

                actualTag.Should().BeEquivalentTo(expectedcalendar);
                await this.apiBroker.DeleteTagByIdAsync(actualTag.Id);
            }
        }

        [Fact]
        public async Task ShouldDeleteTagAsync()
        {
            //given
            Tag randomTag = await PostRandomTagAsync();
            Tag inputTag = randomTag;
            Tag expectedTag = inputTag;

            //when
            Tag deletedTag =
                await this.apiBroker.DeleteTagByIdAsync(inputTag.Id);

            ValueTask<Tag> getTagByIdTask =
                this.apiBroker.DeleteTagByIdAsync(inputTag.Id);

            // then
            deletedTag.Should().BeEquivalentTo(expectedTag);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getTagByIdTask.AsTask());
        }
    }
}
