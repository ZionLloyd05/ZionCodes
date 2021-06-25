using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async void ShouldThrowValidationExceptionOnModifyWhenCategoryIsNullAndLogItAsync()
        {
            // given
            Category randomCategory = null;
            Category nullCategory = randomCategory;
            var nullCategoryException = new NullCategoryException();

            var expectedCategoryValidationException =
                new CategoryValidationException(nullCategoryException);

            // when
            ValueTask<Category> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(nullCategory);

            // then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                modifyCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCategoryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoryByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidFeeId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            Category randomCategory = CreateRandomCategory(dateTime);
            Category inputCategory = randomCategory;
            inputCategory.Id = default;

            var invalidCategoryInputException = new InvalidCategoryException(
                parameterName: nameof(Category.Id),
                parameterValue: inputCategory.Id);

            var expectedCategoryValidationException =
                new CategoryValidationException(invalidCategoryInputException);

            // when
            ValueTask<Category> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(inputCategory);

            // then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                modifyCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCategoryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoryByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
