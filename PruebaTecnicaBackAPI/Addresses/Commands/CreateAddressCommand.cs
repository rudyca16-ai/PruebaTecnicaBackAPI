using PruebaTecnicaBackAPI.Addresses.DTOs;

namespace PruebaTecnicaBackAPI.Addresses.Commands
{
    public record CreateAddressCommand(int UserId, CreateAddressDTO Dto);
}
