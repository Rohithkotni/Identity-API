using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Identity.Domain.RequestDTOs
{
    public class RegistrationDto()
    {
        //This payload can be used for Registration

        [Required]
        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [Required]
        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [Required]
        [DataMember(Name = "newPassword")]
        public string NewPassword { get; set; }

        [Required]
        [DataMember(Name = "confirmPassword")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataMember(Name = "emailAddress")]
        public string EmailAddress { get; set; }

        [Required]
        [DataMember(Name = "phoneNumber")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataMember(Name = "emailOptIn")]
        public bool? EmailOptIn { get; set; }

        [Required]
        [DataMember(Name = "textOptIn")]
        public bool? TextOptIn { get; set; }
    }
}