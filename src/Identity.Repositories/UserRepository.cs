using Identity.Domain.Models;
using Identity.Domain.RequestDTOs;
using Identity.Domain.ResponseDTOs;
using Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Identity.Repositories
{
    public class UserRepository(DataContext db) : IUserRepository
    {
        private readonly DataContext _db = db;

        public bool GetByEmail(string email)
        {
            return _db.Registrations.Any(x => x.EmailAddress == email);
        }

        public async Task<CustomerDto> GetCustomer(string emailAddress)
        {
            var results = await _db.Registrations.Where(x => x.EmailAddress == emailAddress).FirstOrDefaultAsync();
            return Mapper.Map<CustomerDto>(results);
        }

        public async Task<CredentialDto> Authenticate(string dtoEmailAddress)
        {
           var result =await _db.Credentials.Where(x => x.EmailAddress == dtoEmailAddress).FirstOrDefaultAsync();
           var customer = new CredentialDto()
           {
               EmailAddress = result.EmailAddress,
               Password = result.Password
           };
           return customer;
        }

        public Task<AuthResponseDto> Login(CredentialDto credential)
        {
            throw new NotImplementedException();
        }

        public async Task<int> RegisterAsync(Registration customer, Credential credential)
        {
            ArgumentNullException.ThrowIfNull(customer);
            ArgumentNullException.ThrowIfNull(credential);
            try
            {
                // await _db.Set<Registration>().AddAsync(customer);
                // await _db.Set<Credential>().AddAsync(credential);
                _db.Registrations.Add(customer);
                _db.Credentials.Add(credential);
                // Use the asynchronous version of SaveChanges
                await _db.SaveChangesAsync();
                // Update the CustomerKey if needed (optional depending on your logic)
                return customer.CustomerKey;

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