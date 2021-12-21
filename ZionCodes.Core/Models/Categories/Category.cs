using System;
using System.ComponentModel.DataAnnotations;

namespace ZionCodes.Core.Models.Categories
{
    public class Category : IAuditable
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}
