using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDECore.Service.Interface
{
    public interface IAuthenticateService
    {
        string AuthenticatJwtToken(string userId, string userName, string email);
    }
}
