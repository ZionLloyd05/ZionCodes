using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Brokers.DateTimes;
using ZionCodes.Core.Brokers.Loggings;
using ZionCodes.Core.Brokers.Storages;
using ZionCodes.Core.Models.Comments;

namespace ZionCodes.Core.Services.Comments
{
    public partial class CommentService : ICommentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public CommentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker
            )
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Comment> AddCommentAsync(Comment comment) =>
        TryCatch(() =>
        {
            ValidateCommentOnCreate(comment);
            ValidateCommentIdIsNull(comment.Id);
            return this.storageBroker.InsertCommentAsync(comment);
        });

        public IQueryable<Comment> RetrieveAllComments()
        {
            IQueryable<Comment> storageComments = this.storageBroker.SelectAllComments();
            ValidateStorageComments(storageComments);
            return storageComments;
        }
    }
}
