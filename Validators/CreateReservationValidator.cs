using FluentValidation;
using LabManAPI.Models;

namespace LabManAPI.Validators
{
    public class CreateReservationValidator : AbstractValidator<Reservation>
    {
        public CreateReservationValidator()
        {
            RuleFor(x => x.EndDate - x.StartDate).Must(u => u == new System.TimeSpan(2, 0, 0));
        }

    }
}