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
        }

        private void ValidateTagIsNull(Tag tag)
        {
            if (tag is null)
            {
                throw new NullTagException();
            }
        }
    }
}
