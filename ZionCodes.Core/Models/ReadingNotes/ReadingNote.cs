using System;

namespace ZionCodes.Core.Models.ReadingNotes
{
    public class ReadingNote : IAuditable
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public string Note { get; set; }
        public int Like { get; set; }
        public int Heart { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
