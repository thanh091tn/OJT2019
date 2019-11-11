using System;

namespace BO.Dtos
{
    public class UserDtos
    {
        public string UserName { get; set; }
    }
    public class UserCreateDto : UserDtos
    {

        public string Password { get; set; }
        public string Email { get; set; }
    }
    public class UserDetailDto : UserDtos
    {
        public string Token { get; set; }
        public Guid Id { get; set; }
        public string About { get; set; }
        public string Img { get; set; }
        public string Email { get; set; }
        public bool IsFollowed { get; set; }
    }
    public class UserLoginDto
    {

        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserUpdateDto : UserCreateDto
    {
        public string Img { get; set; }
        public string About { get; set; }
    }
}
