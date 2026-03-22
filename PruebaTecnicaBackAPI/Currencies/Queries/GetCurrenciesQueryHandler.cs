using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBackAPI.Currencies.DTOs;
using PruebaTecnicaBackAPI.Data;

namespace PruebaTecnicaBackAPI.Currencies.Queries
{
    public class GetCurrenciesQueryHandler(AppDbContext db)
    {
        /// <summary>
        /// Obtiene todas las monedas registradas.
        /// </summary>
        public async Task<List<CurrencyResponseDTO>> GetAllAsync()
        {
            return await db.Currencies
                .Select(c => new CurrencyResponseDTO
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    RateToBase = c.RateToBase
                })
                .ToListAsync();
        }
    }
}
