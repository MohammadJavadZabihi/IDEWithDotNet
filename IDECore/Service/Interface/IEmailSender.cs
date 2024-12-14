using IDECore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDECore.Service.Interface
{
    public interface IEmailSender
    {
        Task SendEmail(EmailDTO emailDTO);
    }
}
