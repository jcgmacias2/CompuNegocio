using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMFlujoPorArticulo
    {
        public VMFlujoPorArticulo(Articulo item)
        {
            IdArticulo = item.idArticulo;
            Codigo = item.codigo;
            Descripcion = item.descripcion;
            IdUnidadDeMedida = item.idUnidadDeMedida;
            UnidadDeMedida = item.UnidadesDeMedida.descripcion;
        }

        public int IdArticulo { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int IdUnidadDeMedida { get; set; }
        public string UnidadDeMedida { get; set; }
        public decimal EntradasAjuste { get; set; }
        public decimal EntradasCompras { get; set; }
        public decimal EntradasTotales { get; set; }
        public decimal SalidasFacturas { get; set; }
        public decimal SalidasAjustes { get; set; }
        public decimal SalidasVentas { get; set; }
        public decimal SalidasRemisiones { get; set; }
        public decimal SalidasTotales { get; set; }
        public decimal Existencia { get; set; }
    }
}
