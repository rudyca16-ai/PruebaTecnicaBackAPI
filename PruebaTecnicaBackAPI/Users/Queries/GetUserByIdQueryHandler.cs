using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBackAPI.Data;
using PruebaTecnicaBackAPI.Users.DTOs;

namespace PruebaTecnicaBackAPI.Users.Queries
{
    public class GetUserByIdQueryHandler(AppDbContext db)
    {
        public async Task<UserResponseDTO?> HandleAsync(GetUserByIdQuery query)
        {
            return await db.Users
                .Where(u => u.Id == query.Id)
                .Select(u => new UserResponseDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    IsActive = u.IsActive
                })
                .FirstOrDefaultAsync();
        }
    }
}
