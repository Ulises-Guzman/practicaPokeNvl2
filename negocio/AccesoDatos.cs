using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Declaro el SqlCliente
using System.Data.SqlClient;

namespace negocio
{
    public class AccesoDatos
    {
        //1-Declaro los atributos, osea los objetos, para realizar la conexion,
        //con alcance privado, de esta manera centralizo todos los alcances privados
        //de la conexion que utilice en distintas clases que realizan procesos
        //de la conexion a la DB.

        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        //Creo la propiedad del SqlDataReader para acceder al lector desde afuera.
        public SqlDataReader Lector
        {
            get { return lector; }
        }

        //Creo un constructor y le paso por parametro,
        //en la instancia del objeto SqlConnection, la cadena de la conexion.
        public AccesoDatos()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true");
            comando = new SqlCommand();

        }

        //Es posible crear una funcion en la cual realizar la Consulta SQL
        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        //Tambien es posible crear una funcion que realice la ejecucion de la lectura
        //ejecucion de la lectura: conexion, abrir la conexion, ejecutar el lector.
        //De esta manera esta encapsulada.
        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //creo un metodo que es para setear los parametros agregados
        //en la  segunda consulta de la clase AccesoDatos.
        //AddWithValue recibe como parametro (un string NombreDeParametro y un objet Valor)
        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        public void ejecutarAccion()
        {
            comando.Connection = conexion;

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //Creo una funcion que cierra la conexion.
        //Cierra si esta ejecutado el lector, tanto, si no.
        public void cerrarConexion()
        {
            if (lector != null)
                conexion.Close();
            conexion.Close();
        }
    }
}
