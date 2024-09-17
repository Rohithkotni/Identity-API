using Identity.Domain.Models;
using Identity.Domain.RequestDTOs;
using Identity.Domain.ResponseDTOs;
using Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Identity.Services;
using Newtonsoft.Json;

namespace Identity.Repositories
{
    public class UserRepository(DataContext db, IMapper mapper, JwtService jwtService) : IUserRepository
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
                // Update the CustomerKey if needed (optional depending on your logic)
                return user.CustomerKey;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
          
        }



        //public async Task<AuthResponseDto?> Authenticate(CredentialDto model)
        //{
        //    var user = await db.Credentials.SingleOrDefaultAsync(x => x.EmailAddress == model.EmailAddress && x.Password == model.Password);

        //    // return null if user not found
        //    if (user == null) return null;

        //    // authentication successful so generate jwt token
        //    var token = await generateJwtToken(user);

        //    return new AuthResponseDto(user.EmailAddress,token);
        //}

        //public async Task<IEnumerable<User>> GetAll()
        //{
        //    return await db.Users.Where(x => x.isActive == true).ToListAsync();
        //}

        //public async Task<User?> GetById(int id)
        //{
        //    return await db.Users.FirstOrDefaultAsync(x => x.Id == id);
        //}

        //public async Task<User?> AddAndUpdateUser(User userObj)
        //{
        //    bool isSuccess = false;
        //    if (userObj.Id > 0)
        //    {
        //        var obj = await db.Users.FirstOrDefaultAsync(c => c.Id == userObj.Id);
        //        if (obj != null)
        //        {
        //            // obj.Address = userObj.Address;
        //            obj.FirstName = userObj.FirstName;
        //            obj.LastName = userObj.LastName;
        //            db.Users.Update(obj);
        //            isSuccess = await db.SaveChangesAsync() > 0;
        //        }
        //    }
        //    else
        //    {
        //        await db.Users.AddAsync(userObj);
        //        isSuccess = await db.SaveChangesAsync() > 0;
        //    }

        //    return isSuccess ? userObj : null;
        //}
    }
}