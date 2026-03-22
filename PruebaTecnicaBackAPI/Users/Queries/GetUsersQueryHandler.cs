using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBackAPI.Data;
using PruebaTecnicaBackAPI.Users.DTOs;

namespace PruebaTecnicaBackAPI.Users.Queries
{
    public class GetUsersQueryHandler(AppDbContext db)
    {
        public async Task<List<UserResponseDTO>> HandleAsync(UserFilterDTO filter)
        {
            var query = db.Users.AsQueryable();

            if (filter.IsActive.HasValue)
                query = query.Where(u => u.IsActive == filter.IsActive.Value);
            
            return await query
                .Select(u => new UserResponseDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    IsActive = u.IsActive
                })
                .ToListAsync();
        }
    }
}
