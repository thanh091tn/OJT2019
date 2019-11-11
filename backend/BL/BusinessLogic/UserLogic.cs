using BL.Commons;
using BL.Helpers;
using BL.Interfaces;
using BO.Dtos;
using BO.Models;
using DAL.Repository;
using DAL.Repository.UnitOfWorks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BL
{


    public class UserLogic : IUserLogic
    {
       
        private readonly UserHelper _userHelper;
        
        private readonly IUnitOfWork _uow;
        private readonly AppSettings _appSettings;


        public UserLogic( IOptions<AppSettings> appSettings, UserHelper userHelper, IUnitOfWork uow)
        {
           
            _userHelper = userHelper;
          
            _uow = uow;
            _appSettings = appSettings.Value;
        }


        //business logic create new user
        public UserDetailDto CreateNewUser(UserCreateDto userCreateDto)
        {
            var newUser = new User
            {
                Id = new Guid(),
                Password = userCreateDto.Password,
                UserName = userCreateDto.UserName,
                Email = userCreateDto.Email
            };
            _uow.GetRepository<User>().Insert(newUser);

            var newFollow = new Follow
            {
                FollowerId = newUser.Id
            };
            _uow.GetRepository<Follow>().Insert(newFollow);
            _uow.SaveChange();
            return Login(userCreateDto.Email, userCreateDto.Password);

        }

        //business logic Login
        public UserDetailDto Login(string email, string password)
        {


            var User = _uow.GetRepository<User>().GetAll()
                .FirstOrDefault(user => user.Email == email && user.Password == password);

            if (User == null)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, User.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new UserDetailDto
            {
                UserName = User.UserName,
                Token = tokenString,
                Id = User.Id
            };
        }

        //business logic update existing user
        public bool UpdateUser(UserUpdateDto newuser)
        {
            var userId = _userHelper.GetUserId();
            var user = _uow.GetRepository<User>().GetAll()
                .First(user1 => user1.Id == userId);
            
            if (user != null)
            {
                user.About = newuser.About;
                user.Password = newuser.Password;
                user.Img = newuser.Img;
                user.Email = newuser.Email;
                user.UserName = newuser.UserName;

                
                _uow.GetRepository<User>().Update(user);

                _uow.SaveChange();
                 return true;
            }

            return false;

        }

        //business logic show user detail
        public UserDetailDto GetCurrentUser()
        {

            var id = _userHelper.GetUserId();
            if (id == null)
            {
                return null;
            }
            return _uow.GetRepository<User>().GetAll()
                .Select(c => new UserDetailDto()
                {
                    Id = c.Id,
                    About = c.About,
                    Email = c.Email,
                    Img = c.Img,
                    UserName = c.UserName
                }).Single(c => c.Id == id);

        }

        public UserDetailDto GetUser(string userName)
        {
            var id = _userHelper.GetUserId();
            if (id == null)
            {
                return null;
            }

            var rs = _uow.GetRepository<Follow>().GetAll()
                .First(c => c.User.UserName == userName);


            return _uow.GetRepository<User>().GetAll().Select(c => new UserDetailDto()
            {
                Id = c.Id,
                About = c.About,
                IsFollowed = _uow.GetRepository<UserFollow>().GetAll()
                                       .Any(follow => follow.UserId == id && follow.FollowId == rs.Id),
                Img = c.Img,
                UserName = c.UserName
            }).Single(c => c.UserName == userName);

        }
    }
}