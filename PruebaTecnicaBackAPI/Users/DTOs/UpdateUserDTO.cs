using System.ComponentModel;

namespace PruebaTecnicaBackAPI.Users.DTOs
{
    public class UpdateUserDTO
    {
        [DefaultValue("Juan")]
        public string Name { get; set; } = string.Empty;
        [DefaultValue("juan@test.com")]
        public string Email { get; set; } = string.Empty;
        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
