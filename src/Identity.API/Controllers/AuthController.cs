using Identity.Domain.Models;
using Identity.Domain.RequestDTOs;
using Identity.Services;
using Microsoft.AspNetCore.Mvc;
using Identity.Repositories;

namespace Identity.API.Controllers;

public class AuthController(IUserRepository repository, JwtService jwtService) : Controller
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationDto body)
    {
        if (IsExistingUser(body.EmailAddress))
        {
            return BadRequest(new { message = "Email already exists!" });
        }
        var user = new Registration
        {
            FirstName = body.FirstName,
            LastName = body.LastName,
            EmailAddress = body.EmailAddress,
            PhoneNumber = body.PhoneNumber,
            EmailOptIn = body.EmailOptIn,
            TextOptIn = body.TextOptIn
        };
        var credential = new Credential
        {
            EmailAddress = body.EmailAddress,
            Password = BCrypt.Net.BCrypt.HashPassword(body.NewPassword)
        };

        return Created("success", await repository.RegisterAsync(user,credential));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(Credential dto)
    {
        var user = await repository.Authenticate(dto.EmailAddress);

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
        {
            return BadRequest(new { message = "Invalid Credentials" });
        }
        var customer=await repository.GetCustomer(dto.EmailAddress);
       // var jwt = jwtService.Generate();

        // Response.Cookies.Append("jwt", jwt, new CookieOptions
        // {
        //     HttpOnly = true
        // });

        return Ok(new
        {
            message = "success"
        });
    }

    [HttpGet("user")]
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

    [HttpPost("logout")]
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
        return repository.GetByEmail(email);
    }
}