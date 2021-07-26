using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZionCodes.Core.Tests.Acceptance.Models.Articles
{
    public class Article
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
    }

    public enum ArticleStatus
    {
        Sketch,
        Draft,
        Published,
        Archived
    }
}
