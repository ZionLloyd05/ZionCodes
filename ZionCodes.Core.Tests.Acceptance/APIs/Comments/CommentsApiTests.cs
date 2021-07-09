using System;
using System.Threading.Tasks;
using Tynamix.ObjectFiller;
using Xunit;
using ZionCodes.Core.Models.Comments;
using ZionCodes.Core.Tests.Acceptance.Brokers;

namespace ZionCodes.Core.Tests.Acceptance.APIs.Comments
{
    [Collection(nameof(ApiTestCollection))]
    public partial class CommentsApiTests
    {
        private readonly ApiBroker apiBroker;

        public CommentsApiTests(ApiBroker commentApiBroker) =>
            this.apiBroker = commentApiBroker;

        private static Comment CreateRandomComment() =>
             CreateRandomCommentFiller().Create();

        private async ValueTask<Comment> PostRandomCommentAsync()
        {
            Comment randomComment = CreateRandomComment();
            await this.apiBroker.PostCommentAsync(randomComment);

            return randomComment;
        }

        private static Comment UpdateCommentRandom(Comment comment)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Comment>();

            filler.Setup()
                .OnProperty(comment => comment.Id).Use(comment.Id)
                .OnProperty(comment => comment.CreatedDate).Use(comment.CreatedDate)
                .OnProperty(comment => comment.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static Filler<Comment> CreateRandomCommentFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();

            var filler = new Filler<Comment>();

            filler.Setup()
                .OnProperty(comment => comment.CreatedDate).Use(now)
                .OnProperty(comment => comment.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
    }
}
