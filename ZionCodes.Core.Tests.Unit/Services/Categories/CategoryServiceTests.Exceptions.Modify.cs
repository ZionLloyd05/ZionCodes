﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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
    }
}
