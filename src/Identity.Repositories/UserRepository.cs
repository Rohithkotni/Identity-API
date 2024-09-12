using Identity.Domain.Models;
using Identity.Domain.RequestDTOs;
using Identity.Domain.ResponseDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Identity.Infrastructure;

namespace Identity.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext db;
        private readonly IServiceProvider serviceProvider;

        public UserRepository(DataContext _db,IServiceProvider _serviceProvider)
        {
            db = _db;
            serviceProvider= _serviceProvider;
        }

        public Task<CredentialDto> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public object GetById(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerDto> GetCustomer(string emailAddress)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponseDto> Login(CredentialDto credential)
        {
            throw new NotImplementedException();
        }

        public async Task<int> RegisterAsync(Registration customer, Credential credential)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            if (credential == null) throw new ArgumentNullException(nameof(credential));

            try
            {
                using (var scope = serviceProvider.CreateAsyncScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                    dbContext.Registrations.AddAsync(customer);
                    dbContext.Credentials.AddAsync(credential);

                    // Use the asynchronous version of SaveChanges
                    await dbContext.SaveChangesAsync();

                    // Update the CustomerKey if needed (optional depending on your logic)
                    return customer.CustomerKey;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int Register(Registration customer, Credential credential)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            if (credential == null) throw new ArgumentNullException(nameof(credential));

            db.Registrations.Add(customer);
            db.Credentials.Add(credential);

            // Use the asynchronous version of SaveChanges
            int result = db.SaveChanges();

            // Update the CustomerKey if needed (optional depending on your logic)
            customer.CustomerKey = result;

            return result;
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