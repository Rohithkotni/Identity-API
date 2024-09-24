using FluentValidation;
using Identity.Domain.RequestDTOs;

namespace Identity.Domain.Validators;

public class RegistrationValidator:AbstractValidator<RegistrationDto>
{
  public RegistrationValidator()
  {
    RuleFor(user => user.EmailAddress)
      .NotEmpty().WithMessage("Email is required.")
      .EmailAddress().WithMessage("Invalid email format.");

    RuleFor(user => user.FirstName).NotEmpty().WithMessage("First name is required.").NotEqual("string").WithMessage("Invalid first name.");
    RuleFor(user => user.LastName).NotEmpty().WithMessage("Last name is required.").NotEqual("string").WithMessage("Invalid last name.");
    RuleFor(user=>user.PhoneNumber).NotEmpty().WithMessage("Phone number is required.").NotEqual("string").WithMessage("Invalid phone number.");
    RuleFor(user => user.NewPassword)
      .NotEmpty().WithMessage("Password is required.")
      .NotEqual("string").WithMessage("Invalid password.")
      .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
      .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
      .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
      .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
      .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character (e.g., @, #, $).");
    
  }
}