using BL.Interfaces;
using BO.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    public class EditorsController : BaseController
    {
        private readonly IArticleLogic _articleLogic;

        public EditorsController(IArticleLogic articleLogic)
        {
            _articleLogic = articleLogic;
        }

        // PUT: api/Editors/5
        [HttpPut("{articleId}")]
        public IActionResult EditArticle([FromRoute] Guid articleId, [FromBody] ArticleEditDto articleEditDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            _articleLogic.EditArticle(articleEditDto, articleId);

            return NoContent();
        }

        // POST: api/Editors
        [HttpPost]
        public Guid AddNewArticle([FromBody] ArticleCreateNewDto articleCreateNewDto)
        {

            var result = _articleLogic.AddNewArticle(articleCreateNewDto);

            return result;
        }

        // DELETE: api/userFollows
        [HttpDelete]
        public IActionResult DeleteuserFollow(Guid articleId)
        {

            bool result = _articleLogic.DeleteArticle(articleId);
            if (result) return Ok();
            return NotFound();
        }




    }
}
