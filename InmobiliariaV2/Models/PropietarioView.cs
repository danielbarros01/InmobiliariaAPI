namespace InmobiliariaV2.Models
{
    public class PropietarioView
    {
        public int Id { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }

        public PropietarioView(Propietario p)
        {
            Id = p.Id;
            Dni = p.Dni;
            Nombre = p.Nombre;
            Apellido = p.Apellido;
            Email = p.Email;
            Telefono = p.Telefono;
        }
    }
}
