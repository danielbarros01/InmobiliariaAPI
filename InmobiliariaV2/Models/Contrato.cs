using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaV2.Models
{
    public class Contrato
    {
        public int Id { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public string Condiciones { get; set; }
        public double Monto { get; set; }

        [Column("inmueble_Id")]
        public int InmuebleId { get; set; }
        public Inmueble Inmueble { get; set; }

        [Column("inquilino_Id")]
        public int InquilinoId { get; set; }
        public Inquilino Inquilino { get; set; }
    }
}
