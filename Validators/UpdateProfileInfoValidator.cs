using FluentValidation;
using System.ComponentModel.DataAnnotations;
using LabManAPI.Contracts.Requests;

namespace LabManAPI.Validators
{
    public class UserChangeEmailInfoValidator : AbstractValidator<UserChangeEmailInfo>
    {
        public UserChangeEmailInfoValidator()
        {
            var EmailAddressPattern = new EmailAddressAttribute();
            RuleFor(x => x.Email).Must(u => EmailAddressPattern.IsValid(u));
            RuleFor(x => x.Password).Length(6, 50);
        }

    }

    public class UserChangePasswordInfoValidator : AbstractValidator<UserChangePasswordInfo>
    {
        public UserChangePasswordInfoValidator()
        {
            RuleFor(x => x.Password).Length(6, 50);
            RuleFor(x => x.RepeatedPassword).Length(6, 50);
        }

    }
}



