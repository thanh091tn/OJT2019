using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    public class LikesController : BaseController
    {
        private readonly ILikeLogic _likeLogic;

        public LikesController(ILikeLogic likeLogic)
        {
            _likeLogic = likeLogic;
        }


        // POST: api/Likes?articleId=?
        [HttpPost]
        public IActionResult PostLike(Guid articleId)
        {

            var result = _likeLogic.Like(articleId);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        // DELETE: api/Likes?articleId=?
        [HttpDelete]
        public IActionResult DeleteLike(Guid articleId)
        {


            var result = _likeLogic.DisLike(articleId);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}