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
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            // given
            int randomCategoryId = default;
            int inputCategoryId = randomCategoryId;

            var invalidCategoryInputException = new InvalidCategoryInputException(
                    parameterName: nameof(Category.Id),
                    parameterValue: inputCategoryId);

            var expectedCategoryValidationException =
                new CategoryValidationException(invalidCategoryInputException);

            // when
            ValueTask<Category> retrieveCategoryByIdTask =
                this.categoryService.RetrieveCategoryByIdAsync(inputCategoryId);

            // then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                retrieveCategoryByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCategoryValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoryByIdAsync(It.IsAny<int>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageCategoryIsNullAndLogItAsync()
        {
            // given
            int randomCategoryId = 1;
            int someCategoryId = randomCategoryId;
            Category invalidStorageCategory = null;
            var notFoundCategoryException = new NotFoundCategoryException(someCategoryId);

            var exceptionCategoryValidationException =
                new CategoryValidationException(notFoundCategoryException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCategoryByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(invalidStorageCategory);

            // when
            ValueTask<Category> retrieveCategoryByIdTask =
                this.categoryService.RetrieveCategoryByIdAsync(someCategoryId);

            // then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                retrieveCategoryByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(exceptionCategoryValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoryByIdAsync(It.IsAny<int>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
