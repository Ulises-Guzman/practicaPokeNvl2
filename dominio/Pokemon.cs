using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Pokemon
    {
        public int Id { get; set; }

        //Annotation, sirve para modificar los nombres de la columnas
        [DisplayName("Número")]
        public int Numero { get; set; }

        public string Nombre { get; set; }

        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        public string UrlImagen { get; set; }

        //El Tipo de Pokemon es un obejto del Tipo Elemento
        public Elemento Tipo { get; set; }

        public Elemento Debilidad { get; set; }
    }
}
