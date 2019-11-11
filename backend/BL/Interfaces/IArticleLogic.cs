using BO.Dtos;
using System;
using System.Linq;

namespace BL.Interfaces
{
    public interface IArticleLogic
    {
        ShowArticleDto GetGlobalArticles(int currentPage,
            int pageRange);

        ShowArticleDto GetGlobalArticlesByTagId(Guid tagId,
            int currentPage, int pageRange);

        IQueryable<ArticleDto> GetArticleDetails(Guid articleId);

        ShowArticleDto GetArticleByListUserId(
            int currentPage, int pageRange);

        ShowArticleDto GetArticleByCurrentUserId(int currentPage,
            int pageRange);

        ShowArticleDto GetArticleByUserName(string userName, int currentPage, int pageRange);

        ShowArticleDto GetArticlebyLikeArticleId(string userName, int currentPage,
            int pageRange);

        Guid AddNewArticle(ArticleCreateNewDto articleCreateNewDto);
        void EditArticle(ArticleEditDto articleEditDto, Guid articleId);
        bool DeleteArticle(Guid articleId);
    }
}
