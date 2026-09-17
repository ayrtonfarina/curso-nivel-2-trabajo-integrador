using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace Solucion_TPIntegrador
{
    public partial class VerDetalle : Form
    {
        private Articulo articulo;

        public VerDetalle(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
        }

        private void VerDetalle_Load(object sender, EventArgs e)
        {
            try
            {
                lblCodigo.Text = articulo.Codigo;
                lblNombre.Text = articulo.Nombre;
                lblMarca.Text = articulo.Marca.Descripcion;
                lblCategoria.Text = articulo.Categoria.Descripcion;
                lblPrecio.Text = "$ " + articulo.Precio.ToString();
                lblDescripcion.Text = articulo.Descripcion;

                cargarImagen(articulo.ImagenUrl);       

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxDetalle.Load(imagen);
            }
            catch (Exception ex)
            {

                pbxDetalle.Load("https://i0.wp.com/enfermeriacreativa.com/wp-content/uploads/2019/07/placeholder.png?ssl=1");
            }

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();   
        }
    }
}
