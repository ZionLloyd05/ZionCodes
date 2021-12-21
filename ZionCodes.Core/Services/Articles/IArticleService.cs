using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Articles;

namespace ZionCodes.Core.Services.Articles
{
    public interface IArticleService
    {
        ValueTask<Article> AddArticleAsync(Article article);
        ICollection<Article> RetrieveAllArticles();
        ValueTask<Article> RetrieveArticleByIdAsync(int articleId);
        ValueTask<Article> ModifyArticleAsync(Article article);
        ValueTask<Article> RemoveArticleByIdAsync(int articleId);
    }
}
