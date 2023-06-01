using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaV2.Models
{

    public enum enumUso
    {
        Residencial = 1,
        Comercial = 2
    }

    public class Inmueble
    {
        public int Id { get; set; }

        public string Direccion { get; set; }

        public int Uso { get; set; }
        public string UsoNombre => Uso > 0 ? ((enumUso)Uso).ToString() : "";

        [Column("Cantidad_ambientes")]
        public int CantidadAmbientes { get; set; }

        public string Coordenadas { get; set; }
        public double Precio { get; set; }
        public bool Disponible { get; set; }

        [Column("propietario_Id")]
        public int PropietarioId { get; set; }
        public Propietario Propietario { get; set; }

        [Column("tipo_inmueble_Id")]
        public int TipoInmuebleId { get; set; }
        public TipoInmueble TipoInmueble { get; set; }
    }
}
