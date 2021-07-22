using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using RESTFulSense.Exceptions;
using Xunit;
using ZionCodes.Core.Tests.Acceptance.Models.Categories;

namespace ZionCodes.Core.Tests.Acceptance.APIs.Categories
{
    public partial class CategoriesApiTests
    {
        [Fact]
        public async Task ShouldPostCategoryAsync()
        {
            // given
            Category randomCategory = CreateRandomCategory();
            Category inputCategory = randomCategory;
            Category expectedCategory = inputCategory;

            // when 
            await this.apiBroker.PostCategoryAsync(inputCategory);

            Category actualCategory =
                 await this.apiBroker.GetCategoryByIdAsync(inputCategory.Id);

            // then
            actualCategory.Id.Should().Be(expectedCategory.Id);
            actualCategory.Title.Should().Be(expectedCategory.Title);
            actualCategory.Description.Should().Be(expectedCategory.Description);
            actualCategory.CreatedBy.Should().Be(expectedCategory.CreatedBy);
            actualCategory.UpdatedBy.Should().Be(expectedCategory.UpdatedBy);
            actualCategory.CreatedDate.Date.Should().Be(expectedCategory.CreatedDate.Date);
            actualCategory.UpdatedDate.Date.Should().Be(expectedCategory.UpdatedDate.Date);

            await this.apiBroker.DeleteCategoryByIdAsync(actualCategory.Id);
        }

        [Fact]
        public async Task ShouldPutCategoryAsync()
        {
            // given
            Category randomCategory = await PostRandomCategoryAsync();
            Category modifiedCategory = UpdateCategoryRandom(randomCategory);

            // when
            await this.apiBroker.PutCategoryAsync(modifiedCategory);

            Category actualCategory =
                await this.apiBroker.GetCategoryByIdAsync(randomCategory.Id);

            // then
            actualCategory.Id.Should().Be(modifiedCategory.Id);
            actualCategory.Title.Should().Be(modifiedCategory.Title);
            actualCategory.Description.Should().Be(modifiedCategory.Description);
            actualCategory.CreatedBy.Should().Be(modifiedCategory.CreatedBy);
            actualCategory.UpdatedBy.Should().Be(modifiedCategory.UpdatedBy);
            actualCategory.CreatedDate.Date.Should().Be(modifiedCategory.CreatedDate.Date);
            actualCategory.UpdatedDate.Date.Should().Be(modifiedCategory.UpdatedDate.Date);

            await this.apiBroker.DeleteCategoryByIdAsync(actualCategory.Id);
        }

        [Fact]
        public async Task ShouldGetAllCategoriesAsync()
        {
            //given
            var randomCategories = new List<Category>();

            for (var i = 0; i <= GetRandomNumber(); i++)
            {
                randomCategories.Add(await PostRandomCategoryAsync());
            }

            List<Category> inputedCategories = randomCategories;
            List<Category> expectedCategories = inputedCategories.ToList();

            //when 
            List<Category> actualCategories = await this.apiBroker.GetAllCategoriesAsync();

            //then
            foreach (var expectedCategory in expectedCategories)
            {
                Category actualCategory = actualCategories.Single(Category => Category.Id == expectedCategory.Id);

                actualCategory.Id.Should().Be(expectedCategory.Id);
                actualCategory.Title.Should().Be(expectedCategory.Title);
                actualCategory.Description.Should().Be(expectedCategory.Description);
                actualCategory.CreatedBy.Should().Be(expectedCategory.CreatedBy);
                actualCategory.UpdatedBy.Should().Be(expectedCategory.UpdatedBy);
                actualCategory.CreatedDate.Date.Should().Be(expectedCategory.CreatedDate.Date);
                actualCategory.UpdatedDate.Date.Should().Be(expectedCategory.UpdatedDate.Date);

                await this.apiBroker.DeleteCategoryByIdAsync(actualCategory.Id);
            }
        }

        [Fact]
        public async Task ShouldDeleteCategoryAsync()
        {
            //given
            Category randomCategory = await PostRandomCategoryAsync();
            Category inputCategory = randomCategory;
            Category expectedCategory = inputCategory;

            //when
            Category deletedCategory =
                await this.apiBroker.DeleteCategoryByIdAsync(inputCategory.Id);

            ValueTask<Category> getCategoryByIdTask =
                this.apiBroker.DeleteCategoryByIdAsync(inputCategory.Id);

            // then
            deletedCategory.Id.Should().Be(expectedCategory.Id);
            deletedCategory.Title.Should().Be(expectedCategory.Title);
            deletedCategory.Description.Should().Be(expectedCategory.Description);
            deletedCategory.CreatedBy.Should().Be(expectedCategory.CreatedBy);
            deletedCategory.UpdatedBy.Should().Be(expectedCategory.UpdatedBy);
            deletedCategory.CreatedDate.Date.Should().Be(expectedCategory.CreatedDate.Date);
            deletedCategory.UpdatedDate.Date.Should().Be(expectedCategory.UpdatedDate.Date);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getCategoryByIdTask.AsTask());
        }
    }
}
