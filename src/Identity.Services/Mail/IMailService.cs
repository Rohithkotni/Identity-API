namespace Identity.Services.Mail;

public interface IMailService
{
    Task<string> SendEmailAsync(string toEmail, string subject, string body);
}