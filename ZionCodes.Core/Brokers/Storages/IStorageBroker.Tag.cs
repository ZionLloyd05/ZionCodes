using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Tags;

namespace ZionCodes.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        public ValueTask<Tag> InsertTagAsync(Tag tag);
        public IQueryable<Tag> SelectAllTags();
        public ValueTask<Tag> SelectTagByIdAsync(Guid tagId);
        public ValueTask<Tag> UpdateTagAsync(Tag tag);
        public ValueTask<Tag> DeleteTagAsync(Tag tag);
    }
}
