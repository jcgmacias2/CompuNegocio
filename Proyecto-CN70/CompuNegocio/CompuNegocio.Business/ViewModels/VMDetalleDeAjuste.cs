using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMDetalleDeAjuste
    {
        public VMDetalleDeAjuste() { }

        public VMDetalleDeAjuste(string folio, string codigo, string descripcion, decimal cantidad, DateTime fecha, string usuario)
        {
            Folio = folio;
            Codigo = codigo;
            Descripcion = descripcion;
            Cantidad = cantidad;
            Fecha = fecha;
            Usuario = usuario;
        }

        public string Folio { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
    }
}
