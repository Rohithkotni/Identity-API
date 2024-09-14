using Identity.Domain.Models;
using Identity.Domain.RequestDTOs;
using Identity.Domain.ResponseDTOs;

namespace Identity.Repositories
{
    public interface IUserRepository
    {
        public Task<int> RegisterAsync(Registration dto, Credential credential);
        public bool GetByEmail(string email);
        Task<CredentialDto> Authenticate(string dtoEmailAddress);
        public Task<CustomerDto> GetCustomer(string emailAddress);
        public Task<AuthResponseDto> Login(CredentialDto credential);

    }
}