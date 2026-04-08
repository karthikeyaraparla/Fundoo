using ModelLayer.DTOs;
using System.Collections.Generic;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        Task<UserResponseDto> CreateUserAsync(UserRegisterDto userDto);
    }
}
