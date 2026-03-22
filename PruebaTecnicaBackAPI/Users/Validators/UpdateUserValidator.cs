using FluentValidation;
using PruebaTecnicaBackAPI.Users.DTOs;

namespace PruebaTecnicaBackAPI.Users.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDTO>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(100).WithMessage("Máximo 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es requerido.")
                .EmailAddress().WithMessage("Email inválido.")
                .MaximumLength(100);

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("IsActive es requerido.");
        }
    }
}
