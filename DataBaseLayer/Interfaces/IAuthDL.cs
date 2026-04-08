using ModelLayer;
using ModelLayer.DTOs;

namespace DataBaseLayer.Interfaces
{
    public interface IAuthDL
    {
        Task<User> Login(string email);
        Task<int> Register(UserRegisterDto dto);

    }
}
