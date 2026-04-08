using Dapper;
using System.Data;
using ModelLayer;
using DataBaseLayer.Interfaces;
using ModelLayer.DTOs;

namespace DataBaseLayer.Repository
{
    public class AuthDL : IAuthDL
    {
        private readonly IDbConnection _db;

        public AuthDL(IDbConnection db)
        {
            _db = db;
        }

        public async Task<User> Login(string email)
        {
            var query = "SELECT * FROM Users WHERE Email = @Email";
            return await _db.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
        }
        public async Task<int> Register(UserRegisterDto dto)
        {
            var query = @"
    INSERT INTO Users (FirstName, LastName, Email, Password)
    VALUES (@FirstName, @LastName, @Email, @Password);
    SELECT CAST(SCOPE_IDENTITY() as int);";

            return await _db.ExecuteScalarAsync<int>(query, dto);
        }
    }
}
