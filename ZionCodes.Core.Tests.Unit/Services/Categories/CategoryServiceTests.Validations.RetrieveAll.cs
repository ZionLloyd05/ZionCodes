using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Categories;

namespace ZionCodes.Core.Tests.Unit.Services.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenCategoriesWasEmptyAndLogIt()
        {
            // given
            ICollection<Category> emptyStorageCategories = new List<Category>().ToList();
            ICollection<Category> expectedCategories = emptyStorageCategories;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCategories())
                    .Returns(expectedCategories);

            // when
            ICollection<Category> actualCategory =
                this.categoryService.RetrieveAllCategories();

            // then
            actualCategory.Should().BeEquivalentTo(expectedCategories);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No categories found in storage."));

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCategories(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
