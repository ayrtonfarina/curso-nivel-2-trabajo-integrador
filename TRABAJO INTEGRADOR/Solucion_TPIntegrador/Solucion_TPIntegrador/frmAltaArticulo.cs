using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Solucion_TPIntegrador
{
    public partial class frmAltaArticulo : Form
    {
        private Articulo articulo = null;
        

        public frmAltaArticulo()
        {
            InitializeComponent();
        }

        public frmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo"; 
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                ptbArticulo.Load(imagen);   
            }
            catch (Exception ex)
            {

                ptbArticulo.Load("https://i0.wp.com/enfermeriacreativa.com/wp-content/uploads/2019/07/placeholder.png?ssl=1");
            }
        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio(); 
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            try
            {
                cboMarca.DataSource = marcaNegocio.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";

                cboCategoria.DataSource = categoriaNegocio.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if(articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtURLimagen.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);
                    txtPrecio.Text = articulo.Precio.ToString();    
                    cboMarca.SelectedValue = articulo.Marca.Id;
                    cboCategoria.SelectedValue = articulo.Categoria.Id; 
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //PARA QUE ME TIRE UN CARTEL POR CADA CASILLA EN BLANCO:
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("Falta completar el Codigo");
                txtCodigo.Focus();//"Focus()" es para que el cursor aparesca arriba de la casilla
                return; //el return para que me aparezca el cartel sino despues devuelve todo junto
            }
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Falta completar el Nombre");
                txtNombre.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("Falta completar la Descripcion");
                txtDescripcion.Focus();
                return;
            }
            //esto para el catel de las listas pero commo ya aparecen precargadas no es necesario
            //if (cboMarca.SelectedItem == null)
            //{
            //    MessageBox.Show("Falta selecionar Marca");
            //    return;
            //}
            //if (cboCategoria.SelectedItem == null)
            //{
            //    MessageBox.Show("Falta selecionar Categoria");
            //    return;
            //}
            if (string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("Falta completar el Precio");
                txtPrecio.Focus();
                return; 
            }
            if (!decimal.TryParse(txtPrecio.Text, out _))//esto para validar q sean numeros
            {
                MessageBox.Show("El precio debe ser un numero");
                txtPrecio.Focus();
                return; 
            }



            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (articulo == null)
                    articulo = new Articulo();


                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;   
                articulo.Descripcion = txtDescripcion.Text;
                articulo.ImagenUrl = txtURLimagen.Text; 
                articulo.Precio = decimal.Parse(txtPrecio.Text);
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;  

                if(articulo.Id != 0)
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("agregado exitosamente");   
                }


                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();    
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e) 
        {   //esto solo deja escribir numeros, coma, punto y borrar:
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',') 
            {
                e.Handled = true; 
            }
        }

        private void txtURLimagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtURLimagen.Text); 
        }


        //ESTO PARA PODER CARGAR UNA IMAGEN DESDE EL PC:
        //private void btnAgregarImagen_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog archivo = new OpenFileDialog(); //crear un objeto para cargar un archivo
        //    archivo.Filter = "jpg|*.jpg;|png|*.png"; //puedo poner un filtro de tipo de archivo
        //    if (archivo.ShowDialog() == DialogResult.OK) //confirmo si se seleciono un archivo
        //    {
        //        txtURLimagen.Text = archivo.FileName; //esto guarda la ruta del archivo
        //        cargarimagen(archivo.FileName); //con esto carga el archivo

        //        //GUARDAR LA IMAGEN EN UNA CARPETA:
        //        //File.Copy(archivo.FileName, ConfigurationManager.AppSettings["poke-app"] + archivo.SafeFileName); 



    }
        }

