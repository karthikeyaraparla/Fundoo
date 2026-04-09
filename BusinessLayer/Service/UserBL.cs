using BusinessLayer.Interface;
using BusinessLayer.RabbitMQ;
using ModelLayer;
using ModelLayer.DTOs;
using DataBaseLayer.Interfaces;
using BusinessLayer.Exceptions;

namespace BusinessLayer.Service;

public class UserBL : IUserBL
{
    private readonly IUserDL _userDL;
    private readonly IRabbitMQProducer _rabbitMqProducer;

    public UserBL(IUserDL userDL, IRabbitMQProducer rabbitMqProducer)
    {
        _userDL = userDL;
        _rabbitMqProducer = rabbitMqProducer;
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

        await _rabbitMqProducer.SendMessageAsync(new EmailDto
        {
            To = createdUser.Email,
            Subject = subject,
            Body = body
        });

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
