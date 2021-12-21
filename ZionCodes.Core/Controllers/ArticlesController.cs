using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZionCodes.Core.Dtos.Generic;
using ZionCodes.Core.Models.Articles;
using ZionCodes.Core.Services.Articles;
using ZionCodes.Core.Utils;

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
        public ActionResult<ICollection<Article>> GetAllArticles([FromQuery] PaginationQuery paginationQuery) =>
        TryCatchArticleFunction(() =>
        {
            ICollection<Article> storageArticle =
                    this.articleService.RetrieveAllArticles();

            if(paginationQuery != null)
            {
                PaginationFilter filter = new()
                {
                    PageNumber = paginationQuery.PageNumber,
                    PageSize = paginationQuery.PageSize,
                };
                                
                if (paginationQuery.PageNumber < 1 || paginationQuery.PageSize < 1)
                {
                    return Ok(new PagedResponse<ICollection<Article>>(storageArticle));
                }

                var paginationResponse = PaginationBuilder.CreatePaginatedResponse(filter, storageArticle);

                return Ok(paginationResponse);
            }        

            return Ok(storageArticle);
        });

        [HttpGet("{articleId}")]
        public ValueTask<ActionResult<Article>> GetArticleAsync(int articleId) =>
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
        public ValueTask<ActionResult<Article>> DeleteArticleAsync(int articleId) =>
        TryCatchArticleFunction(async () =>
        {
            Article storageArticle =
                    await this.articleService.RemoveArticleByIdAsync(articleId);

            return Ok(storageArticle);
        });
    }
}
