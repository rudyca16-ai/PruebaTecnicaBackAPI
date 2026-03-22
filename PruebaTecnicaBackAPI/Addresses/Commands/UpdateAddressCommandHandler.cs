using PruebaTecnicaBackAPI.Addresses.DTOs;
using PruebaTecnicaBackAPI.Data;

namespace PruebaTecnicaBackAPI.Addresses.Commands
{
    public class UpdateAddressCommandHandler(AppDbContext db)
    {
        /// <summary>
        /// Actualiza una dirección existente.
        /// </summary>
        /// <exception cref="KeyNotFoundException">Si la dirección no existe.</exception>
        public async Task<AddressResponseDTO?> HandleAsync(UpdateAddressCommand command)
        {
            var address = await db.Addresses.FindAsync(command.Id);
            if (address is null)
                return null;

            address.Street = command.Dto.Street;
            address.City = command.Dto.City;
            address.Country = command.Dto.Country;
            address.ZipCode = command.Dto.ZipCode;

            await db.SaveChangesAsync();

            return new AddressResponseDTO
            {
                Id = address.Id,
                UserId = address.UserId,
                Street = address.Street,
                City = address.City,
                Country = address.Country,
                ZipCode = address.ZipCode
            };
        }

    }
}
