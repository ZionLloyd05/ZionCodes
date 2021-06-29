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
            ValidateTagAuditFieldsOnCreate(tag);
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
        private void ValidateTagAuditFieldsOnCreate(Tag tag)
        {
            switch (tag)
            {
                case { } when IsInvalid(input: tag.CreatedBy):
                    throw new InvalidTagException(
                        parameterName: nameof(tag.CreatedBy),
                        parameterValue: tag.CreatedBy);

                case { } when IsInvalid(input: tag.UpdatedBy):
                    throw new InvalidTagException(
                        parameterName: nameof(tag.UpdatedBy),
                        parameterValue: tag.UpdatedBy);
            }
        }

        private bool IsInvalid(Guid input) => input == default;
    }
}
