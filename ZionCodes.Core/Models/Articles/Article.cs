using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Articles.Enums;
using ZionCodes.Core.Models.Categories;

namespace ZionCodes.Core.Models.Articles
{
    public class Article : IAuditable
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Like { get; set; }
        public int Heart { get; set; }
        public Guid CategoryId { get; set; }
        public ArticleStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
  
        [JsonIgnore]
        public virtual Category Category { get; set; }
    }
}
