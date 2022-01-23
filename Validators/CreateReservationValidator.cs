using FluentValidation;
using LabManAPI.Models;
using LabManAPI.Contracts.Requests;

namespace LabManAPI.Validators
{
    public class CreateReservationValidator : AbstractValidator<CreateReservationRequest>
    {
        public CreateReservationValidator()
        {
            RuleFor(x => x.EndDate - x.StartDate).Must(u => u == new System.TimeSpan(2, 0, 0));
        }

    }
}