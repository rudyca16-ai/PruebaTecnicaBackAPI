using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBackAPI.Currencies.DTOs;
using PruebaTecnicaBackAPI.Data;

namespace PruebaTecnicaBackAPI.Currencies.Validators
{
    public class ConvertCurrencyValidator : AbstractValidator<ConvertCurrencyDTO>
    {
        public ConvertCurrencyValidator(AppDbContext db)
        {
            RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("El monto debe ser mayor a 0.");

            RuleFor(x => x.FromCurrencyCode)
                .NotEmpty().WithMessage("El código de fromCurrency es requerido.")
                .Length(3).WithMessage("El código debe tener exactamente 3 caracteres.")
                .Matches(@"^[A-Z]+$").WithMessage("El código debe estar en mayúsculas.")
                .MustAsync(async (code, _) =>
                    await db.Currencies.AnyAsync(c => c.Code == code)
                ).WithMessage(m => $"fromCurrencyCode = {m.FromCurrencyCode} no existe en la base de datos.");

            RuleFor(x => x.ToCurrencyCode)
                .NotEmpty().WithMessage("El código de toCurrency es requerido.")
                .Length(3).WithMessage("El código debe tener exactamente 3 caracteres.")
                .Matches(@"^[A-Z]+$").WithMessage("El código debe estar en mayúsculas.")
                .MustAsync(async (code, _) =>
                    await db.Currencies.AnyAsync(c => c.Code == code)
                ).WithMessage(m => $"toCurrencyCode = {m.ToCurrencyCode} no existe en la base de datos.")
                .NotEqual(x => x.FromCurrencyCode)
                .WithMessage("El código de fromCurrency y toCurrency no pueden ser iguales.");
        }
    }
}
