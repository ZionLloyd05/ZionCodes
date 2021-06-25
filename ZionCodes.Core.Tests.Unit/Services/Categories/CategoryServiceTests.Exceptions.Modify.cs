using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using ZionCodes.Core.Models.Categories;
using ZionCodes.Core.Models.Categories.Exceptions;

namespace ZionCodes.Core.Tests.Unit.Services.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Category randomCategory = CreateRandomCategory(randomDateTime);
            Category someCategory = randomCategory;
            someCategory.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

            var expectedCategoryDependencyException =
                new CategoryDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCategoryByIdAsync(someCategory.Id))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Category> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(someCategory);

            // then
            await Assert.ThrowsAsync<CategoryDependencyException>(() =>
                modifyCategoryTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoryByIdAsync(someCategory.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCategoryDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Category someCategory = CreateRandomCategory(randomDateTime);
            someCategory.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var expectedCategoryDependencyException =
                new CategoryDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCategoryByIdAsync(someCategory.Id))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Category> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(someCategory);

            // then
            await Assert.ThrowsAsync<CategoryDependencyException>(() =>
                modifyCategoryTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoryByIdAsync(someCategory.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCategoryDependencyException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Category randomCategory = CreateRandomCategory(randomDateTime);
            Category someCategory = randomCategory;
            someCategory.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedCategoryException = new LockedCategoryException(databaseUpdateConcurrencyException);

            var expectedCategoryDependencyException =
                new CategoryDependencyException(lockedCategoryException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCategoryByIdAsync(someCategory.Id))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Category> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(someCategory);

            // then
            await Assert.ThrowsAsync<CategoryDependencyException>(() =>
                modifyCategoryTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoryByIdAsync(someCategory.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCategoryDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Category randomCategory = CreateRandomCategory(randomDateTime);
            Category someCategory = randomCategory;
            someCategory.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var serviceException = new Exception();

            var expectedCategoryServiceException =
                new CategoryServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCategoryByIdAsync(someCategory.Id))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Category> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(someCategory);

            // then
            await Assert.ThrowsAsync<CategoryServiceException>(() =>
                modifyCategoryTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoryByIdAsync(someCategory.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCategoryServiceException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
