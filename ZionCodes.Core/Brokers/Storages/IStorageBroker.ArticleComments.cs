using System;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.ArticleComments;

namespace ZionCodes.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        public ValueTask<ArticleComment> InsertArticleCommentAsync(ArticleComment ArticleComment);
        public IQueryable<ArticleComment> SelectAllArticleComments();
        public ValueTask<ArticleComment> SelectArticleCommentByIdAsync(Guid ArticleCommentId);
        public ValueTask<ArticleComment> UpdateArticleCommentAsync(ArticleComment ArticleComment);
        public ValueTask<ArticleComment> DeleteArticleCommentAsync(ArticleComment ArticleComment);
    }
}
