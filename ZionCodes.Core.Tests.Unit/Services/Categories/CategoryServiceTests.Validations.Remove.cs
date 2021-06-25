using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Categories;
using ZionCodes.Core.Models.Categories.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomCategoryId = default;
            Guid inputCategoryId = randomCategoryId;

            var invalidCategoryInputException = new InvalidCategoryException(
                parameterName: nameof(Category.Id),
                parameterValue: inputCategoryId);

            var expectedCategoryValidationException =
                new CategoryValidationException(invalidCategoryInputException);

            // when
            ValueTask<Category> deleteCategoryTask =
               this.categoryService.RemoveCategoryByIdAsync(inputCategoryId);

            // then
            await Assert.ThrowsAsync<CategoryValidationException>(() => deleteCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCategoryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoryByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCategoryAsync(It.IsAny<Category>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenStorageCategoryIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Category randomCategory = CreateRandomCategory(dateTime);
            Guid inputCategoryId = randomCategory.Id;
            Category inputCategory = randomCategory;
            Category nullStorageCategory = null;

            var notFoundCategoryException = new NotFoundCategoryException(inputCategoryId);

            var expectedCategoryValidationException =
                new CategoryValidationException(notFoundCategoryException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCategoryByIdAsync(inputCategoryId))
                    .ReturnsAsync(nullStorageCategory);

            // when
            ValueTask<Category> actualCategoryTask =
                this.categoryService.RemoveCategoryByIdAsync(inputCategoryId);

            // then
            await Assert.ThrowsAsync<CategoryValidationException>(() => actualCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCategoryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoryByIdAsync(inputCategoryId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCategoryAsync(It.IsAny<Category>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
