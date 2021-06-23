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
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomCategoryId = default;
            Guid inputCategoryId = randomCategoryId;

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
                broker.SelectCategoryByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
