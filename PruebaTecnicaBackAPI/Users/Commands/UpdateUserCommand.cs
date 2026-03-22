using PruebaTecnicaBackAPI.Users.DTOs;

namespace PruebaTecnicaBackAPI.Users.Commands
{
    public record UpdateUserCommand(int Id, UpdateUserDTO Dto);
}
