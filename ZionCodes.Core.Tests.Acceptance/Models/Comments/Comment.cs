using System;

namespace ZionCodes.Core.Tests.Acceptance.Models.Comments
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public int Upvote { get; set; }
        public int Downvote { get; set; }
        public Guid ParentCommentId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
