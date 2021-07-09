using System;
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

        public IQueryable<Comment> RetrieveAllComments() =>
            TryCatch(() =>
            {
                IQueryable<Comment> storageComments = this.storageBroker.SelectAllComments();
            
                ValidateStorageComments(storageComments);
            
                return storageComments;
            });

        public ValueTask<Comment> RetrieveCommentByIdAsync(Guid commentId) =>
            TryCatch(async () =>
            {
                ValidateCommentId(commentId);
                
                Comment storageComment = 
                    await this.storageBroker.SelectCommentByIdAsync(commentId);

                ValidateStorageComment(storageComment, commentId);

                return storageComment;
            });

        public ValueTask<Comment> ModifyCommentAsync(Comment comment) =>
            TryCatch(async () =>
            {
                ValidateCommentOnModify(comment);
                ValidateCommentIdIsNull(comment.Id);
                Comment maybeComment =
                    await this.storageBroker.SelectCommentByIdAsync(comment.Id);

                return await this.storageBroker.UpdateCommentAsync(comment);
            });

        public async ValueTask<Comment> RemoveCommentByIdAsync(Guid commentId)
        {
            Comment maybeComment =
                await this.storageBroker.SelectCommentByIdAsync(commentId);

            return await this.storageBroker.DeleteCommentAsync(maybeComment);
        }
    }
}
