using System;

namespace ZionCodes.Core.Tests.Acceptance.Models.Articles
{
    public class Article
    {
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
    }

    public enum ArticleStatus
    {
        Sketch,
        Draft,
        Published,
        Archived
    }
}
