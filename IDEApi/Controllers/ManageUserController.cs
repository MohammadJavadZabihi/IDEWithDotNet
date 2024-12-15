using IDECore.DTOs;
using IDECore.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IDEApi.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class ManageUserController : ControllerBase
    {
        private readonly IUserManagerService _userManagerService;
        public ManageUserController(IUserManagerService userManagerService)
        {
            _userManagerService = userManagerService;
        }

        #region Register

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(UserRegisterDTO userRegisterDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registerResult = await _userManagerService.RegisterUser(userRegisterDTO);

            if (!registerResult)
                return BadRequest(registerResult);

            return Ok(registerResult);
        }

        #endregion

        #region Confrim Email

        [HttpPost("ConfrimEmail/{userId}/{token}")]
        public async Task<IActionResult> ConfrimEmial(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("Null Token or User Id Exeption");

            var emailConfrimStatuce = await _userManagerService.ConfrimEmail(userId, token);

            if(!emailConfrimStatuce)
                return BadRequest();

            return Ok();
        }

        #endregion

        #region Login

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDTO loginUserDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userManagerService.LoginUser(loginUserDTO);

            if(result != null)
                return Ok(result);

            return BadRequest();
        }

        #endregion
    }
}
