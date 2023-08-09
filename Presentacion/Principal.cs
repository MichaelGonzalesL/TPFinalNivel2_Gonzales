using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NegocioDatos;


namespace Presentacion
{
    public partial class frmPrincipal : Form
    {
        private List<Articulos> listaArticulo;
        private Ayuda ayuda = new Ayuda();
        public frmPrincipal()
        {
            InitializeComponent();
        }
        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            //ArticulosNegocio negocio = new ArticulosNegocio();
            //ayuda.Cargar(listaArticulo, pbxArticulos, dgvArticulos);

            cboFiltro.Items.Add("Predeterminado");
            cboFiltro.Items.Add("Mayor precio");
            cboFiltro.Items.Add("Menor precio");
            cargar();
            //cargarImagen(listaArticulo[0].Imagen);
        }

        private void cargar()
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            try
            {
                listaArticulo = negocio.Listar();
                dgvArticulos.DataSource = listaArticulo;
                cboFiltro.SelectedIndex = 0;   
                ocultarColumnas();
                //dgvArticulos.Columns[0].HeaderText = "Codigo Articulo";    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
   
        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
          //Articulos seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
          //  ayuda.cargarImagen(seleccionado.Imagen, pbxArticulos);
            if (dgvArticulos.CurrentRow != null)
            {
                Articulos seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.Imagen);
            }
        }
        private void cargarImagen(string seleccionado)
        {
            try
            {
                pbxArticulos.Load(seleccionado);
            }
            catch (Exception)
            {
                pbxArticulos.Load("https://static.vecteezy.com/system/resources/previews/005/337/799/non_2x/icon-image-not-found-free-vector.jpg"); ;
            }
        }

        private void ocultarColumnas()
        {
                    dgvArticulos.Columns["Imagen"].Visible = false;
                    dgvArticulos.Columns["Id"].Visible = false;
        }

        private bool grillaVacia()
        {
            if (dgvArticulos.CurrentRow != null)
            {
                return true;
            }
            return false;
        }

        ///Eventos de los botones
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AltaArticulos alta = new AltaArticulos();
            alta.ShowDialog();
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (grillaVacia())
            {
                Articulos seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
                ArticulosNegocio negocio = new ArticulosNegocio();
                try
                {
                    DialogResult respuesta = MessageBox.Show("Seguro que desea eliminar este articulo", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                    if (respuesta == DialogResult.Yes)
                    {
                        negocio.Eliminar(seleccionado.Id);
                        MessageBox.Show("Articulo eliminado exitosamente");
                        cargar();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("No hay registros para eliminar");
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {

            if (grillaVacia())
            {
                Articulos seleccionado;
                seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
                AltaArticulos modificar = new AltaArticulos(seleccionado);
                modificar.ShowDialog();
                cargar();
            }
            else
            {
                MessageBox.Show("No hay registros para modicar");
                
            }
        }

    //FILTRO
        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulos> listaFiltrada;
            try { 
            
                if (txtFiltro.Text != "")
                {
                    listaFiltrada = listaArticulo.FindAll(x => x.Nombre.ToUpper().Contains(txtFiltro.Text.ToUpper()));
                }
                else
                {
                    listaFiltrada = listaArticulo;
                }
                dgvArticulos.DataSource = null;
                dgvArticulos.DataSource = listaFiltrada;
                ocultarColumnas();
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cboFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Articulos> listaNueva;
            //string opcion = cboFiltro.SelectedItem.ToString();
            try
            {
                if (cboFiltro.SelectedIndex == 1)
                {
                    listaNueva = listaArticulo.OrderByDescending(x => x.Precio).ToList();
                }
                else if (cboFiltro.SelectedIndex == 2)
                {
                    listaNueva = listaArticulo.OrderBy(x => x.Precio).ToList();
                }
                else
                {
                    listaNueva = listaArticulo;
                }
                 dgvArticulos.DataSource = null;
                 dgvArticulos.DataSource = listaNueva;
                 ocultarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        
    }
}
