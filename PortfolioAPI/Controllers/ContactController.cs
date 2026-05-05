using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Models;
using Resend;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly ResendClient _resend;
    private readonly IConfiguration _config;

    public ContactController(ResendClient resend, IConfiguration config)
    {
        _resend = resend;
        _config = config;
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail([FromBody] ContactRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("All fields are required.");

        var message = new EmailMessage();
        message.From = "Portfolio <onboarding@resend.dev>";
        message.To.Add(_config["Resend:RecipientEmail"]!);
        message.Subject = $"Portfolio Message from {request.Name}";
        message.TextBody = $"From: {request.Name}\nEmail: {request.Email}\n\n{request.Message}";

        await _resend.EmailSendAsync(message);

        return Ok();
    }
}