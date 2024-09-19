using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


namespace Identity.Services.Mail;

public class MailService : IMailService
{
    MailSettings mailSettings = null;
    private readonly SmtpClient _smtpClient;
    public MailService(IOptions<MailSettings> options)
    {
        mailSettings = options.Value;
        _smtpClient = new SmtpClient("smtp.gmail.com", 587)
         {
                 Credentials = new NetworkCredential(mailSettings.UserName, mailSettings.Password),
                 EnableSsl = true
         };
     }
     
         public async Task<string> SendEmailAsync(string toEmail, string subject, string name)
         {
             var r=Directory.GetCurrentDirectory();
             var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "../Identity.Services/Mail/MailTemplate.html");
             // Load the email template
             var template = await File.ReadAllTextAsync(templatePath);
    
             // Replace placeholders with actual values
             template = template.Replace("{{name}}", name);
             var mailMessage = new MailMessage
             {
                 From = new MailAddress("sarotesting.123@gmail.com"),
                 Subject = subject,
                 Body = template,
                 IsBodyHtml = true // Set to true if your body contains HTML
             };
     
             mailMessage.To.Add(toEmail);
     
             try
             {
                 await _smtpClient.SendMailAsync(mailMessage);
                 return("Email sent successfully.");
             }
             catch (Exception ex)
             {
                return ($"Failed to send email: {ex.Message}");
             }
         }
     
    
}