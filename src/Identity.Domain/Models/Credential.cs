using System.Text.Json.Serialization;

namespace Identity.Domain.Models
{
    public class Credential
    {
        public int CustomerKey { get; set; }
        public int EmailAddressId { get; set; }
        public string EmailAddress { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
    }
}
