using System;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Tags;

namespace ZionCodes.Core.Services.Tags
{
    public interface ITagService
    {
        ValueTask<Tag> AddTagAsync(Tag tag);
        ValueTask<Tag> RetrieveTagByIdAsync(Guid tagId);
        IQueryable<Tag> RetrieveAllTags();
        ValueTask<Tag> ModifyTagAsync(Tag tag);
        ValueTask<Tag> RemoveTagByIdAsync(Guid tagId);
    }
}
