using ModelLayer.DTOs;

namespace BusinessLayer.Interface
{
    public interface IAuthBL
    {
        Task<LoginResponseDto> Login(LoginDto dto);

        Task<int> Register(UserRegisterDto dto);
    }
}
