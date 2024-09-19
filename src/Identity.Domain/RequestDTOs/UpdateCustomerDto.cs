namespace Identity.Domain.RequestDTOs;

public class UpdateCustomerDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public bool? EmailOptIn { get; set; }
    public bool? TextOptIn { get; set; }
}