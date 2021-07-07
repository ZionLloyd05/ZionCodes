using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Comments;
using ZionCodes.Core.Models.Comments.Exceptions;

namespace ZionCodes.Core.Services.Comments
{
    public partial class CommentService
    {
        private void ValidateCommentOnCreate(Comment Comment)
        {
            ValidateCommentIsNull(Comment);
            ValidateCommentAuditFieldsOnCreate(Comment);
        }

        private void ValidateCommentIsNull(Comment Comment)
        {
            if (Comment is null)
            {
                throw new NullCommentException();
            }
        }

        private void ValidateCommentIdIsNull(Guid commentId)
        {
            if (commentId == default)
            {
                throw new InvalidCommentException(
                    parameterName: nameof(Comment.Id),
                    parameterValue: commentId);
            }
        }

        private void ValidateCommentAuditFieldsOnCreate(Comment comment)
        {
            switch (comment)
            {
                case { } when IsInvalid(input: comment.CreatedDate):
                    throw new InvalidCommentException(
                        parameterName: nameof(comment.CreatedDate),
                        parameterValue: comment.CreatedDate);
                case { } when IsInvalid(input: comment.UpdatedDate):
                    throw new InvalidCommentException(
                        parameterName: nameof(comment.UpdatedDate),
                        parameterValue: comment.UpdatedDate);
                default:
                    break;
            }
        }

        private bool IsInvalid(DateTimeOffset input) => input == default;
    }
}
