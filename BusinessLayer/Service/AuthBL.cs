using BCrypt.Net;
using BusinessLayer.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessLayer.Interface;
using BusinessLayer.Service;
using DataBaseLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using ModelLayer;
using ModelLayer.DTOs;

public class AuthBL : IAuthBL
{
    private readonly IAuthDL _authDL;
    private readonly IConfiguration _config;
    private readonly EmailService _emailService; 

    public AuthBL(IAuthDL authDL, IConfiguration config, EmailService emailService)
    {
        _authDL = authDL;
        _config = config;
        _emailService = emailService;
    }

    public async Task<LoginResponseDto> Login(LoginDto dto)
    {
        var user = await _authDL.Login(dto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            throw new InvalidCredentialsException("Invalid email or password.");

        return GenerateJwt(user);
    }

    public async Task<int> Register(UserRegisterDto dto)
    {
        // BCrypt.HashPassword converts the plain password into a secure hash before saving.
        dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        
        var userId = await _authDL.Register(dto);

        var subject = "Welcome to Fundoo Notes ";

        var body = $@"
            <h2>Welcome {dto.FirstName}!</h2>
            <p>Your account has been created successfully.</p>
            <p>Start creating notes now </p>
            <br/>
            <b>Fundoo</b>
        ";

        await _emailService.SendEmailAsync(dto.Email, subject, body);

        return userId;
    }

    private LoginResponseDto GenerateJwt(User user)
    {
        var jwtKey = _config["Jwt:Key"]
                     ?? throw new InvalidOperationException("JWT key is missing.");

        var issuer = _config["Jwt:Issuer"]
                     ?? throw new InvalidOperationException("JWT issuer is missing.");

        var audience = _config["Jwt:Audience"]
                       ?? throw new InvalidOperationException("JWT audience is missing.");

        var expiryMinutes = int.TryParse(_config["Jwt:ExpiryMinutes"], out var configuredExpiry)
            ? configuredExpiry
            : 120;

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)
        );

        var creds = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);

        // The "UserId" claim is later read from the JWT inside authorized controllers.
        var claims = new[]
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds
        );

        return new LoginResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAtUtc = expiresAt
        };
    }
}
