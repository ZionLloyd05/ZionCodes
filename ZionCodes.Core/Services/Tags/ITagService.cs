using System;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Categories;
using ZionCodes.Core.Models.Tags;

namespace ZionCodes.Core.Services.Tags
{
    public interface ITagServices
    {
        ValueTask<Tag> AddTagAsync(Tag tag);
    }
}
