using NegocioDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;//Libreria que me permite utilizar las propiedades del form para traerlos a esta clase
using Dominio;


namespace Presentacion
{
    public class Ayuda
    {

        public void cargarImagen(string seleccionado,PictureBox pbxArticulos)
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

        public List<Articulos> Cargar(List<Articulos> listaArticulo,DataGridView dgvArticulos)///Sacar el pbx.Ver si no afecta a otros llamados
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            listaArticulo = negocio.Listar();
            dgvArticulos.DataSource = listaArticulo;
            return listaArticulo;
        }

        public bool grillaVacia(DataGridView grilla)
        {
            if (grilla.CurrentRow != null)
            {
                return true;
            }
            return false;
        }

        //FILTROS
        
        public void soloDecimal(KeyPressEventArgs e,TextBox precio,ErrorProvider error)
        {

            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = false;

            }
            else if (char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (e.KeyChar.ToString().Equals(".") && hayPunto(precio.Text))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                error.SetError(precio, "Solo numeros ");
            }
            //celdaVacia();
        }

        public bool celdaVacia(TextBox campo,ErrorProvider error)
        {
            if (string.IsNullOrEmpty(campo.Text) || string.IsNullOrWhiteSpace(campo.Text))
            {
                error.SetError(campo, "Requiere completar ");
                return true;
            }
            else
            {
                error.Clear();
                return false;
            }
        }
        public bool hayPunto(string precio)
        {
            int hay = 0;
            foreach (char caracter in precio)
            {
                if (caracter.ToString().Equals("."))
                {
                    hay++;
                }
            }

            if (hay == 1) { return false; }
            else { return true; }

        }

    }
}
