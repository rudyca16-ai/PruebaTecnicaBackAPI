using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBackAPI.Addresses.DTOs;
using PruebaTecnicaBackAPI.Data;

namespace PruebaTecnicaBackAPI.Addresses.Queries
{
    public class GetAddressesByUserQueryHandler(AppDbContext db)
    {
        /// <summary>
        /// Obtiene todas las direcciones de un usuario.
        /// </summary>
        public async Task<List<AddressResponseDTO>> HandleAsync(GetAddressesByUserQuery query)
        {
            var user = await db.Users.FindAsync(query.UserId)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            return await db.Addresses
                .Where(a => a.UserId == query.UserId)
                .Select(a => new AddressResponseDTO
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    Street = a.Street,
                    City = a.City,
                    Country = a.Country,
                    ZipCode = a.ZipCode
                })
                .ToListAsync();
        }
    }
}
