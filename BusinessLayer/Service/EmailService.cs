using System.Net;
using System.Net.Mail;
using BusinessLayer.Exceptions;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Service;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var settings = LoadSettings();

        try
        {
            using var smtp = new SmtpClient(settings.Host, settings.Port)
            {
                Credentials = new NetworkCredential(settings.UserName, settings.Password),
                EnableSsl = settings.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            // MailMessage is the built-in .NET type that holds the sender, recipients, subject, and body.
            using var message = new MailMessage
            {
                From = new MailAddress(settings.FromEmail ?? settings.UserName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(to);

            await smtp.SendMailAsync(message);
        }
        catch (SmtpException ex)
        {
            var details = ex.InnerException?.Message ?? ex.Message;
            if (details.Contains("Authentication Required", StringComparison.OrdinalIgnoreCase) ||
                details.Contains("client was not authenticated", StringComparison.OrdinalIgnoreCase))
            {
                throw new EmailDeliveryException(
                    "SMTP authentication failed. For Gmail, use your full Gmail address as UserName, enable 2-Step Verification, and use a Google App Password instead of your normal Gmail password.",
                    ex);
            }

            throw new EmailDeliveryException($"SMTP failed to send the email: {details}", ex);
        }
    }

    private SmtpSettings LoadSettings()
    {
        var settings = new SmtpSettings
        {
            Host = (_config["SmtpSettings:Host"] ?? string.Empty).Trim(),
            UserName = (_config["SmtpSettings:UserName"] ?? string.Empty).Trim(),
            Password = NormalizePassword(_config["SmtpSettings:Password"]),
            FromEmail = _config["SmtpSettings:FromEmail"]?.Trim()
        };

        if (!int.TryParse(_config["SmtpSettings:Port"], out var port))
            throw new InvalidOperationException("SMTP port must be a valid integer.");

        settings.Port = port;

        if (bool.TryParse(_config["SmtpSettings:EnableSsl"], out var enableSsl))
            settings.EnableSsl = enableSsl;

        if (string.IsNullOrWhiteSpace(settings.Host))
            throw new InvalidOperationException("SMTP host is missing.");

        if (string.IsNullOrWhiteSpace(settings.UserName))
            throw new InvalidOperationException("SMTP username is missing.");

        if (string.IsNullOrWhiteSpace(settings.Password))
            throw new InvalidOperationException("SMTP password is missing.");

        if (settings.UserName.Equals("yourmail@gmail.com", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("SMTP username is still using the placeholder value.");

        if (settings.Password.Equals("your-app-password", StringComparison.Ordinal))
            throw new InvalidOperationException("SMTP password is still using the placeholder value.");

        ValidateGmailSettings(settings);

        return settings;
    }

    private static void ValidateGmailSettings(SmtpSettings settings)
    {
        if (!settings.Host.Contains("gmail", StringComparison.OrdinalIgnoreCase))
            return;

        // SmtpClient works with Gmail most reliably on port 587 with SSL enabled.
        if (settings.Port != 587)
            throw new InvalidOperationException("For Gmail SMTP, use port 587.");

        if (!settings.EnableSsl)
            throw new InvalidOperationException("For Gmail SMTP, EnableSsl must be true.");

        if (!settings.UserName.Contains('@'))
            throw new InvalidOperationException("For Gmail SMTP, UserName must be the full Gmail address.");

        if (!string.IsNullOrWhiteSpace(settings.FromEmail) &&
            !settings.FromEmail.Equals(settings.UserName, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("For Gmail SMTP, FromEmail must match UserName.");
    }

    private static string NormalizePassword(string? rawPassword)
    {
        if (string.IsNullOrWhiteSpace(rawPassword))
            return string.Empty;

        // Google shows app passwords with spaces for readability, but SMTP needs the raw value.
        return rawPassword.Replace(" ", string.Empty).Trim();
    }
}
