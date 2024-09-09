namespace Identity.Domain.Models
{
    public class Registration
    {
        public int CustomerKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public bool? EmailOptIn { get; set; }
        public bool? TextOptIn { get; set; }
    }
}