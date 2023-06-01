using System.ComponentModel.DataAnnotations;

namespace InmobiliariaV2.Models
{
    public class PasswordsPropietario
    {
        [DataType(DataType.Password)]
        public string contraseñaActual { get; set; }

        [DataType(DataType.Password)]
        public string contraseñaNueva { get; set; }
    }
}
