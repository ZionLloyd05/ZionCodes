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
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomCategoryId = Guid.NewGuid();
            Guid inputCategoryId = randomCategoryId;
            SqlException sqlException = GetSqlException();

            var expectedCategoryDependencyException =
                new CategoryDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCategoryByIdAsync(inputCategoryId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Category> deleteCategoryTask =
                this.categoryService.RemoveCategoryByIdAsync(inputCategoryId);

            // then
            await Assert.ThrowsAsync<CategoryDependencyException>(() =>
                deleteCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCategoryDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoryByIdAsync(inputCategoryId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCategoryAsync(It.IsAny<Category>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomCategoryId = Guid.NewGuid();
            Guid inputCategoryId = randomCategoryId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedCategoryException =
                new LockedCategoryException(databaseUpdateConcurrencyException);

            var expectedStudentCategoryException =
                new CategoryDependencyException(lockedCategoryException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCategoryByIdAsync(inputCategoryId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Category> deleteStudentCategoryTask =
                this.categoryService.RemoveCategoryByIdAsync(inputCategoryId);

            // then
            await Assert.ThrowsAsync<CategoryDependencyException>(() => deleteStudentCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentCategoryException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoryByIdAsync(inputCategoryId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomCategoryId = Guid.NewGuid();
            Guid inputCategoryId = randomCategoryId;
            var exception = new Exception();

            var expectedStudentCategoryException =
                new CategoryServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCategoryByIdAsync(inputCategoryId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Category> deleteStudentCategoryTask =
                this.categoryService.RemoveCategoryByIdAsync(inputCategoryId);

            // then
            await Assert.ThrowsAsync<CategoryServiceException>(() =>
                deleteStudentCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentCategoryException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoryByIdAsync(inputCategoryId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
