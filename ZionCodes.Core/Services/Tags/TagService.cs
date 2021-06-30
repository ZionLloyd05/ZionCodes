using System;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Brokers.DateTimes;
using ZionCodes.Core.Brokers.Loggings;
using ZionCodes.Core.Brokers.Storages;
using ZionCodes.Core.Models.Tags;

namespace ZionCodes.Core.Services.Tags
{
    public partial class TagService : ITagServices
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

        public IQueryable<Tag> RetrieveAllTags()
        {
            throw new NotImplementedException();
        }

        public ValueTask<Tag> RetrieveTagByIdAsync(Guid tagId) =>
            TryCatch(async () =>
            {
                ValidateTagId(tagId);

                Tag storageTag =
                    await this.storageBroker.SelectTagByIdAsync(tagId);

                ValidateStorageTag(storageTag, tagId);

                return storageTag;
            });
    }
}
