using System.ComponentModel;

namespace PruebaTecnicaBackAPI.Addresses.DTOs
{
    public class UpdateAddressDTO
    {
        [DefaultValue("Calle Falsa 123")]
        public string Street { get; set; } = string.Empty;
        [DefaultValue("Asunción")]
        public string City { get; set; } = string.Empty;
        [DefaultValue("Paraguay")]
        public string Country { get; set; } = string.Empty;
        [DefaultValue("9999")]
        public string? ZipCode { get; set; }
    }
}
