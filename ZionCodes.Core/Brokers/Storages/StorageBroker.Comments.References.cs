using Microsoft.EntityFrameworkCore;
using ZionCodes.Core.Models.Comments;

namespace ZionCodes.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void AddCommentArticleReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasOne(comment => comment.Article)
                .WithMany(article => article.Comments)
                .HasForeignKey(comment => comment.ArticleId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
