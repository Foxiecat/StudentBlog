using FastEndpoints;
using FluentValidation;
using src.Features.Users.DTOs;

namespace src.Validators;

public class UserRequestValidator : Validator<UserRequest>
{
    public UserRequestValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty().WithMessage("Username is required")
            .Length(2, 20).WithMessage("Username must be between 2 and 20 characters");
        
        RuleFor(user => user.Firstname)
            .NotEmpty().WithMessage("Firstname is required")
            .Length(2, 20).WithMessage("Firstname must be between 2 and 20 characters");
        
        RuleFor(user => user.Lastname)
            .NotEmpty().WithMessage("Lastname is required")
            .Length(2, 20).WithMessage("Lastname must be between 2 and 20 characters");
        
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");
        
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Invalid Password: Must be at least 8 characters")
            .Matches("[0-9]+").WithMessage("Invalid Password: Must contain at least 1 number")
            .Matches("[a-z]+").WithMessage("Invalid Password: Must contain at least 1 lowercase letter")
            .Matches("[A-Z]+").WithMessage("Invalid Password: Must contain at least 1 capital letter")
            .Matches("[^a-zA-Z\\d]+").WithMessage("Invalid Password: Must contain at least 1 special character")
            .Must(password => !password.Any(character => "æøåÆØÅ"
                .Contains(character))).WithMessage("Invalid Password: Contains invalid characters. '(æ ø å Æ Ø Å)'");
    }
}