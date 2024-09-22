using Identity.Domain.Models;
using Identity.Domain.RequestDTOs;
using Identity.Domain.ResponseDTOs;
using Identity.Services.Jwt;
using Microsoft.AspNetCore.Mvc;
using Identity.Repositories;

namespace Identity.API.Controllers;

public class AuthController(IUserRepository repository, JwtService jwtService,ILogger<AuthController> logger) : Controller
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationDto body)
    {
        if (IsExistingUser(body.EmailAddress))
        {
            return BadRequest(new { message = "Email already exists!" });
        }

        await repository.RegisterAsync(body);
        logger.LogInformation("Registration is successful.");
        return Ok("Account Created Successfully");
    }

    [HttpPost("login")]

    public async Task<IActionResult> Login([FromBody] CredentialDto dto)
    {
        var response = await repository.AuthenticateAsync(dto.EmailAddress,dto.Password);
        
        if(response==null)
        {
            return BadRequest(new { message = "Invalid Credentials" });
        }
        
        Response.Cookies.Append("jwt", response, new CookieOptions
        {
            HttpOnly = true
        });

        return Ok(new AuthResponseDto(dto.EmailAddress,response));
    }

  //  [HttpGet("user")]
    public IActionResult User()
    {
        try
        {
            var jwt = Request.Cookies["jwt"];

            var token = jwtService.Verify(jwt);

            int userId = int.Parse(token.Issuer);

           // var user = repository.GetById(userId);

            return Ok();
        }
        catch (Exception)
        {
            return Unauthorized();
        }
    }

   // [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");

        return Ok(new
        {
            message = "success"
        });
    }

    public bool IsExistingUser(string email)
    {
        if (!string.Equals(email as string, "string", StringComparison.OrdinalIgnoreCase))
        {
            return repository.GetByEmail(email);
        }
        else 
            return false;
    }
}