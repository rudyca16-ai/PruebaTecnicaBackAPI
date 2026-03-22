using FluentValidation;
using PruebaTecnicaBackAPI.Addresses.DTOs;

namespace PruebaTecnicaBackAPI.Addresses.Validators
{
    public class CreateAddressValidator : AbstractValidator<CreateAddressDTO>
    {
        public CreateAddressValidator()
        {
            RuleFor(x => x.Street)
             .NotEmpty().WithMessage("La calle es requerida.")
             .MaximumLength(200);

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("La ciudad es requerida.")
                .MaximumLength(100);

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("El país es requerido.")
                .MaximumLength(100);

            RuleFor(x => x.ZipCode)
                .MaximumLength(20)
                .When(x => x.ZipCode is not null);
        }
    }
}
