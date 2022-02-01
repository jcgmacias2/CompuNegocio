using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMInventario
    {
        public VMInventario() { }

        public VMInventario(Articulo articulo, VwExistencia existencia)
        {
            IdArticulo = articulo.idArticulo;
            Codigo = articulo.codigo;
            Descripcion = articulo.descripcion;
            Existencia = existencia.existencia.GetValueOrDefault();
            UnidadDeMedida = articulo.UnidadesDeMedida.descripcion;
        }

        public VMInventario(VwReporteInventarioFisico vw)
        {
            IdArticulo = vw.idArticulo;
            Codigo = vw.codigo;
            Descripcion = vw.descripcion;
            Existencia = vw.existencia.GetValueOrDefault(0m);
            UnidadDeMedida = vw.unidadMedida;
        }

        public int IdArticulo { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Existencia { get; set; }
        public string UnidadDeMedida { get; set; }
    }
}
