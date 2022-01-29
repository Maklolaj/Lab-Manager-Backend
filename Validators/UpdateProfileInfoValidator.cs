using FluentValidation;
using System.ComponentModel.DataAnnotations;
using LabManAPI.Contracts.Requests;

namespace LabManAPI.Validators
{
    public class UserUpdateProfileInfoValidator : AbstractValidator<UserUpdateProfileInfo>
    {
        public UserUpdateProfileInfoValidator()
        {
            var EmailAddressPattern = new EmailAddressAttribute();
            RuleFor(x => x.Email).Must(u => EmailAddressPattern.IsValid(u));
            RuleFor(x => x.Password).NotEmpty().Length(6, 50);
            RuleFor(x => x.RepeatedPassword).NotEmpty().Length(6, 50);
        }

    }
}

