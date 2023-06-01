using InmobiliariaV2.Models;

namespace InmobiliariaV2.Utilities
{
    public class ValoresEnum
    {
        //metodo que devueleve los valores de enum independientemente del enum que sea
        public static IDictionary<int, string> ObtenerUsos(Enum e)
        {
            SortedDictionary<int, string> usos = new SortedDictionary<int, string>();
            Type tipoEnum = e.GetType(); //Obtengo el tipo del enum

            foreach (var valor in Enum.GetValues(tipoEnum)) //recorro todos los valores
            {
                usos.Add((int)valor, Enum.GetName(tipoEnum, valor));
            }
            return usos;
        }
    }
}
