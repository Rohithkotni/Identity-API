using Identity.Domain.Models;
using Identity.Domain.RequestDTOs;
using Identity.Domain.ResponseDTOs;

namespace Identity.Repositories
{
    public interface IUserRepository
    {
        public Task<int> RegisterAsync(RegistrationDto dto);
        public bool GetByEmail(string email);
        Task<string> AuthenticateAsync(string dtoEmailAddress,string password);
        public Task<AuthCustomerDto> GetCustomer(string emailAddress);

    }
}