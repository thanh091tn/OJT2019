using BL.Interfaces;
using BO.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserLogic _userLogic;

        public UsersController(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        //Get user detail
        //GET api/Users/1
        [AllowAnonymous]
        [HttpGet("{userName}")]
        public IActionResult GetUser(string userName)
        {
            var user = _userLogic.GetUser(userName);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("currentdetail")]
        public IActionResult GetCurrentUser()
        {
            var user = _userLogic.GetCurrentUser();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        //Login
        // Post: api/Users/Login
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult GetUser([FromBody]UserLoginDto userlogindto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userLogic.Login(userlogindto.Email, userlogindto.Password);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        //Update user infomation
        // PUT: api/Users/5
        [HttpPut]
        public IActionResult PutUser([FromBody] UserUpdateDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _userLogic.UpdateUser(user);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }

        //Create new user
        // POST: api/Users
        [HttpPost]
        [AllowAnonymous]
        public IActionResult PostUser([FromBody] UserCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = _userLogic.CreateNewUser(createDto);
            return Ok(user);
        }
    }
}