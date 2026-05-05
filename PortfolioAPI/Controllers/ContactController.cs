using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using PortfolioAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IConfiguration _config;

    public ContactController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail([FromBody] ContactRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("All fields are required.");

        var settings = _config.GetSection("EmailSettings");

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Portfolio Contact", settings["SenderEmail"]));
        email.To.Add(new MailboxAddress("Brandon Ward", settings["RecipientEmail"]));
        email.Subject = $"Portfolio Message from {request.Name}";
        email.Body = new TextPart("plain")
        {
            Text = $"From: {request.Name}\nEmail: {request.Email}\n\n{request.Message}"
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(settings["SmtpHost"], int.Parse(settings["SmtpPort"]!), SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(settings["SenderEmail"], settings["SenderPassword"]);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);

        return Ok();
    }
}