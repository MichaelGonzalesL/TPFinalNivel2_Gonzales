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
        Ayuda ayuda = new Ayuda();
        private OpenFileDialog archivo = null;
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

        private void AltaArticulos_Load(object sender, EventArgs e)//revisar
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
               /* celdaVacia(txtCodigo);
                celdaVacia(txtNombre);
                celdaVacia(txtPrecio);*/
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
       
        /// FUNCIONES FILTRO /******************************************************************************
        private bool hayPunto(string precio)
        {
            int hay = 0;
            foreach (char caracter in precio)
            {
                if (caracter.ToString().Equals("."))
                {
                    hay++;
                }
            }

            if (hay==1) { return false; }
            else { return true; }
            
        } 
        private void soloDecimal(KeyPressEventArgs e)
        {

            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = false;

            }
            else if (char.IsSeparator(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if(e.KeyChar.ToString().Equals(".") && hayPunto(txtPrecio.Text))
            {               
                    e.Handled = false;             
            }
            else
            {
                e.Handled=true;
                ePvdrFiltro.SetError(txtPrecio, "Solo numeros ");
             
            }
           // celdaVacia(txtPrecio) ;
        }

        private bool celdaVacia(TextBox campo)
        {
            if (string.IsNullOrEmpty(campo.Text) || string.IsNullOrWhiteSpace(campo.Text))
            {
                ePvdrFiltro.SetError(campo,"Requiere completar ");
                return true;
            }
            else { 
                ePvdrFiltro.Clear();
                return false;
            }
        } 

        private void validacionCampos()
        {
            // btnAceptar.Enabled = true;
            //if(celdaVacia(txtCodigo)/* && celdaVacia(txtNombre) && celdaVacia(txtPrecio)*/) { btnAceptar.Enabled = false; }
            /*else if (celdaVacia(txtNombre)) { btnAceptar.Enabled = false; ; }
            else if (celdaVacia(txtPrecio)) { btnAceptar.Enabled = false; ; }
            else { btnAceptar.Enabled = true;}
            */

            if (string.IsNullOrEmpty(txtCodigo.Text)) { btnAceptar.Enabled = false; }
            else if (string.IsNullOrEmpty(txtNombre.Text)) { btnAceptar.Enabled = false;}
            else if (string.IsNullOrEmpty(txtPrecio.Text)) { btnAceptar.Enabled = false;}
            else { btnAceptar.Enabled = true; }
            /*var vr = !string.IsNullOrEmpty(txtCodigo.Text) && !string.IsNullOrEmpty(txtNombre.Text) && !string.IsNullOrEmpty(txtPrecio.Text); 
            btnAceptar.Enabled = vr;*/
        }
        ///EVENTOS 
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
            soloDecimal(e);
        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            celdaVacia(txtCodigo);
            validacionCampos();
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            celdaVacia(txtNombre);
            validacionCampos();
        }
        private void txtPrecio_TextChanged(object sender, EventArgs e)
        {
           celdaVacia(txtPrecio);
           validacionCampos();
        }
    }
}
