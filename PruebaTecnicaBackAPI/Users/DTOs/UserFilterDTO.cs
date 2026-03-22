using System.ComponentModel;

namespace PruebaTecnicaBackAPI.Users.DTOs
{
    public class UserFilterDTO
    {
        [DefaultValue(true)]
        public bool? IsActive { get; set; }
    }
}
