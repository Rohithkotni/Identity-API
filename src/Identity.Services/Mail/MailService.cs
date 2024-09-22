using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;


namespace Identity.Services.Mail;

public class MailService : IMailService
{
    MailSettings mailSettings = null;
    private readonly SmtpClient _smtpClient;
    private readonly ILogger<MailService> logger;
    public MailService(IOptions<MailSettings> options,ILogger<MailService> _logger)
    {
        mailSettings = options.Value;
        logger=_logger;
        _smtpClient = new SmtpClient("smtp.gmail.com", 587)
         {
                 Credentials = new NetworkCredential(mailSettings.UserName, mailSettings.Password),
                 EnableSsl = true
         };
     }
     
         public async Task<string> SendEmailAsync(string toEmail, string subject, string name)
         {
             var templatePath = Path.Combine(AppContext.BaseDirectory, "Identity.Services", "Mail", "MailTemplate.html");
             // Load the email template
             var template = await File.ReadAllTextAsync(templatePath);
    
             // Replace placeholders with actual values
             template = template.Replace("{{name}}", name);
             var mailMessage = new MailMessage
             {
                 From = new MailAddress("sarotesting.123@gmail.com"),
                 Subject = subject,
                 Body =  $"Hello {name}!, \n nice to meet you! This is a Test email generated as part of the API Integration Testing. \n Thank you for your cooperation",
                 IsBodyHtml = false // Set to true if your body contains HTML
             };
     
             mailMessage.To.Add(toEmail);
     
             try
             {
                 await _smtpClient.SendMailAsync(mailMessage);
                 logger.LogInformation($"Email sent to {toEmail}");
                 return("Email sent successfully.");
             }
             catch (Exception ex)
             {
                 logger.LogError($"Error sending email to {toEmail}: {ex.Message}");
                return ($"Failed to send email");
             }
         }
     
    
}