using System;
using System.Linq;
using ZionCodes.Core.Models.Articles;
using ZionCodes.Core.Models.Articles.Exceptions;
using ZionCodes.Core.Models.Categories.Exceptions;

namespace ZionCodes.Core.Services.Articles
{
    public partial class ArticleService
    {
        private void ValidateArticleOnCreate(Article Article)
        {
            ValidateArticleIsNull(Article);
            ValidateArticleIdIsNull(Article.Id);
            ValidateArticleProperties(Article);
            ValidateArticleAuditFieldsOnCreate(Article);
        }

        private void ValidateArticleIsNull(Article article)
        {
            if (article is null)
            {
                throw new NullArticleException();
            }
        }


        private void ValidateArticleId(Guid articleId)
        {
            if (articleId == Guid.Empty)
            {
                throw new InvalidArticleInputException(
                    parameterName: nameof(Article.Id),
                    parameterValue: articleId);
            }
        }

        private void ValidateArticleIdIsNull(Guid articleId)
        {
            if (articleId == default)
            {
                throw new InvalidArticleException(
                    parameterName: nameof(Article.Id),
                    parameterValue: articleId);
            }
        }

        private void ValidateStorageArticle(Article storageArticle, Guid articleId)
        {
            if (storageArticle == null)
            {
                throw new NotFoundArticleException(articleId);
            }
        }

        private void ValidateArticleProperties(Article article)
        {
            switch (article)
            {
                case { } when IsInvalid(article.Title):
                    throw new InvalidArticleException(
                        parameterName: nameof(Article.Title),
                        parameterValue: article.Title);
                case { } when IsInvalid(article.Content):
                    throw new InvalidArticleException(
                        parameterName: nameof(Article.Content),
                        parameterValue: article.Content);
            }
        }

        private void ValidateStorageArticles(IQueryable<Article> storageArticles)
        {
            if (storageArticles.Count() == 0)
            {
                this.loggingBroker.LogWarning("No articles found in storage.");
            }
        }


        private void ValidateArticleOnModify(Article article)
        {
            ValidateArticleIsNull(article);
            ValidateArticleProperties(article);
            ValidateArticleAuditFieldsOnModify(article);
        }


        private void ValidateArticleAuditFieldsOnCreate(Article article)
        {
            switch (article)
            {
                case { } when IsInvalid(input: article.CreatedBy):
                    throw new InvalidArticleException(
                        parameterName: nameof(article.CreatedBy),
                        parameterValue: article.CreatedBy);

                case { } when IsInvalid(input: article.UpdatedBy):
                    throw new InvalidArticleException(
                        parameterName: nameof(article.UpdatedBy),
                        parameterValue: article.UpdatedBy);

                case { } when IsInvalid(input: article.CreatedDate):
                    throw new InvalidArticleException(
                        parameterName: nameof(article.CreatedDate),
                        parameterValue: article.CreatedDate);
                case { } when IsInvalid(input: article.UpdatedDate):
                    throw new InvalidArticleException(
                        parameterName: nameof(article.UpdatedDate),
                        parameterValue: article.UpdatedDate);
                case { } when article.UpdatedDate != article.CreatedDate:
                    throw new InvalidArticleException(
                        parameterName: nameof(article.UpdatedDate),
                        parameterValue: article.UpdatedDate);
                case { } when IsDateNotRecent(article.CreatedDate):
                    throw new InvalidArticleException(
                        parameterName: nameof(Article.CreatedDate),
                        parameterValue: article.CreatedDate);
                default:
                    break;
            }
        }

        private void ValidateArticleAuditFieldsOnModify(Article article)
        {
            switch (article)
            {
                case { } when IsInvalid(input: article.CreatedBy):
                    throw new InvalidArticleException(
                        parameterName: nameof(Article.CreatedBy),
                        parameterValue: article.CreatedBy);

                case { } when IsInvalid(input: article.UpdatedBy):
                    throw new InvalidArticleException(
                        parameterName: nameof(Article.UpdatedBy),
                        parameterValue: article.UpdatedBy);

                case { } when IsInvalid(input: article.CreatedDate):
                    throw new InvalidArticleException(
                        parameterName: nameof(Article.CreatedDate),
                        parameterValue: article.CreatedDate);

                case { } when IsInvalid(input: article.UpdatedDate):
                    throw new InvalidArticleException(
                        parameterName: nameof(Article.UpdatedDate),
                        parameterValue: article.UpdatedDate);
            }
        }

        private bool IsInvalid(DateTimeOffset input) => input == default;
        private bool IsInvalid(string articleBody) => String.IsNullOrWhiteSpace(articleBody);
        private bool IsInvalid(Guid input) => input == default;

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }
    }
}
