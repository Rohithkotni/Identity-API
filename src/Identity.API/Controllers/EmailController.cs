using Identity.Domain.Models;
using Identity.Domain.RequestDTOs;
using Identity.Services.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Identity.API.Controllers;

public class EmailController(IMailService mailService) : Controller
{
    // GET
   // [Authorize]
    [HttpPost("api/sendmail")]
    public async Task<IActionResult> SendMail([FromBody] EmailRequestDto dto)
    {
        // var customerInfoJson = User.FindFirst("CustomerInfo")?.Value;
        // if (string.IsNullOrEmpty(customerInfoJson))
        // {
        //     return Unauthorized("Customer information not found in the token.");
        // }
        // var customerInfo =JsonConvert.DeserializeObject<Registration>(customerInfoJson);
        var result = await mailService.SendEmailAsync(dto.EmailAddress, "Welcome to My .Net API",
            dto.FirstName);

        if (result.Contains("Failed"))
        {
            return StatusCode(500, result);
        }
        return Ok(result);
    }
}