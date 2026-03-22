using PruebaTecnicaBackAPI.Data;

namespace PruebaTecnicaBackAPI.Users.Commands
{
    public class DeleteUserCommandHandler(AppDbContext db)
    {
        /// <summary>
        /// Elimina un usuario existente.
        /// </summary>
        public async Task<bool> HandleAsync(DeleteUserCommand command)
        {
            var user = await db.Users.FindAsync(command.Id);
            if (user is null) return false;

            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
