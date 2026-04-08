using BusinessLayer.Exceptions;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs;
using ModelLayer.Utility;

namespace FunDooDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly IUserBL _userBL;

        public UserController(IUserBL userBL)
        {
            _userBL = userBL;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync(UserRegisterDto userRegisterDto)
        {
            try
            {
                var result = await _userBL.CreateUserAsync(userRegisterDto);
                return Ok(new ApiResponse<UserResponseDto>
                {
                    Success = true,
                    Message = "User registered successfully",
                    Data = result
                });
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = new[] { ex.Message }
                });
            }
            catch (EmailDeliveryException ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway, new ApiResponse<object>
                {
                    Success = false,
                    Message = "User registered, but SMTP failed to send the welcome email.",
                    Errors = new[] { ex.Message }
                });
            }
        }
    }
}
