using IDECore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDECore.Service.Interface
{
    public interface IUserManagerService
    {
        Task<bool> RegisterUser(UserRegisterDTO userRegisterDTO);
        Task<string> LoginUser();
        Task<bool> ConfrimEmail(string userId, string token);
    }
}
