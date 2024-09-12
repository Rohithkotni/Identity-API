using Identity.Domain.Models;
using Identity.Domain.RequestDTOs;
using Identity.Services;
using Microsoft.AspNetCore.Mvc;
using Identity.Repositories;

namespace Identity.API.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly JwtService _jwtService;
        public AuthController(IUserRepository repository, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegistrationDto body)
        {
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

            return Created("success", _repository.RegisterAsync(user,credential));
        }

        [HttpPost("login")]
        public IActionResult Login(Credential dto)
        {
            var user = _repository.GetByEmail(dto.EmailAddress);

            if (user.Result == null) return BadRequest(new { message = "Invalid Credentials" });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Result.Password))
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }
            var details=_repository.GetCustomer(dto.EmailAddress);
            var jwt = _jwtService.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

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

                var token = _jwtService.Verify(jwt);

                int userId = int.Parse(token.Issuer);

                var user = _repository.GetById(userId);

                return Ok(user);
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
    }
}
