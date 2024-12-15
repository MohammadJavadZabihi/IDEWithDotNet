using IDECore.DTOs;
using IDECore.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace IDECore.Service
{
    public class UserManagerService : IUserManagerService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly IAuthenticateService _authenticateService;
        public UserManagerService(UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            IEmailSender emailSender,
            SignInManager<IdentityUser> signInManager, IAuthenticateService authenticateService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _authenticateService = authenticateService;
        }

        public async Task<bool> ConfrimEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            return true;
        }

        public Task<string> LoginUser()
        {
            throw new NotImplementedException();
        }

        public async Task<string> LoginUser(LoginUserDTO loginUserDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginUserDTO.Email);

            if( user == null)
            {
                return null;
            }

            var resul = await _signInManager.
                PasswordSignInAsync(user, loginUserDTO.Password, loginUserDTO.RememberMe, false);

            if(resul.Succeeded)
            {
                var token = _authenticateService.AuthenticatJwtToken(user.Id, user.UserName, user.Email);

                return token;
            }

            return null;
        }

        public async Task<bool> RegisterUser(UserRegisterDTO userRegisterDTO)
        {
            var user = new IdentityUser
            {
                Email = userRegisterDTO.Email,
                UserName = userRegisterDTO.UserName,
            };

            var registerResult = await _userManager.CreateAsync(user, userRegisterDTO.Password);

            if (registerResult.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var domain = _configuration["AppSettings:Domain"];

                var confirmationLink = $"{domain}/api/Users/ConfrimEmail?/{user.Id}/{token}";

                var emailDto = new EmailDTO(userRegisterDTO.Email, "ActiveAccount", confirmationLink);

                await _emailSender.SendEmail(emailDto);

                return true;
            }
            else
            {
                foreach (var error in registerResult.Errors)
                {
                    throw new Exception(error.ToString());
                }
            }

            return false;
        }
    }
}
