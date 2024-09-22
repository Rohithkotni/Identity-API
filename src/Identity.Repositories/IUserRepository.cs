using Identity.Domain.Models;
using Identity.Domain.RequestDTOs;
using Identity.Domain.ResponseDTOs;
using Microsoft.Extensions.Logging;

namespace Identity.Repositories
{
    public interface IUserRepository
    {
        public Task<int> RegisterAsync(RegistrationDto dto);
        public bool GetByEmail(string email);
        Task<string> AuthenticateAsync(string dtoEmailAddress,string password);
        public Task<AuthCustomerDto> GetCustomer(string emailAddress);
        
        public Task<string> UpdateCustomerAsync(int customerKey,UpdateCustomerDto dto);

    }
}