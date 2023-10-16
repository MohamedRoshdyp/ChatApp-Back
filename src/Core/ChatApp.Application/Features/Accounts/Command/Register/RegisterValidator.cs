using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Accounts.Command.Register;
public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.UserName).NotNull()
            .NotEmpty().WithMessage("User Name Not Empty !")
            .MinimumLength(3).WithMessage("{PropertyName} Limited With 3 character");

        RuleFor(x => x.Email).NotNull()
            .NotEmpty().WithMessage("{PropertyName} is required")
            .EmailAddress().WithMessage("{PropertyValue} not valid");

        RuleFor(x => x.Password).NotNull()
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MinimumLength(6).WithMessage("{PropertyName} length must be at least 6")
            .MaximumLength(9).WithMessage("{PropertyName} length must be not exceed 9")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                    .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                    .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");
                    //.Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");


        RuleFor(x => x.KnownAs).NotNull()
            .NotEmpty().WithMessage("Known as required !")
            .MinimumLength(3).WithMessage("{PropertyName} Limited With 3 character");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("{PropertyName} is Required !")
            .Must(beAtLeast15YearsOld)
            .WithMessage("The person must be at least 15 years old");
    }

    private bool beAtLeast15YearsOld(DateTime dob)
    {
        int age = DateTime.Today.Year - dob.Year;
        if (dob.Date > DateTime.Today.AddYears(-age))
            age--;
        return age >= 15;

    }
}
