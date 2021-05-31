using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Categories;
using ZionCodes.Core.Models.Categories.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCategoryIsNullAndLogItAsync()
        {
            // given
            Category randomCategory = null;
            Category nullCategory = randomCategory;
            var nullCategoryException = new NullCategoryException();

            var expectedCategoryValidationException =
                new CategoryValidationException(nullCategoryException);

            // when
            ValueTask<Category> createCategoryTask =
                this.categoryService.AddCategoryAsync(nullCategory);

            // then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                createCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCategoryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCategoryAsync(It.IsAny<Category>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCategoryIdIsInvalidAndLogItAsync()
        {
            //given
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
            ValueTask<Category> registerCategoryTask =
                this.categoryService.AddCategoryAsync(inputCategory);

            // then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                registerCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCategoryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCategoryAsync(It.IsAny<Category>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Category randomCategory = CreateRandomCategory(dateTime);
            Category inputCategory = randomCategory;
            inputCategory.CreatedBy = default;

            var invalidCategoryException = new InvalidCategoryException(
                parameterName: nameof(Category.CreatedBy),
                parameterValue: inputCategory.CreatedBy);

            var expectedCategoryValidationException =
                new CategoryValidationException(invalidCategoryException);

            // when
            ValueTask<Category> createCategoryTask =
                this.categoryService.AddCategoryAsync(inputCategory);

            // then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                createCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCategoryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCategoryAsync(It.IsAny<Category>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Category randomCategory = CreateRandomCategory(dateTime);
            Category inputCategory = randomCategory;
            inputCategory.UpdatedBy = default;

            var invalidCategoryException = new InvalidCategoryException(
                parameterName: nameof(Category.UpdatedBy),
                parameterValue: inputCategory.UpdatedBy);

            var expectedCategoryValidationException =
                new CategoryValidationException(invalidCategoryException);

            // when
            ValueTask<Category> createCategoryTask =
                this.categoryService.AddCategoryAsync(inputCategory);

            // then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                createCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCategoryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCategoryAsync(It.IsAny<Category>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Category randomCategory = CreateRandomCategory(dateTime);
            Category inputCategory = randomCategory;
            inputCategory.CreatedDate = default;

            var invalidCategoryException = new InvalidCategoryException(
                parameterName: nameof(Category.CreatedDate),
                parameterValue: inputCategory.CreatedDate);

            var expectedCategoryValidationException =
                new CategoryValidationException(invalidCategoryException);

            // when
            ValueTask<Category> createCategoryTask =
                this.categoryService.AddCategoryAsync(inputCategory);

            // then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                createCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCategoryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCategoryAsync(It.IsAny<Category>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Category randomCategory = CreateRandomCategory(dateTime);
            Category inputCategory = randomCategory;
            inputCategory.UpdatedDate = default;

            var invalidCategoryException = new InvalidCategoryException(
                parameterName: nameof(Category.UpdatedDate),
                parameterValue: inputCategory.UpdatedDate);

            var expectedCategoryValidationException =
                new CategoryValidationException(invalidCategoryException);

            // when
            ValueTask<Category> createCategoryTask =
                this.categoryService.AddCategoryAsync(inputCategory);

            // then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                createCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCategoryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCategoryAsync(It.IsAny<Category>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Category randomCategory = CreateRandomCategory(dateTime);
            Category inputCategory = randomCategory;
            inputCategory.UpdatedBy = randomCategory.CreatedBy;
            inputCategory.UpdatedDate = GetRandomDateTime();

            var invalidCategoryValidationException = new InvalidCategoryException(
                parameterName: nameof(Category.UpdatedDate),
                parameterValue: inputCategory.UpdatedDate);

            var expectedCategoryValidationException =
                new CategoryValidationException(invalidCategoryValidationException);

            // when
            ValueTask<Category> createCategoryTask =
                this.categoryService.AddCategoryAsync(inputCategory);

            // then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                createCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCategoryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCategoryAsync(It.IsAny<Category>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Category randomCategory = CreateRandomCategory(dateTime);
            Category inputCategory = randomCategory;
            inputCategory.UpdatedBy = randomCategory.CreatedBy;
            inputCategory.CreatedDate = dateTime.AddMinutes(minutes);
            inputCategory.UpdatedDate = inputCategory.CreatedDate;

            var invalidCategoryValidationException = new InvalidCategoryException(
                parameterName: nameof(Category.CreatedDate),
                parameterValue: inputCategory.CreatedDate);

            var expectedCategoryValidationException =
                new CategoryValidationException(invalidCategoryValidationException);

            // when 
            ValueTask<Category> createCategoryTask =
                this.categoryService.AddCategoryAsync(inputCategory);

            // then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                createCategoryTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCategoryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCategoryAsync(It.IsAny<Category>()),
                    Times.Never);


            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCategoryAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Category randomCategory = CreateRandomCategory(dateTime);
            Category alreadyExistsCategory = randomCategory;
            alreadyExistsCategory.UpdatedBy = alreadyExistsCategory.CreatedBy;

            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsCategoryException =
                new AlreadyExistsCategoryException(duplicateKeyException);

            var expectedCategoryValidationException =
                new CategoryValidationException(alreadyExistsCategoryException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCategoryAsync(alreadyExistsCategory))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Category> createCategoryTask =
                this.categoryService.AddCategoryAsync(alreadyExistsCategory);

            // then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                createCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCategoryValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCategoryAsync(alreadyExistsCategory),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
