using System;
using ZionCodes.Core.Models.Articles;
using ZionCodes.Core.Models.Comments;

namespace ZionCodes.Core.Models.ArticleComments
{
    public class ArticleComment
    {
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
        public Guid CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
