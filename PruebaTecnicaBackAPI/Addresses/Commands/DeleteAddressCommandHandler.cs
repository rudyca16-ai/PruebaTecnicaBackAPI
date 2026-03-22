using PruebaTecnicaBackAPI.Data;

namespace PruebaTecnicaBackAPI.Addresses.Commands
{
    public class DeleteAddressCommandHandler(AppDbContext db)
    {
        /// <summary>
        /// Elimina una dirección existente.
        /// </summary>
        public async Task<bool> HandleAsync(DeleteAddressCommand command)
        {
            var address = await db.Addresses.FindAsync(command.Id);
            if (address is null) return false;

            db.Addresses.Remove(address);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
