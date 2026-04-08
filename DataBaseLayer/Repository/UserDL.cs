using Dapper;
using DataBaseLayer.Interfaces;
using ModelLayer;
using System.Data;

namespace Databaseayer.Repository
{
    public class UserDL : IUserDL
    {
        private readonly IDbConnection _dbConnection;

        public UserDL(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            user.IsActive = true;

            const string sql = @"
                INSERT INTO Users (FirstName, LastName, Email, Password, CreatedAt, UpdatedAt, IsActive)
                VALUES (@FirstName, @LastName, @Email, @Password, @CreatedAt, @UpdatedAt, @IsActive);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            var userId = await _dbConnection.ExecuteScalarAsync<int>(sql, user);
            user.Id = userId;
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            const string sql = @"
                SELECT Id, FirstName, LastName, Email, Password, CreatedAt, UpdatedAt, IsActive
                FROM Users
                WHERE LOWER(Email) = LOWER(@Email);";

            return await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }
    }
}
