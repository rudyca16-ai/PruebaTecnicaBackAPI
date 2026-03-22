using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBackAPI.Data;
using PruebaTecnicaBackAPI.Models;
using PruebaTecnicaBackAPI.Users.DTOs;
using System.Security.Cryptography;

namespace PruebaTecnicaBackAPI.Users.Commands
{
    /// <summary>
    /// Crea un nuevo usuario.
    /// </summary>
    /// <param name="command">Comando con los datos del usuario.</param>
    /// <returns>El user creado como DTO de respuesta.</returns>
    public class CreateUserCommandHandler(AppDbContext db)
    {
        public async Task<UserResponseDTO> HandleAsync(CreateUserCommand command)
        {
            var emailExists = await db.Users.AnyAsync(u => u.Email == command.Dto.Email);
            if (emailExists)
                throw new InvalidOperationException("El email ya está registrado.");

            var user = new User
            {
                Name = command.Dto.Name,
                Email = command.Dto.Email,
                IsActive = true,
                Password = BCrypt.Net.BCrypt.HashPassword(GenerateRandomPassword())
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                IsActive = user.IsActive
            };
        }

        /// <summary>
        /// Genera una contraseña aleatoria de 8 caracteres alfanuméricos
        /// usando un generador criptográficamente seguro.
        /// </summary>
        /// <returns>String aleatorio de 8 caracteres.</returns>
        private static string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Range(0, 8)
                .Select(_ => chars[RandomNumberGenerator.GetInt32(chars.Length)])
                .ToArray());
        }
    }
}
