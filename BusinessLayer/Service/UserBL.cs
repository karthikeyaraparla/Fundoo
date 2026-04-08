using BusinessLayer.Interface;
using ModelLayer;
using ModelLayer.DTOs;
using DataBaseLayer.Interfaces;
using BusinessLayer.Exceptions;

namespace BusinessLayer.Service;

public class UserBL : IUserBL
{
    private readonly IUserDL _userDL;
    private readonly EmailService _emailService;

    public UserBL(IUserDL userDL, EmailService emailService)
    {
        _userDL = userDL;
        _emailService = emailService;
    }

    public async Task<UserResponseDto> CreateUserAsync(UserRegisterDto userDto)
    {
        var existingUser = await _userDL.GetUserByEmailAsync(userDto.Email);
        if (existingUser != null)
            throw new UserAlreadyExistsException($"Email '{userDto.Email}' is already in use");

        var user = new User
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var createdUser = await _userDL.CreateUserAsync(user);

        var subject = "Welcome to Fundoo Notes";
        var body = $@"
            <h2>Welcome {createdUser.FirstName}!</h2>
            <p>Your account has been created successfully.</p>
            <p>Start creating notes now.</p>
            <br/>
            <b>Fundoo Team</b>
        ";

        await _emailService.SendEmailAsync(createdUser.Email, subject, body);

        return new UserResponseDto
        {
            Id = createdUser.Id,
            FirstName = createdUser.FirstName,
            LastName = createdUser.LastName,
            Email = createdUser.Email,
            CreatedAt = createdUser.CreatedAt,
            UpdatedAt = createdUser.UpdatedAt,
            IsActive = createdUser.IsActive
        };
    }
}
