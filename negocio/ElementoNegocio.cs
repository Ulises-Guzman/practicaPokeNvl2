using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Para utilizar las clases de dominio, Agrego la libreria/clases: dominio
using dominio;


namespace negocio
{
    public class ElementoNegocio
    {
        //Creo un Metodo para traer un listado de Elementos
        public List<Elemento> listar()
        {
            List<Elemento> lista = new List<Elemento>();
            
            //Creo el Objeto AccesoDatos
            //Este objeto tiene: una conexion instanciada, un comando instaciado, un lector, y una cadena de la conexion configurada
            AccesoDatos datos = new AccesoDatos();

            try
            {
                //Seteo la consulta, utilizando el metodo que hereda de AccesoDatos.
                datos.setearConsulta("SELECT Id, Descripcion FROM ELEMENTOS");
                datos.ejecutarLectura();

                //Maqueto la lista, leyendo con el Lector a Datos
                while (datos.Lector.Read())
                {
                    //Creo el objeto Tipo Elemnto aux y lo instancio
                    Elemento aux = new Elemento();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                //Cierro la conexion utilizando el metodo que heredo de AccesoDatos
                datos.cerrarConexion();
            }
        }
    }
}
