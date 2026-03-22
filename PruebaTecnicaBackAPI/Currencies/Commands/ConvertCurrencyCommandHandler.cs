using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBackAPI.Currencies.DTOs;
using PruebaTecnicaBackAPI.Data;

namespace PruebaTecnicaBackAPI.Currencies.Commands
{
    public class ConvertCurrencyCommandHandler(AppDbContext db)
    {
        /// <summary>
        /// Convierte un monto de una moneda a otra usando RateToBase.
        /// </summary>
        public async Task<ConversionResultDTO> ConvertAsync(ConvertCurrencyCommand command)
        {
            var from = await db.Currencies
                .FirstOrDefaultAsync(c => c.Code == command.Dto.FromCurrencyCode)
                ?? throw new KeyNotFoundException($"Moneda '{command.Dto.FromCurrencyCode}' no encontrada.");

            var to = await db.Currencies
                .FirstOrDefaultAsync(c => c.Code == command.Dto.ToCurrencyCode)
                ?? throw new KeyNotFoundException($"Moneda '{command.Dto.ToCurrencyCode}' no encontrada.");

            // Fórmula de conversión
            var montoBase = command.Dto.Amount * from.RateToBase;
            var convertedAmount = montoBase / to.RateToBase;

            return new ConversionResultDTO
            {
                FromCurrencyCode = from.Code,
                ToCurrencyCode = to.Code,
                OriginalAmount = command.Dto.Amount,
                ConvertedAmount = convertedAmount
            };
        }
    }
}
