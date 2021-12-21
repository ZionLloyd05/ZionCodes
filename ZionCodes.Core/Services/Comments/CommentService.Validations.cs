using System;
using System.Collections.Generic;
using System.Linq;
using ZionCodes.Core.Models.Comments;
using ZionCodes.Core.Models.Comments.Exceptions;

namespace ZionCodes.Core.Services.Comments
{
    public partial class CommentService
    {
        private void ValidateCommentOnCreate(Comment Comment)
        {
            ValidateCommentIsNull(Comment);
            ValidateCommentIdIsNull(Comment.Id);
            ValidateCommentArticleIdIsNull(Comment.ArticleId);
            ValidateCommentProperties(Comment);
            ValidateCommentAuditFieldsOnCreate(Comment);
        }

        private void ValidateCommentIsNull(Comment comment)
        {
            if (comment is null)
            {
                throw new NullCommentException();
            }
        }


        private void ValidateCommentId(int commentId)
        {
            if (commentId < 1)
            {
                throw new InvalidCommentInputException(
                    parameterName: nameof(Comment.Id),
                    parameterValue: commentId);
            }
        }

        private void ValidateCommentIdIsNull(int commentId)
        {
            if (commentId == default)
            {
                throw new InvalidCommentException(
                    parameterName: nameof(Comment.Id),
                    parameterValue: commentId);
            }
        }

        private void ValidateCommentArticleIdIsNull(int commentArticleId)
        {
            if (commentArticleId == default)
            {
                throw new InvalidCommentException(
                    parameterName: nameof(Comment.ArticleId),
                    parameterValue: commentArticleId);
            }
        }

        private void ValidateStorageComment(Comment storageComment, int commentId)
        {
            if (storageComment == null)
            {
                throw new NotFoundCommentException(commentId);
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

        private void ValidateStorageComments(ICollection<Comment> storageComments)
        {
            if (storageComments.Count() == 0)
            {
                this.loggingBroker.LogWarning("No comments found in storage.");
            }
        }


        private void ValidateCommentOnModify(Comment comment)
        {
            ValidateCommentIsNull(comment);
            ValidateCommentProperties(comment);
            ValidateCommentAuditFieldsOnModify(comment);
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

        private void ValidateCommentAuditFieldsOnModify(Comment comment)
        {
            switch (comment)
            {
                case { } when IsInvalid(input: comment.CreatedDate):
                    throw new InvalidCommentException(
                        parameterName: nameof(Comment.CreatedDate),
                        parameterValue: comment.CreatedDate);

                case { } when IsInvalid(input: comment.UpdatedDate):
                    throw new InvalidCommentException(
                        parameterName: nameof(Comment.UpdatedDate),
                        parameterValue: comment.UpdatedDate);
            }
        }

        private bool IsInvalid(DateTimeOffset input) => input == default;
        private bool IsInvalid(string commentBody) => String.IsNullOrWhiteSpace(commentBody);


        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }
    }
}
