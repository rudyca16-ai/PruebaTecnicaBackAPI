using FluentValidation;
using PruebaTecnicaBackAPI.Users.DTOs;

namespace PruebaTecnicaBackAPI.Users.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserDTO>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es requerido.")
                .EmailAddress().WithMessage("Email inválido.")
                .MaximumLength(100);
        }
    }
}
