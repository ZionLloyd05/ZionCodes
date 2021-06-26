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
            actualCategory.Should().BeEquivalentTo(expectedCategory);
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
            actualCategory.Should().BeEquivalentTo(modifiedCategory);
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
            foreach (var expectedcalendar in expectedCategories)
            {
                Category actualCategory = actualCategories.Single(calendar => calendar.Id == expectedcalendar.Id);

                actualCategory.Should().BeEquivalentTo(expectedcalendar);
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
            deletedCategory.Should().BeEquivalentTo(expectedCategory);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getCategoryByIdTask.AsTask());
        }
    }
}
