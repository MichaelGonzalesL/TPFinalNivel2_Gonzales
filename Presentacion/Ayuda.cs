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

        public void Cargar(List<Articulos> listaArticulo,DataGridView dgvArticulos)///Sacar el pbx.Ver si no afecta a otros llamados
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            listaArticulo = negocio.Listar();
            dgvArticulos.DataSource = listaArticulo;
            //pbxArticulos.Load(listaArticulo[0].Imagen);
        }


    }
}
