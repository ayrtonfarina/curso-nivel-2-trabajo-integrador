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
    public partial class frmArticulos : Form
    {
        private List<Articulo> listaArticulo;
        public frmArticulos()
        {
            InitializeComponent();
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception)
            {
                pbxArticulo.Load("https://i0.wp.com/enfermeriacreativa.com/wp-content/uploads/2019/07/placeholder.png?ssl=1");
            }
        }

        private void frmArticulos_Load(object sender, EventArgs e)
        {
            cargar();

            cboCampo.Items.Add("Codigo");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Categoria");
            cboCampo.Items.Add("Precio");
        }

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulo = negocio.listar();
                dgvArticulos.DataSource = listaArticulo;

                dgvArticulos.Columns["ImagenUrl"].Visible = false;
                dgvArticulos.Columns["Id"].Visible = false;
                dgvArticulos.RowHeadersVisible = false; //asi se borra la primera comuna que tiene la flechita



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo alta = new frmAltaArticulo();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

            frmAltaArticulo modificar = new frmAltaArticulo(seleccionado);
            modificar.ShowDialog();  //el "showdialog" es para que no pueda hacer click fuera de la ventana abierta
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;

            try
            {
                DialogResult respuesta = MessageBox.Show("seguro que desea eliminar este articulo?", "Eliminado", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.Id);
                    cargar();
                }
            }


            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private bool validarFiltro()
        {
            //para saber si el cbo esta cargado, pregunto:
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("por favor, seleccione el campo para filtrar");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("por favor, seleccione el criterio para filtrar");
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtFiltro.Text))
                {
                    MessageBox.Show("Debes cargar el filtro numerico...");
                    return true;
                }

                if (!(soloNumeros(txtFiltro.Text)))
                {
                    MessageBox.Show("Solo numeros para filtrar por una campo numerico...");
                    return true;
                }
            }
            return false;   
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (validarFiltro())
                    return;

                string Campo = cboCampo.SelectedItem.ToString();
                string Criterio = cboCriterio.SelectedItem.ToString();  
                string filtro = txtFiltro.Text;
                dgvArticulos.DataSource = negocio.Filtrar(Campo,  Criterio, filtro) ;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "Precio")    //si el primer filtro es == Precio, el segundo filtro:
            {
                cboCriterio.Items.Clear();              
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
                cboCriterio.SelectedIndex = 0;
            }
            else //si no es ==Precio, el segundo filtro:
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
                cboCriterio.SelectedIndex = 0;
            }
        }

        private void btnVerDetalle_Click(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                VerDetalle detalle = new VerDetalle(seleccionado);
                detalle.ShowDialog();  //el "showdialog" es para que no pueda hacer click fuera de la ventana abierta
                
            }
            else
            {
                MessageBox.Show("seleccione un articulo"); 
            }
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

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listafiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length >= 3)
            {
                listafiltrada = listaArticulo.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()));  
            }
            else
            {
                listafiltrada = listaArticulo; 
            }

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listafiltrada;
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }


        //para que los botones siempre esten 10 pixeles abajo de la grilla
        private void frmArticulos_Resize(object sender, EventArgs e)
        {
            int espacio = 10; 
            btnAgregar.Top = dgvArticulos.Bottom + espacio;
            btnModificar.Top = dgvArticulos.Bottom + espacio;
            btnVerDetalle.Top = dgvArticulos.Bottom + espacio;
            btnEliminar.Top = dgvArticulos.Bottom + espacio;

        }
    }
}


