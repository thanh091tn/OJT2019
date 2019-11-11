using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    public class UserFollowsController : BaseController
    {
        private readonly IFollowLogic _followLogic;

        public UserFollowsController(IFollowLogic followLogic)
        {
            _followLogic = followLogic;
        }

        // POST: api/userFollows
        [HttpPost]
        public IActionResult PostuserFollow(Guid newuserId)
        {

            var followId = _followLogic.getFollowId(newuserId);
            bool result = _followLogic.addFollower(followId);
            if (result) return Ok();
            return NotFound();

        }

        // DELETE: api/userFollows
        [HttpDelete]
        public IActionResult DeleteuserFollow(Guid newuserId)
        {
            var followId = _followLogic.getFollowId(newuserId);
            bool result = _followLogic.removeFollower(followId);
            if (result) return Ok();
            return NotFound();
        }


    }
}