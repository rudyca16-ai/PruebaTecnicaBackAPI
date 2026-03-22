using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBackAPI.Data;
using PruebaTecnicaBackAPI.Users.DTOs;

namespace PruebaTecnicaBackAPI.Users.Commands
{
    public class UpdateUserCommandHandler(AppDbContext db)
    {
        /// <summary>
        /// Actualiza el usuario existente.
        /// </summary>
        public async Task<UserResponseDTO?> HandleAsync(UpdateUserCommand command)
        {
            var user = await db.Users.FindAsync(command.Id);
            if (user is null) return null;

            var emailExists = await db.Users.AnyAsync(u => u.Email == command.Dto.Email && u.Id != command.Id);
            if (emailExists)
                throw new InvalidOperationException("El email ya está registrado.");

            user.Name = command.Dto.Name;
            user.Email = command.Dto.Email;
            user.IsActive = command.Dto.IsActive;

            await db.SaveChangesAsync();

            return new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                IsActive = user.IsActive
            };
        }
    }
}
