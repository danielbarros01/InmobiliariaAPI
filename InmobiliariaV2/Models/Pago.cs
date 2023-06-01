using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaV2.Models
{
    [Table("pagos")]
    public class Pago
    {
        [Key]
        public int NumeroPago { get; set; }
        public DateTime Fecha { get; set; }

        [Column("contrato_Id")]
        public int ContratoId { get; set; }
    }
}
