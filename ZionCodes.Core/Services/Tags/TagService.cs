using System;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Brokers.DateTimes;
using ZionCodes.Core.Brokers.Loggings;
using ZionCodes.Core.Brokers.Storages;
using ZionCodes.Core.Models.Tags;

namespace ZionCodes.Core.Services.Tags
{
    public partial class TagService : ITagService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public TagService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker
            )
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Tag> AddTagAsync(Tag tag) =>
            TryCatch(async () =>
            {
                ValidateTagOnCreate(tag);

                return await this.storageBroker.InsertTagAsync(tag);
            });

        public IQueryable<Tag> RetrieveAllTags() =>
            TryCatch(() =>
            {
                IQueryable<Tag> storageTags = this.storageBroker.SelectAllTags();

                ValidateStorageTags(storageTags);

                return storageTags;
            });

        public ValueTask<Tag> RetrieveTagByIdAsync(Guid tagId) =>
            TryCatch(async () =>
            {
                ValidateTagId(tagId);

                Tag storageTag =
                    await this.storageBroker.SelectTagByIdAsync(tagId);

                ValidateStorageTag(storageTag, tagId);

                return storageTag;
            });

        public ValueTask<Tag> ModifyTagAsync(Tag tag) =>
            TryCatch(async () =>
            {
                ValidateTagOnModify(tag);
                Tag maybeTag =
                    await this.storageBroker.SelectTagByIdAsync(tag.Id);

                ValidateTagOnModify(maybeTag);

                return await this.storageBroker.UpdateTagAsync(tag);
            });

        public ValueTask<Tag> RemoveTagByIdAsync(Guid tagId) =>
            TryCatch(async () =>
            {
                ValidateTagIdIsNull(tagId);

                Tag storageTag =
                await this.storageBroker.SelectTagByIdAsync(tagId);

                ValidateStorageTag(storageTag, tagId);

                return await this.storageBroker.DeleteTagAsync(storageTag);
            });
    }
}
