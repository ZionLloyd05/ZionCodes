using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tynamix.ObjectFiller;
using Xunit;
using ZionCodes.Core.Models.Tags;
using ZionCodes.Core.Tests.Acceptance.Brokers;

namespace ZionCodes.Core.Tests.Acceptance.APIs.Tags
{
    [Collection(nameof(ApiTestCollection))]
    public partial class TagsApiTests
    {
        private readonly ApiBroker apiBroker;

        public TagsApiTests(ApiBroker tagApiBroker) =>
            this.apiBroker = tagApiBroker;

        private static Tag CreateRandomTag() =>
             CreateRandomTagFiller().Create();

        private async ValueTask<Tag> PostRandomTagAsync()
        {
            Tag randomTag = CreateRandomTag();
            await this.apiBroker.PostTagAsync(randomTag);

            return randomTag;
        }

        private static Tag UpdateTagRandom(Tag tag)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Tag>();

            filler.Setup()
                .OnProperty(tag => tag.Id).Use(tag.Id)
                .OnProperty(tag => tag.CreatedBy).Use(tag.CreatedBy)
                .OnProperty(tag => tag.UpdatedBy).Use(tag.UpdatedBy)
                .OnProperty(tag => tag.CreatedDate).Use(tag.CreatedDate)
                .OnProperty(tag => tag.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static Filler<Tag> CreateRandomTagFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();

            var filler = new Filler<Tag>();

            filler.Setup()
                .OnProperty(tag => tag.CreatedBy).Use(posterId)
                .OnProperty(tag => tag.UpdatedBy).Use(posterId)
                .OnProperty(tag => tag.CreatedDate).Use(now)
                .OnProperty(tag => tag.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
    }
}
