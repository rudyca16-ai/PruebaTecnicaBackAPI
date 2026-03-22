using PruebaTecnicaBackAPI.Currencies.DTOs;
using PruebaTecnicaBackAPI.Data;
using PruebaTecnicaBackAPI.Models;

namespace PruebaTecnicaBackAPI.Currencies.Commands
{
    public class CreateCurrencyCommandHandler(AppDbContext db)
    {
        /// <summary>
        /// Crea una nueva moneda.
        /// </summary>
        public async Task<CurrencyResponseDTO> CreateAsync(CreateCurrencyCommand command)
        {
            var currency = new Currency
            {
                Code = command.Dto.Code,
                Name = command.Dto.Name,
                RateToBase = command.Dto.RateToBase
            };

            db.Currencies.Add(currency);
            await db.SaveChangesAsync();

            return new CurrencyResponseDTO
            {
                Id = currency.Id,
                Code = currency.Code,
                Name = currency.Name,
                RateToBase = currency.RateToBase
            };
        }
    }
}
