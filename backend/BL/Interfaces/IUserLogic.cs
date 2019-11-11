using BO.Dtos;

namespace BL.Interfaces
{
    public interface IUserLogic
    {
        UserDetailDto CreateNewUser(UserCreateDto userCreateDto);
        UserDetailDto Login(string email, string password);
        bool UpdateUser(UserUpdateDto user);
        UserDetailDto GetUser(string userName);
        UserDetailDto GetCurrentUser();
    }
}
