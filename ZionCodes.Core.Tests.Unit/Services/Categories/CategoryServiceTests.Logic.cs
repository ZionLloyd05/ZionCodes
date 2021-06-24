using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Categories;

namespace ZionCodes.Core.Tests.Unit.Services.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public async Task ShouldAddCategoryAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            Category randomCategory = CreateRandomCategory(dateTime);
            Category inputCategory = randomCategory;
            inputCategory.UpdatedBy = inputCategory.CreatedBy;
            inputCategory.UpdatedDate = inputCategory.CreatedDate;
            Category expectedCategory = inputCategory;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCategoryAsync(inputCategory))
                    .ReturnsAsync(expectedCategory);

            //when
            Category actualCategory =
                await this.categoryService.AddCategoryAsync(inputCategory);

            //then
            actualCategory.Should().BeEquivalentTo(expectedCategory);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCategoryAsync(inputCategory),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetriveAllCategories()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<Category> randomCategories =
                CreateRandomCategories(randomDateTime);

            IQueryable<Category> storageCategories =
                randomCategories;

            IQueryable<Category> expectedCategories =
                storageCategories;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCategories())
                    .Returns(storageCategories);

            // when
            IQueryable<Category> actualCategories =
                this.categoryService.RetrieveAllCategories();

            // then
            actualCategories.Should().BeEquivalentTo(expectedCategories);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCategories(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveCategoryByIdAsync()
        {
            // given
            Guid randomCategoryId = Guid.NewGuid();
            Guid inputCategoryId = randomCategoryId;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Category randomCategory = CreateRandomCategory(randomDateTime);
            Category storageCategory = randomCategory;
            Category expectedCategory = storageCategory;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCategoryByIdAsync(inputCategoryId))
                    .ReturnsAsync(storageCategory);

            // when
            Category actualCategory =
                await this.categoryService.RetrieveCategoryByIdAsync(inputCategoryId);

            // then
            actualCategory.Should().BeEquivalentTo(expectedCategory);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoryByIdAsync(inputCategoryId),
                    Times.Once);


            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
