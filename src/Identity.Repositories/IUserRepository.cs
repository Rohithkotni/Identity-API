using Identity.Domain.Models;
using Identity.Domain.RequestDTOs;
using Identity.Domain.ResponseDTOs;

namespace Identity.Repositories
{
    public interface IUserRepository
    {
        public Task<int> RegisterAsync(Registration dto, Credential credential);
        public int Register(Registration dto, Credential credential);

        public Task<AuthResponseDto> Login(CredentialDto credential);

        public Task<CredentialDto> GetByEmail(string email);

        public object GetById(int userId);
        public Task<CustomerDto> GetCustomer(string emailAddress);
    }
}