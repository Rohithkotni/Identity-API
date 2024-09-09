namespace Identity.Domain.ResponseDTOs
{
    public record AuthResponseDto(string EmailAddress,string FirstName,string LastName,string AccessToken);
}
