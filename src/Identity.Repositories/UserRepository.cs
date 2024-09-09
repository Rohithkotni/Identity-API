using Identity.Domain.Models;
using Identity.Domain.RequestDTOs;
using Identity.Domain.ResponseDTOs;
using MyApp.Identity.Infrastructure;
using System.Net;

namespace Identity.Repositories
{
    
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _db;
        public UserRepository(DataContext db) {
            _db = db;
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

            _db.Registrations.Add(customer);
            _db.Credentials.Add(credential);

            // Use the asynchronous version of SaveChanges
            int result = await _db.SaveChangesAsync();

            // Update the CustomerKey if needed (optional depending on your logic)
            customer.CustomerKey = result;

            return result;
        }

        public int Register(Registration customer, Credential credential)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            if (credential == null) throw new ArgumentNullException(nameof(credential));

            _db.Registrations.Add(customer);
            _db.Credentials.Add(credential);

            // Use the asynchronous version of SaveChanges
            int result = _db.SaveChanges();

            // Update the CustomerKey if needed (optional depending on your logic)
            customer.CustomerKey = result;

            return result;
        }

    }
}
