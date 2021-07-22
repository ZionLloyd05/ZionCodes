using System;
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

            var calculatedCreatedDate = actualTag.CreatedDate.AddHours(actualTag.CreatedDate.Offset.Hours);
            var calculatedUpdatedDate = actualTag.CreatedDate.AddHours(actualTag.UpdatedDate.Offset.Hours);

            actualTag.CreatedDate =
                DateTimeOffset.FromUnixTimeSeconds(calculatedCreatedDate.ToUnixTimeSeconds());
            actualTag.UpdatedDate =
                DateTimeOffset.FromUnixTimeSeconds(calculatedUpdatedDate.ToUnixTimeSeconds());

            // then
            actualTag.Id.Should().Be(expectedTag.Id);
            actualTag.Title.Should().Be(expectedTag.Title);
            actualTag.Description.Should().Be(expectedTag.Description);
            actualTag.CreatedBy.Should().Be(expectedTag.CreatedBy);
            actualTag.UpdatedBy.Should().Be(expectedTag.UpdatedBy);
            actualTag.CreatedDate.Date.Should().Be(expectedTag.CreatedDate.Date);
            actualTag.UpdatedDate.Date.Should().Be(expectedTag.UpdatedDate.Date);

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
            actualTag.Id.Should().Be(modifiedTag.Id);
            actualTag.Title.Should().Be(modifiedTag.Title);
            actualTag.Description.Should().Be(modifiedTag.Description);
            actualTag.CreatedBy.Should().Be(modifiedTag.CreatedBy);
            actualTag.UpdatedBy.Should().Be(modifiedTag.UpdatedBy);
            actualTag.CreatedDate.Date.Should().Be(modifiedTag.CreatedDate.Date);
            actualTag.UpdatedDate.Date.Should().Be(modifiedTag.UpdatedDate.Date);

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
            foreach (var expectedTag in expectedTags)
            {
                Tag actualTag = actualTags.Single(tag => tag.Id == expectedTag.Id);

                actualTag.Id.Should().Be(expectedTag.Id);
                actualTag.Title.Should().Be(expectedTag.Title);
                actualTag.Description.Should().Be(expectedTag.Description);
                actualTag.CreatedBy.Should().Be(expectedTag.CreatedBy);
                actualTag.UpdatedBy.Should().Be(expectedTag.UpdatedBy);
                actualTag.CreatedDate.Date.Should().Be(expectedTag.CreatedDate.Date);
                actualTag.UpdatedDate.Date.Should().Be(expectedTag.UpdatedDate.Date);

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
            deletedTag.Id.Should().Be(expectedTag.Id);
            deletedTag.Title.Should().Be(expectedTag.Title);
            deletedTag.Description.Should().Be(expectedTag.Description);
            deletedTag.CreatedBy.Should().Be(expectedTag.CreatedBy);
            deletedTag.UpdatedBy.Should().Be(expectedTag.UpdatedBy);
            deletedTag.CreatedDate.Date.Should().Be(expectedTag.CreatedDate.Date);
            deletedTag.UpdatedDate.Date.Should().Be(expectedTag.UpdatedDate.Date);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getTagByIdTask.AsTask());
        }
    }
}
