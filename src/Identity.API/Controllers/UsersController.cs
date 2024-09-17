using Identity.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Identity.API.Controllers;

public class UsersController : Controller
{
    // GET
    [Authorize]
    [HttpGet("api/users")]
    public IActionResult GetUser()     
    {
        var customerInfoJson = User.FindFirst("CustomerInfo")?.Value;
        if (string.IsNullOrEmpty(customerInfoJson))
        {
            return Unauthorized("Customer information not found in the token.");
        }

        try
        {
            // Deserialize the JSON string back into a CustomerInfo object
            var customerInfo = JsonConvert.DeserializeObject<Registration>(customerInfoJson);

            // If deserialization was successful, return the customer information
            return Ok(customerInfo);
                
        }
        catch (JsonException ex)
        {
            // Handle potential deserialization errors
            return StatusCode(500, $"Error deserializing customer information: {ex.Message}");
        }
    }
}