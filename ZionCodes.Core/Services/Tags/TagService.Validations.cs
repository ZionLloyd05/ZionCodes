using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Tags;
using ZionCodes.Core.Models.Tags.Exceptions;

namespace ZionCodes.Core.Services.Tags
{
    public partial class TagService
    {
        private void ValidateTagOnCreate(Tag tag)
        {
            ValidateTagIsNull(tag);
            ValidateTagIdIsNull(tag.Id);
        }

        private void ValidateTagIsNull(Tag tag)
        {
            if (tag is null)
            {
                throw new NullTagException();
            }
        }

        private void ValidateTagIdIsNull(Guid tagId)
        {
            if (tagId == default)
            {
                throw new InvalidTagException(
                    parameterName: nameof(Tag.Id),
                    parameterValue: tagId);
            }
        }
    }
}
