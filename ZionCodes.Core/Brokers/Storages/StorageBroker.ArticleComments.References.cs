using Microsoft.EntityFrameworkCore;
using ZionCodes.Core.Models.ArticleComments;

namespace ZionCodes.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void AddArticleCommentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleComment>()
                .HasKey(articleComment =>
                    new { articleComment.ArticleId, articleComment.CommentId });

            modelBuilder.Entity<ArticleComment>()
                .HasOne(articleComment => articleComment.Article)
                .WithMany(article => article.ArticleComments)
                .HasForeignKey(articleComment => articleComment.ArticleId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
