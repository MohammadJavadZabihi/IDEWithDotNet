using IDECore.DTOs;

namespace IDECore.Service.Interface
{
    public interface IUserManagerService
    {
        Task<bool> RegisterUser(UserRegisterDTO userRegisterDTO);
        Task<string> LoginUser();
        Task<bool> ConfrimEmail(string userId, string token);
        Task<string> LoginUser(LoginUserDTO loginUserDTO);
    }
}
