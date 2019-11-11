using BL.Interfaces;
using BO.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{

    public class ArticlesController : BaseController
    {
        private readonly IArticleLogic _articleLogic;

        public ArticlesController(IArticleLogic articleLogic)
        {
            _articleLogic = articleLogic;
        }

        // GET: api/Articles
        [HttpGet("Global")]
        [AllowAnonymous]
        public ShowArticleDto GetArticles(int currentPage)
        {
            int pageRange = 10;


            return _articleLogic.GetGlobalArticles(currentPage, pageRange);
        }

        // GET: api/Articles
        [AllowAnonymous]
        [HttpGet("Fav/{userName}")]
        public ShowArticleDto GetFavouriteArticles(string userName, int currentPage)
        {
            int pageRange = 10;


            return _articleLogic.GetArticlebyLikeArticleId(userName, currentPage, pageRange);
        }



        // GET: api/Articles
        [HttpGet("Tag")]
        [AllowAnonymous]
        public ShowArticleDto GetArticlesByTagId(Guid tagId, int currentPage)
        {
            int pageRange = 10;


            return _articleLogic.GetGlobalArticlesByTagId(tagId, currentPage, pageRange);
        }

        // GET: api/Articles
        [HttpGet("YourArticle")]
        public ShowArticleDto GetArticleByCurrentUserId(int currentPage)
        {
            int pageRange = 10;


            return _articleLogic.GetArticleByCurrentUserId(currentPage, pageRange);
        }

        // GET: api/Articles
        [AllowAnonymous]
        [HttpGet("UserArticles/{userName}")]
        public ShowArticleDto GetArticleByUserName(string userName, int currentPage)
        {
            int pageRange = 10;


            return _articleLogic.GetArticleByUserName(userName, currentPage, pageRange);
        }

        // GET: api/Articles/YourFeed
        [HttpGet("YourFeed")]
        public ShowArticleDto GetArticlesOfFollowers(int currentPage)
        {
            int pageRange = 10;

            return _articleLogic.GetArticleByListUserId(currentPage, pageRange);
        }

        // GET: api/Articles/5
        [HttpGet("{articleId}")]
        [AllowAnonymous]
        public IActionResult GetArticleDetails([FromRoute] Guid articleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var article = _articleLogic.GetArticleDetails(articleId);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }
    }
}