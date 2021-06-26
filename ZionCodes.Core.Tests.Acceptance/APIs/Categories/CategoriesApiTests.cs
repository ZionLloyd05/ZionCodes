using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tynamix.ObjectFiller;
using Xunit;
using ZionCodes.Core.Tests.Acceptance.Brokers;
using ZionCodes.Core.Tests.Acceptance.Models.Categories;

namespace ZionCodes.Core.Tests.Acceptance.APIs.Categories
{
    [Collection(nameof(ApiTestCollection))]
    public partial class CategoriesApiTests
    {
        private readonly ApiBroker apiBroker;

        public CategoriesApiTests(ApiBroker categoryApiBroker) =>
            this.apiBroker = categoryApiBroker;

        private static Category CreateRandomCategory() =>
             CreateRandomCategoryFiller().Create();

        private async ValueTask<Category> PostRandomCategoryAsync()
        {
            Category randomCategory = CreateRandomCategory();
            await this.apiBroker.PostCategoryAsync(randomCategory);

            return randomCategory;
        }

        private static Category UpdateCategoryRandom(Category category)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Category>();

            filler.Setup()
                .OnProperty(category => category.Id).Use(category.Id)
                .OnProperty(category => category.CreatedBy).Use(category.CreatedBy)
                .OnProperty(category => category.UpdatedBy).Use(category.UpdatedBy)
                .OnProperty(category => category.CreatedDate).Use(category.CreatedDate)
                .OnProperty(category => category.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static Filler<Category> CreateRandomCategoryFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();

            var filler = new Filler<Category>();

            filler.Setup()
                .OnProperty(category => category.CreatedBy).Use(posterId)
                .OnProperty(category => category.UpdatedBy).Use(posterId)
                .OnProperty(category => category.CreatedDate).Use(now)
                .OnProperty(category => category.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
    }
}
