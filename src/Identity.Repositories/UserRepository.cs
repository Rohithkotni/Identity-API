using Identity.Domain.Models;
using Identity.Domain.RequestDTOs;
using Identity.Domain.ResponseDTOs;
using Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Identity.Services.Jwt;
using Identity.Services.Mail;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Identity.Repositories
{
    public class UserRepository(DataContext db, IMapper mapper, JwtService jwtService,IMailService mailService,ILogger<UserRepository> logger) : IUserRepository
    {
        public bool GetByEmail(string email)
        {
            return db.Registrations.Any(x => x.EmailAddress == email);
        }

        public async Task<AuthCustomerDto> GetCustomer(string emailAddress)
        {
            var results = await db.Registrations.Where(x => x.EmailAddress == emailAddress).FirstOrDefaultAsync();
           var r= mapper.Map<AuthCustomerDto>(results);
           return r;
        }

        public async Task<string> UpdateCustomerAsync(int customerKey,UpdateCustomerDto updatedPayload)
        {
            var user = await db.Registrations.Where(x => x.CustomerKey==customerKey).FirstOrDefaultAsync();
            
            if (user == null)
            {
                logger.LogError($"User {customerKey} not found");
                throw new KeyNotFoundException("Customer not found");
            }

            // Get properties of the UpdateEntityDto
            var properties = typeof(UpdateCustomerDto).GetProperties();
        
            foreach (var prop in properties)
            {
                var newValue = prop.GetValue(updatedPayload);
                if (newValue != null && !string.Equals(newValue as string, "string", StringComparison.OrdinalIgnoreCase))
                {
                    // Find the corresponding property on the existing entity
                    var existingProp = typeof(Registration).GetProperty(prop.Name);
                    if (existingProp != null && existingProp.CanWrite)
                    {
                        existingProp.SetValue(user, newValue);
                    }
                }
            }

            // Save changes to the database
            await db.SaveChangesAsync();
            logger.LogInformation($"User {customerKey} updated");
            return "success";
        }

        public async Task<string> AuthenticateAsync(string dtoEmailAddress,string password)
        {
          var user =await db.Credentials.Where(x => x.EmailAddress == dtoEmailAddress).FirstOrDefaultAsync();
          
           if (user != null)
           {
               if (!BCrypt.Net.BCrypt.Verify(password, user?.Password))
               {
                   return null;
               }
               var customer =await db.Registrations.Where(x => x.EmailAddress == dtoEmailAddress).FirstOrDefaultAsync();
              var info= JsonConvert.SerializeObject(customer);
               var jwt = jwtService.Generate(info);
               return jwt;
           }

           return null;
        }

        public async Task<int> RegisterAsync(RegistrationDto body)
        {
            ArgumentNullException.ThrowIfNull(body);
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
            try
            {
                db.Registrations.Add(user);
                await db.SaveChangesAsync();
                credential.CustomerKey=user.CustomerKey;
                db.Credentials.Add(credential);
                await db.SaveChangesAsync();
                logger.LogInformation($"User {user.EmailAddress} registered");
                
               // await mailService.SendEmailAsync(user.EmailAddress,"Welcome to .Net Blog",user.FirstName);
                
                return user.CustomerKey;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
          
        }

    }
}