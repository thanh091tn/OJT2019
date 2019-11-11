using BL.Interfaces;
using BO.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    public class CommentsController : BaseController
    {
        private readonly ICommentLogic _commentLogic;

        public CommentsController(ICommentLogic commentLogic)
        {
            _commentLogic = commentLogic;
        }


        // GET: api/Comments/5
        [HttpGet("{articleId}")]
        [AllowAnonymous]
        public IActionResult GetComments([FromRoute] Guid articleId)
        {

            var comments = _commentLogic.GetComments(articleId);
            if (comments == null)
            {
                return NotFound();
            }

            return Ok(comments);
        }


        // POST: api/Comments
        [HttpPost]

        public IActionResult PostComment([FromBody] CommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool rs = _commentLogic.PostComment(commentDto);


            if (rs == false)
            {
                return NotFound();
            }
            return Ok();
        }

        // DELETE: api/Comments/5
        [HttpDelete("{commentId}")]
        public IActionResult DeleteComment([FromRoute] Guid commentId)
        {

            var result = _commentLogic.DeleteComment(commentId);
            if (result == false)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}