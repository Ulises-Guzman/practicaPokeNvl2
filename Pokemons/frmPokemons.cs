using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Agregar las librerias/clases: dominio, negocio
using dominio;
using negocio;
using Pokemons;

namespace winform_app
{
    public partial class frmPokemons : Form
    {
        //Crear una variable privada del Tipo Lista, un atributo, para manipular la lista
        //lo que le paso al DataSource. Hago un pasaje de variables.
        private List<Pokemon> listaPokemon;
        //private List<Elemento> listaElemento;

        public frmPokemons()
        {
            InitializeComponent();
        }

        private void frmPokemons_Load(object sender, EventArgs e)
        {
            //Llamo al Metodo cargar()
            cargar();
            //Cargo el comboBox de Campo:
            cmbCampo.Items.Add("Número");
            cmbCampo.Items.Add("Nombre");
            cmbCampo.Items.Add("Descripción");
        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            //Ajuste para cuando no haya fila que seleccionar sin que se rompa la ejecucion
            if(dgvPokemons.CurrentRow != null)
            {
                //Cuando se seleciona el cursor a la fila
                //Tomar el elemento selecionado en la grilla .CurrentRow
                //Castear el elemento a tipo objeto pokemon y asignarlo a una variable del tipo objeto Pokemon
                //Carga la imagen de cada fila al seleccionar, en el evento Load del PictureBox
                Pokemon seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                //pbxPokemon.Load(seleccionado.UrlImagen);
                //Realizar la carga de la imagen a travez de manejo de excepciones
                //Llamo a la funcion cargar imagen
                cargarImagen(seleccionado.UrlImagen);
            }    
        }


        //Creo un Metodo "cargar()" para que la grilla se actualice cuando se carga un nuevo Pokemon
        private void cargar()
        {
            //Invoco la lectura a la DB
            PokemonNegocio negocio = new PokemonNegocio();

            //Inserto el codigo en manejo de excepcion
            try
            {
                listaPokemon = negocio.listar();
                dgvPokemons.DataSource = listaPokemon;

                //ElementoNegocio negocio1 = new ElementoNegocio();
                //listaElemento = negocio1.listar();
                //dgvElemento.DataSource = listaElemento;

                //Llamo al Metodo ocultarColumnas()
                ocultarColumnas();
                //Cargo las imagenes de la lista en el PictureBox pbxPokemon,
                //leyendo desde el objeto tipo lista Pokemon, listaPokemon.
                //cargo la primer imagen de la lista.
                //pbxPokemon.Load(listaPokemon[0].UrlImagen);
                //Cargo la primer imagen a traves del Metodo cargarImagen
                cargarImagen(listaPokemon[0].UrlImagen);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        //Creo Metodo Ocultar columnas
        private void ocultarColumnas()
        {
            //Desactivo la Columna de la url
            //Desactivo la columna de Id
            dgvPokemons.Columns["UrlImagen"].Visible = false;
            dgvPokemons.Columns["Id"].Visible = false;
        }

        //Creo un Metodo donde realizo el manejo de excepciones
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxPokemon.Load(imagen);
            }
            catch (Exception)
            {
                pbxPokemon.Load("https://www.campana.gob.ar/wp-content/uploads/2022/05/placeholder-1.png");
            }
        }


        //Interactuo a traves del boton Agregar con el nuevo formulario
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaPokemon alta = new frmAltaPokemon();
            alta.ShowDialog();
            //Cuando agrego un pokemon llamo al Metodo cargar() para actualice la grilla
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Pokemon seleccionado;
            seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;

            //Voy a pasarle por parametro a travez de un cosntructor duplicado de la clase "frmAltaPokemon"


            //Es practicamente lo mismo que el metodo Agregar
            frmAltaPokemon modificar = new frmAltaPokemon(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void btnEliminarLogico_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }

        //Creo un Metodo para contener en el, la logica de eliminarFisico y eliminarLogico
        private void eliminar(bool logico = false)//esto se llama mandar un valor opcional, sin mandrle un parametro ya va a tener un valor false, por defecto.
        {
            PokemonNegocio negocio = new PokemonNegocio();
            Pokemon seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("Está seguro de Elimnar el Registro?", "Elimnar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;

                    if (logico)
                        negocio.eliminarLogico(seleccionado.Id);
                    else
                        negocio.eliminar(seleccionado.Id);
                    cargar();
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        //Validacion de campo, criterio, solo numeros, vacio o nulo
        private bool validarFiltro()
        {
            if (cmbCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, ingrese el valor de 'Campo' para filtrar");
                return true;
            }
            if (cmbCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, ingrese el valor de 'Criterio' para filtrar");
                return true;
            }
            if (cmbCampo.SelectedItem.ToString() == "Número")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Debes cargar filtro para numericos...");
                    return true;
                }
                if (!(soloNumeros(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Solo Numeros para filtrar por un campo numerico....");
                    return true;
                }
            }
            
            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            //El boton "Buscar" los utilizo para busqueda avanzada
            //Cargo los 3 campos de busqueda
            //Creo el ojbjeto Tipo PokemonNegocio para que me devuleva una lista
            PokemonNegocio negocio = new PokemonNegocio();

            try
            {   
                //Si es true, Corta le ejecucion
                if (validarFiltro())
                    return;

                string campo = cmbCampo.SelectedItem.ToString();
                string criterio = cmbCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;

                dgvPokemons.DataSource = negocio.filtrar(campo, criterio, filtro);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


            //--------------------------------------------------------------------------------------------------------------------------------------------------------
            //Creo la lista filtrada, la referencia del filtro es la palabra que este en el textBox
            //Creo un objeto lista sin instanciar, List <...>
            //Ahora la instacio con:
            //Usando los metodos del ojbeto tipo List que a su vez es tipo Colletion, es un conjuto de clases dentro del framework
            //Al metodo FindAll le tengo que pasar un parametro especial, "una expresion Lambda"
            //List<Pokemon> listaFiltrada;
            //string filtro = txtFiltro.Text;

            // if Para que cuando realice la busqueda en vacio, traiga todos los datos
            //if (filtro != "")
            //{
                //listaFiltrada = listaPokemon.FindAll(x => x.Nombre == filtro);
                //Ajuste para que busque por caaracteres cadena contenia y sin diferenciar de mayus o minus
                //listaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Tipo.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            //}
            //else
            //{
            //    listaFiltrada = listaPokemon;
            //}
            
            //Para actualizar la grilla

            //dgvPokemons.DataSource = null;
            //dgvPokemons.DataSource = listaFiltrada;
            //ocultarColumnas();
            //--------------------------------------------------------------------------------------------------------------------------------------------------------
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Pokemon> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length >= 3)
            {
                listaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Tipo.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaPokemon;
            }

            dgvPokemons.DataSource = null;
            dgvPokemons.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cmbCampo_SelectedIndexChanged(object sender, EventArgs e)
        {   
            //Aqui guardo el elemento seleccionado
            string opcion = cmbCampo.SelectedItem.ToString();

            if (opcion == "Número")
            {
                cmbCriterio.Items.Clear();
                cmbCriterio.Items.Add("Mayor a");
                cmbCriterio.Items.Add("Menor a");
                cmbCriterio.Items.Add("Igual a");
            }
            else
            {
                cmbCriterio.Items.Clear();
                cmbCriterio.Items.Add("Comienza con");
                cmbCriterio.Items.Add("Termina con");
                cmbCriterio.Items.Add("Contiene");
            }
        }
    }
}
