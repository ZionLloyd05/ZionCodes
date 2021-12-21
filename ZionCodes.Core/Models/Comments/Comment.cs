using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ZionCodes.Core.Models.Articles;

namespace ZionCodes.Core.Models.Comments
{
    public class Comment
    {
        [Key]
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
