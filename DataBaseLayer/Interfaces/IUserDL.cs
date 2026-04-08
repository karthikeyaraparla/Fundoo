using ModelLayer;
namespace DataBaseLayer.Interfaces;

public interface IUserDL
{
    Task<User> CreateUserAsync(User user);
    Task<User> GetUserByEmailAsync(string email);
}
