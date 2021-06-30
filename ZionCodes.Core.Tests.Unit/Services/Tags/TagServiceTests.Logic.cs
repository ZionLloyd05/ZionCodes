using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Tags;

namespace ZionCodes.Core.Tests.Unit.Services.Tags
{
    public partial class TagServiceTests
    {
        [Fact]
        public async Task ShouldAddTagAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            Tag randomTag = CreateRandomTag(dateTime);
            Tag inputTag = randomTag;
            inputTag.UpdatedBy = inputTag.CreatedBy;
            inputTag.UpdatedDate = inputTag.CreatedDate;
            Tag expectedTag = inputTag;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTagAsync(inputTag))
                    .ReturnsAsync(expectedTag);

            //when
            Tag actualTag =
                await this.tagService.AddTagAsync(inputTag);

            //then
            actualTag.Should().BeEquivalentTo(expectedTag);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTagAsync(inputTag),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetriveAllTags()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<Tag> randomTags =
                CreateRandomTags(randomDateTime);

            IQueryable<Tag> storageTags =
                randomTags;

            IQueryable<Tag> expectedTags =
                storageTags;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTags())
                    .Returns(storageTags);

            // when
            IQueryable<Tag> actualTags =
                this.tagService.RetrieveAllTags();

            // then
            actualTags.Should().BeEquivalentTo(expectedTags);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTags(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveTagByIdAsync()
        {
            // given
            Guid randomTagId = Guid.NewGuid();
            Guid inputTagId = randomTagId;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Tag randomTag = CreateRandomTag(randomDateTime);
            Tag storageTag = randomTag;
            Tag expectedTag = storageTag;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(inputTagId))
                    .ReturnsAsync(storageTag);

            // when
            Tag actualTag =
                await this.tagService.RetrieveTagByIdAsync(inputTagId);

            // then
            actualTag.Should().BeEquivalentTo(expectedTag);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(inputTagId),
                    Times.Once);


            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldModifyTagAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Tag randomTag = CreateRandomTag(randomInputDate);
            Tag inputTag = randomTag;
            Tag afterUpdateStorageTag = inputTag;
            Tag expectedTag = afterUpdateStorageTag;
            Tag beforeUpdateStorageTag = randomTag.DeepClone();
            inputTag.UpdatedDate = randomDate;
            Guid tagId = inputTag.Id;

            //this.dateTimeBrokerMock.Setup(broker =>
            //   broker.GetCurrentDateTime())
            //       .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(tagId))
                    .ReturnsAsync(beforeUpdateStorageTag);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateTagAsync(inputTag))
                    .ReturnsAsync(afterUpdateStorageTag);

            // when
            Tag actualTag =
                await this.tagService.ModifyTagAsync(inputTag);

            // then
            actualTag.Should().BeEquivalentTo(expectedTag);

            //this.dateTimeBrokerMock.Verify(broker =>
            //    broker.GetCurrentDateTime(),
            //        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(tagId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTagAsync(inputTag),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldDeleteTagByIdAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            Tag randomTag = CreateRandomTag(dateTime);
            Tag inputTag = randomTag;
            Guid inputTagId = inputTag.Id;
            inputTag.UpdatedBy = inputTag.CreatedBy;
            inputTag.UpdatedDate = inputTag.CreatedDate;
            Tag storageTag = inputTag;
            Tag expectedTag = inputTag;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTagByIdAsync(inputTagId))
                    .ReturnsAsync(inputTag);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteTagAsync(inputTag))
                    .ReturnsAsync(storageTag);

            // when
            Tag actualTag =
                await this.tagService.RemoveTagByIdAsync(inputTagId);

            // then
            actualTag.Should().BeEquivalentTo(expectedTag);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTagByIdAsync(inputTagId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTagAsync(inputTag),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
