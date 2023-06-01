using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaV2.Models
{
    [Table("tipos_inmueble")]
    public class TipoInmueble
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
    }
}
