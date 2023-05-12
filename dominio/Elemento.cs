using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Elemento
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        //Sobre escribo el Metodo ToString para resolver la info que muestra en el grid
        public override string ToString()
        {
            return Descripcion;
        }
    }
}
