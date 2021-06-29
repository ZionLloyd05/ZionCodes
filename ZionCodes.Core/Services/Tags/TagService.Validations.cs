using System;
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

        private void ValidateTagId(Guid tagId)
        {
            if (tagId == Guid.Empty)
            {
                throw new InvalidTagInputException(
                    parameterName: nameof(Tag.Id),
                    parameterValue: tagId);
            }
        }

        private void ValidateStorageTag(Tag storageTag, Guid tagId)
        {
            if (storageTag == null)
            {
                throw new NotFoundTagException(tagId);
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

                case { } when IsInvalid(input: tag.CreatedDate):
                    throw new InvalidTagException(
                        parameterName: nameof(tag.CreatedDate),
                        parameterValue: tag.CreatedDate);

                case { } when IsInvalid(input: tag.UpdatedDate):
                    throw new InvalidTagException(
                        parameterName: nameof(tag.UpdatedDate),
                        parameterValue: tag.UpdatedDate);

                case { } when tag.UpdatedDate != tag.CreatedDate:
                    throw new InvalidTagException(
                        parameterName: nameof(tag.UpdatedDate),
                        parameterValue: tag.UpdatedDate);

                case { } when IsDateNotRecent(tag.CreatedDate):
                    throw new InvalidTagException(
                        parameterName: nameof(Tag.CreatedDate),
                        parameterValue: tag.CreatedDate);
            }
        }

        private bool IsInvalid(Guid input) => input == default;
        private bool IsInvalid(DateTimeOffset input) => input == default;

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }
    }
}
