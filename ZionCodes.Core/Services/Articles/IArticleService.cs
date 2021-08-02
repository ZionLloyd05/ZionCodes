using System;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Articles;

namespace ZionCodes.Core.Services.Articles
{
    public interface IArticleService
    {
        ValueTask<Article> AddArticleAsync(Article article);
        IQueryable<Article> RetrieveAllArticles();
        ValueTask<Article> RetrieveArticleByIdAsync(Guid articleId);
        ValueTask<Article> ModifyArticleAsync(Article article);
        ValueTask<Article> RemoveArticleByIdAsync(Guid articleId);
    }
}
