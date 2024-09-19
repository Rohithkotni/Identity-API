using Identity.Domain.Models;
using Identity.Domain.RequestDTOs;
using Identity.Repositories;
using Identity.Services.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Identity.API.Controllers;

public class UsersController(IUserRepository repository) : Controller
{
    // GET
    [Authorize]
    [HttpGet("api/users")]
    public async Task<IActionResult> GetUser()     
    {
        var customerInfoJson = User.FindFirst("CustomerInfo")?.Value;
        if (string.IsNullOrEmpty(customerInfoJson))
        {
            return Unauthorized("Customer information not found in the token.");
        }

        try
        {
            // Deserialize the JSON string back into a CustomerInfo object
            var customerInfofromJwt =JsonConvert.DeserializeObject<Registration>(customerInfoJson);
            var customerInfo = await repository.GetCustomer(customerInfofromJwt.EmailAddress);
            // If deserialization was successful, return the customer information
            return Ok(customerInfo);
                
        }
        catch (JsonException ex)
        {
            // Handle potential deserialization errors
            return StatusCode(500, $"Error deserializing customer information: {ex.Message}");
        }
    }
   
    [Authorize]
    [HttpPut("api/users")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateCustomerDto dto)     
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
            return Ok(await repository.UpdateCustomerAsync(customerInfo.CustomerKey, dto));

        }
        catch (JsonException ex)
        {
            // Handle potential deserialization errors
            return StatusCode(500, $"Error updating customer information: {ex.Message}");
        }
    }
   
}