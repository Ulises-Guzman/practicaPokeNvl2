using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;
using System.Configuration;

namespace Pokemons
{
    public partial class frmAltaPokemon : Form
    {
        //Creo un atributo privado para cuando agrego un pokemon nuevo, pokemon, con Constructor vacio, sea nulo
        private Pokemon pokemon = null;
        private OpenFileDialog archivo = null;
        public frmAltaPokemon()
        {
            InitializeComponent();
        }

        //Duplico el constructor que contiene al InitialzeComponent
        //Lo modofico para que reciba un (Pokemon pokemon)
        public frmAltaPokemon(Pokemon pokemon)
        {
            InitializeComponent();
            //Hago refernecia al pokemon de atributo privado que es = null
            this.pokemon = pokemon;
            //Modifico el titulo de la ventana
            Text = "Modificar Pokemon";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Para cargar un Pokemon, Creo un objeto de tipo Pokemon
            //Para insetar los datos en la DB creo un objeto tipo PokemonNegocio
           // Pokemon poke = new Pokemon();
           //Voy a utilizar la variable pokemon que cree como atributo privado, pero cuidado que esta en null
            PokemonNegocio negocio = new PokemonNegocio();

            try
            {
                //si llegas hasta aca es porque toque el boton Agregar
                //Entonces creo un objeto pokemon del tipo pokemon
                if (pokemon == null)
                    pokemon = new Pokemon();

                //objeto cargado
                pokemon.Nombre = txtNombre.Text;
                pokemon.Descripcion = txtDescripcion.Text;
                //*
                pokemon.Numero = int.Parse(txtNumero.Text);
                pokemon.UrlImagen = txtUrlImagen.Text;
                //Capturar el valor de las listas desplegables
                pokemon.Tipo = (Elemento)cmbTipo.SelectedItem;
                pokemon.Debilidad = (Elemento)cmbDebilidad.SelectedItem;

                if (pokemon.Id != 0)
                {
                    negocio.modificar(pokemon);
                    MessageBox.Show("Modificado correctamente");
                }
                else
                {
                    //Necesito mandarlo a la DB
                    negocio.agregar(pokemon);
                    MessageBox.Show("Agregado correctamente");
                }

                //Guardo imagen si la levanto localmente
                if (archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);

                Close();

            }
            catch (Exception ex)
            {
                //Muestra el mensaje de error
                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAltaPokemon_Load(object sender, EventArgs e)
        {
            ElementoNegocio elementoNegocio = new ElementoNegocio();

            try
            {   
                cmbTipo.DataSource = elementoNegocio.listar();
                //Para precargar los desplegables utilizo "clave/valor"
                cmbTipo.ValueMember = "Id";
                cmbTipo.DisplayMember = "Descripcion";
                cmbDebilidad.DataSource = elementoNegocio.listar();
                cmbDebilidad.ValueMember = "Id";
                cmbDebilidad.DisplayMember = "Descripcion";

                //Precargo los datos de las cajas de texto cuando presiono el boton "Modificar"
                if (pokemon != null)
                {
                    txtNumero.Text = pokemon.Numero.ToString();
                    txtNombre.Text = pokemon.Nombre;
                    txtDescripcion.Text = pokemon.Descripcion;
                    txtUrlImagen.Text = pokemon.UrlImagen;
                    cargarImagen(pokemon.UrlImagen);

                    //Y para los desplegables
                    cmbTipo.SelectedValue = pokemon.Tipo.Id;
                    cmbDebilidad.SelectedValue = pokemon.Debilidad.Id;
                }
            }
            catch (Exception ex)
            {

               MessageBox.Show(ex.ToString());
            }
        }

        //En este evento ubico la llamada para cargar la imagen de la url del frmAltaPokemon
        //*Tengo que mapear ,la insercion de la cadena de url en el formulario "frmAltaPokemons"
        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }

        //Copio de otro de otro frm, "frmPokemons" el Metodo cargarImagen()
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

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            //Para levantar una imagen desde un ruta local
            //Creo el objeto tipo OpenFileDialog
            //Filtro el tipo de archivo "su extension"
            //ShowDialog() abre el cuadro de seleccion de archivo
            //DialogResult.OK valida la seleccioin del archivo
            //archivo.Filename captura la ruta y nombre del archivo
            //Para guardar localmente el archivo de imagen en un directorio
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg|png|*.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);

                //Guardar imagen
                //Leer la ruta desde el archivo de configuracion
                //Agregar la referencia
                //File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
            }
        }
    }
}
