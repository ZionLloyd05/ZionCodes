using System;
using System.Text.Json.Serialization;
using ZionCodes.Core.Tests.Acceptance.Models.Articles;

namespace ZionCodes.Core.Tests.Acceptance.Models.Comments
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public int Upvote { get; set; }
        public int Downvote { get; set; }
        public int ParentCommentId { get; set; }
        public int ArticleId { get; set; }
        [JsonIgnore]
        public Article Article { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
