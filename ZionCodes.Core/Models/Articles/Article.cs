using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ZionCodes.Core.Models.Articles.Enums;
using ZionCodes.Core.Models.Categories;
using ZionCodes.Core.Models.Comments;

namespace ZionCodes.Core.Models.Articles
{
    public class Article : IAuditable
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Like { get; set; }
        public int Heart { get; set; }
        public int CategoryId { get; set; }
        public ArticleStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }

        [JsonIgnore]
        public IEnumerable<Comment> Comments { get; set; }
    }
}
