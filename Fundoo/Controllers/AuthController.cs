using BusinessLayer.Exceptions;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs;
using ModelLayer.Utility;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthBL _authBL;

    public AuthController(IAuthBL authBL)
    {
        _authBL = authBL;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var token = await _authBL.Login(dto);
            return Ok(new ApiResponse<LoginResponseDto>
            {
                Success = true,
                Message = "Login successful",
                Data = token
            });
        }
        catch (InvalidCredentialsException ex)
        {
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message,
                Errors = new[] { ex.Message }
            });
        }
    }
}
