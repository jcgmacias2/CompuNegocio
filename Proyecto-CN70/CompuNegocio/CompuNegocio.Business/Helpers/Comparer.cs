using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Helpers
{
    public class CompareOnlyItem : IEqualityComparer<DetallesDeCompra>
    {
        public bool Equals(DetallesDeCompra x, DetallesDeCompra y)
        {
            return x.idArticulo == y.idArticulo;
        }

        public int GetHashCode(DetallesDeCompra d)
        {
            return string.Format("{0}{1}",d.idDetalleDeCompra,d.idArticulo).GetHashCode();
        }
    }
}
