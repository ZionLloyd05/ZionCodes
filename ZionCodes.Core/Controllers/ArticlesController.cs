using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZionCodes.Core.Models.Articles;
using ZionCodes.Core.Services.Articles;

namespace ZionCodes.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : BaseController<Article>
    {
        private readonly IArticleService articleService;

        public ArticlesController(IArticleService articleService)
        {
            this.articleService = articleService;
        }


        [HttpPost]
        public ValueTask<ActionResult<Article>> PostArticleAsync(Article article) =>
        TryCatchArticleFunction(async () =>
        {
            Article persistedArticle =
                    await this.articleService.AddArticleAsync(article);

            return Ok(persistedArticle);
        });

        [HttpGet]
        public ActionResult<IQueryable<Article>> GetAllArticles() =>
        TryCatchArticleFunction(() =>
        {
            IQueryable storageArticle =
                    this.articleService.RetrieveAllArticles();

            return Ok(storageArticle);
        });

        [HttpGet("{articleId}")]
        public ValueTask<ActionResult<Article>> GetArticleAsync(Guid articleId) =>
        TryCatchArticleFunction(async () =>
        {
            Article storageArticle =
                   await this.articleService.RetrieveArticleByIdAsync(articleId);

            return Ok(storageArticle);
        });

        [HttpPut]
        public ValueTask<ActionResult<Article>> PutArticleAsync(Article article) =>
        TryCatchArticleFunction(async () =>
        {
            Article registeredArticle =
                    await this.articleService.ModifyArticleAsync(article);

            return Ok(registeredArticle);
        });


        [HttpDelete("{articleId}")]
        public ValueTask<ActionResult<Article>> DeleteArticleAsync(Guid articleId) =>
        TryCatchArticleFunction(async () =>
        {
            Article storageArticle =
                    await this.articleService.RemoveArticleByIdAsync(articleId);

            return Ok(storageArticle);
        });
    }
}
