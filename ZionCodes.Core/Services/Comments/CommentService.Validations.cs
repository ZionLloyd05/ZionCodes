using System;
using ZionCodes.Core.Models.Comments;
using ZionCodes.Core.Models.Comments.Exceptions;

namespace ZionCodes.Core.Services.Comments
{
    public partial class CommentService
    {
        private void ValidateCommentOnCreate(Comment Comment)
        {
            ValidateCommentIsNull(Comment);
            ValidateCommentProperties(Comment);
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

        private void ValidateCommentProperties(Comment comment)
        {
            switch (comment)
            {
                case { } when IsInvalid(comment.Body):
                    throw new InvalidCommentException(
                        parameterName: nameof(Comment.Body),
                        parameterValue: comment.Body);
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
                case { } when comment.UpdatedDate != comment.CreatedDate:
                    throw new InvalidCommentException(
                        parameterName: nameof(comment.UpdatedDate),
                        parameterValue: comment.UpdatedDate);
                case { } when IsDateNotRecent(comment.CreatedDate):
                    throw new InvalidCommentException(
                        parameterName: nameof(Comment.CreatedDate),
                        parameterValue: comment.CreatedDate);
                default:
                    break;
            }
        }

        private bool IsInvalid(DateTimeOffset input) => input == default;
        private bool IsInvalid(string categoryTitle) => String.IsNullOrWhiteSpace(categoryTitle);


        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }
    }
}
