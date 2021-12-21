using Microsoft.EntityFrameworkCore;
using ZionCodes.Core.Models.Articles;
using ZionCodes.Core.Models.Categories;
using ZionCodes.Core.Models.Comments;
using ZionCodes.Core.Models.ReadingNotes;
using ZionCodes.Core.Models.Tags;

namespace ZionCodes.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void AddArticleConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                .Property(article => article.Id)
                .ValueGeneratedOnAdd();
        }

        private static void AddCategoryConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(category => category.Id)
                .ValueGeneratedOnAdd();
        }

        private static void AddTagConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>()
                .Property(tag => tag.Id)
                .ValueGeneratedOnAdd();
        }

        private static void AddReadingNoteConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReadingNote>()
                .Property(readingNote => readingNote.Id)
                .ValueGeneratedOnAdd();
        }

        private static void AddCommentArticleReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .Property(comment => comment.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Comment>()
                .HasOne(comment => comment.Article)
                .WithMany(article => article.Comments)
                .HasForeignKey(comment => comment.ArticleId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
