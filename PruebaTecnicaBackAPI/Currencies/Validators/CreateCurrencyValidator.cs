using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBackAPI.Currencies.DTOs;
using PruebaTecnicaBackAPI.Data;

namespace PruebaTecnicaBackAPI.Currencies.Validators
{
    public class CreateCurrencyValidator : AbstractValidator<CreateCurrencyDTO>
    {
        public CreateCurrencyValidator(AppDbContext db)
        {
            RuleFor(x => x.Code)
            .NotEmpty().WithMessage("El código es requerido.")
            .Length(3).WithMessage("El código debe tener exactamente 3 caracteres.")
            .Matches(@"^[A-Z]+$").WithMessage("El código debe estar en mayúsculas.")
            .MustAsync(async (code, _) =>
                !await db.Currencies.AnyAsync(c => c.Code == code)
            ).WithMessage(x => $"El código '{x.Code}' ya existe.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es requerido.")
                .MaximumLength(100).WithMessage("Máximo 100 caracteres.");

            RuleFor(x => x.RateToBase)
                .GreaterThan(0).WithMessage("El RateToBase debe ser mayor a 0.");
        }
    }
}
