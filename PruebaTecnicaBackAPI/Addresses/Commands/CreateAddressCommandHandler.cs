using PruebaTecnicaBackAPI.Addresses.DTOs;
using PruebaTecnicaBackAPI.Data;
using PruebaTecnicaBackAPI.Models;

namespace PruebaTecnicaBackAPI.Addresses.Commands
{
    public class CreateAddressCommandHandler(AppDbContext db)
    {
        /// <summary>
        /// Crea una nueva dirección para un usuario existente.
        /// </summary>
        /// <param name="command">Comando con los datos de la dirección.</param>
        /// <returns>La dirección creada como DTO de respuesta.</returns>
        /// <exception cref="KeyNotFoundException">Si el usuario no existe.</exception>
        public async Task<AddressResponseDTO> HandleAsync(CreateAddressCommand command)
        {
            _ = await db.Users.FindAsync(command.UserId)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            var address = new Address
            {
                UserId = command.UserId,
                Street = command.Dto.Street,
                City = command.Dto.City,
                Country = command.Dto.Country,
                ZipCode = command.Dto.ZipCode
            };

            db.Addresses.Add(address);
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
