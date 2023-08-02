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
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            try
            {

                cboMarca.DataSource = marcaNegocio.Listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.DataSource = categoriaNegocio.Listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";
                if (articulo!=null)
                {
                    txtCodigo.Text = articulo.CodArticulo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text= articulo.Descripcion;
                    cboMarca.SelectedValue = articulo.Marca.Id;
                    cboCategoria.SelectedValue = articulo.Categoria.Id;
                    txtImagen.Text = articulo.Imagen;
                    txtPrecio.Text = articulo.Precio.ToString();
                    ayuda.cargarImagen(articulo.Imagen,pbxAltaArticulos);
                
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
                if (articulo==null)
                    articulo  = new Articulos();

                articulo.CodArticulo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Marca = (Sello)cboMarca.SelectedItem;
                articulo.Categoria = (Tipo)cboCategoria.SelectedItem;
                articulo.Imagen = txtImagen.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);

                if (archivo!=null && !(txtImagen.Text.ToUpper().Contains("HTTP")))
                {
                    if (!(File.Exists(direccion + (archivo.SafeFileName))))
                    {
                        File.Copy(archivo.FileName, direccion + archivo.SafeFileName);
                    }
                        articulo.Imagen=direccion + archivo.SafeFileName;               
                }

                if (articulo.Id !=0)
                {
                    negocio.Modificar(articulo);
                    MessageBox.Show("El articulo fue modificado exitosamente");
                }
                else
                {
                    MessageBox.Show(/*"El articulo fue agregado exitosamente"*/articulo.Imagen);
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
        private void txtImagen_TextChanged(object sender, EventArgs e)
        {
            ayuda.cargarImagen(txtImagen.Text, pbxAltaArticulos);
        }

        private void btnAgragarImagen_Click(object sender, EventArgs e)
        {
            archivo=new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg|png|*.png";
            
            
            if (archivo.ShowDialog()==DialogResult.OK)
            {

                txtImagen.Text = archivo.FileName;

                if (!(Directory.Exists(direccion)))
                {
                    //MessageBox.Show("Carpeta creada con exito");
                     // MessageBox.Show(archivo.FileName);

                    Directory.CreateDirectory(direccion);
                }

                //if (!(File.Exists(direccion + (archivo.SafeFileName))))
                //{
                //    File.Copy(archivo.FileName, direccion + archivo.SafeFileName);
                //}
                //else
                //{
                //    MessageBox.Show("Ya existe el archivo");
                //}
            }
        }
    }
}
