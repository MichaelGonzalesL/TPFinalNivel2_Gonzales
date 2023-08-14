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
using Dominio;
using NegocioDatos;


namespace Presentacion
{
    public partial class AltaArticulos : Form
    {
        private Articulos articulo = null;
        private OpenFileDialog archivo = null;
        
        private Ayuda ayuda = new Ayuda();
        private string direccion = @"C:\imagenes-articulos\";
        
        public AltaArticulos()
        {
            InitializeComponent();
        }
        public AltaArticulos(Articulos articuloModificar)
        {
            InitializeComponent();
            articulo = articuloModificar;
            Text = "Modificar Articulo";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AltaArticulos_Load(object sender, EventArgs e)//
        {
            btnAceptar.Enabled = false;
            try
            {
                MarcaNegocio marcaNegocio = new MarcaNegocio();
                CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

                cboMarca.DataSource = marcaNegocio.Listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.DataSource = categoriaNegocio.Listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";
                if (articulo != null)
                {
                    txtCodigo.Text = articulo.CodArticulo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    cboMarca.SelectedValue = articulo.Marca.Id;
                    cboCategoria.SelectedValue = articulo.Categoria.Id;
                    txtImagen.Text = articulo.Imagen;
                    txtPrecio.Text = articulo.Precio.ToString();
                    ayuda.cargarImagen(articulo.Imagen, pbxAltaArticulos);
                    btnAceptar.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticulosNegocio negocio = new ArticulosNegocio();

            try
            {
                if (articulo == null)
                    articulo = new Articulos();

                 articulo.CodArticulo = txtCodigo.Text;
                 articulo.Nombre = txtNombre.Text;
                 articulo.Descripcion = txtDescripcion.Text;
                 articulo.Marca = (Sello)cboMarca.SelectedItem;
                 articulo.Categoria = (Tipo)cboCategoria.SelectedItem;
                 articulo.Imagen = txtImagen.Text;
                 articulo.Precio = decimal.Parse(txtPrecio.Text);

                if (archivo != null && !(txtImagen.Text.ToUpper().Contains("HTTP")))
                {
                    if (!(File.Exists(direccion + (archivo.SafeFileName))))
                    {
                        File.Copy(archivo.FileName, direccion + archivo.SafeFileName);
                    }
                    articulo.Imagen = direccion + archivo.SafeFileName;
                }

                if (articulo.Id != 0)
                {
                    negocio.Modificar(articulo);
                    MessageBox.Show("El articulo fue modificado exitosamente");
                }
                else
                {
                    negocio.Agregar(articulo);
                    MessageBox.Show("El articulo fue agregado exitosamente");
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
       
        //EVENTOS 
        private void txtImagen_TextChanged(object sender, EventArgs e)
        {
            ayuda.cargarImagen(txtImagen.Text, pbxAltaArticulos);
        }

        private void btnAgragarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg|png|*.png";


            if (archivo.ShowDialog() == DialogResult.OK)
            {

                txtImagen.Text = archivo.FileName;

                if (!(Directory.Exists(direccion)))
                {

                    Directory.CreateDirectory(direccion);
                }
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            ayuda.soloDecimal(e,txtPrecio,ePvdrFiltro);
        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            ayuda.celdaVacia(txtCodigo,ePvdrFiltro);
            validacionCampos();
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            ayuda.celdaVacia(txtNombre,ePvdrFiltro);
            validacionCampos();
        }
        private void txtPrecio_TextChanged(object sender, EventArgs e)
        {
           ayuda.celdaVacia(txtPrecio,ePvdrFiltro);
           validacionCampos();
        }
        //FUNCIONES FILTRO
        private void validacionCampos()
        {
            if (string.IsNullOrEmpty(txtCodigo.Text)) { btnAceptar.Enabled = false; }
            else if (string.IsNullOrEmpty(txtNombre.Text)) { btnAceptar.Enabled = false; }
            else if (string.IsNullOrEmpty(txtPrecio.Text)) { btnAceptar.Enabled = false; }
            else { btnAceptar.Enabled = true; }
        }
    }
}
