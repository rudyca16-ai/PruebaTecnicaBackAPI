using PruebaTecnicaBackAPI.Addresses.DTOs;

namespace PruebaTecnicaBackAPI.Addresses.Commands
{
    public record UpdateAddressCommand(int Id, UpdateAddressDTO Dto);
}
