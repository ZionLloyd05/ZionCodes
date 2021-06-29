using System;
using System.Threading.Tasks;
using FluentAssertions;
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

    }
}
