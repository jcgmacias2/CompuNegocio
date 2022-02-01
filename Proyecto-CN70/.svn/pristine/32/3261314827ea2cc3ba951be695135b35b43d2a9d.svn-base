using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IItemsCodesForSuppliersView
    {
        event Action AddSupplierCode;
        event Action DeleteSupplierCode;
        event Action FindSupplier;
        event Action OpenSuppliersList;

        List<CodigosDeArticuloPorProveedor> SupplierCodes { get; }
        CodigosDeArticuloPorProveedor CurrentSupplierCode { get; }
        CodigosDeArticuloPorProveedor SelectedSupplierCode { get; }

        void ClearSupplierCode();
        void Show(CodigosDeArticuloPorProveedor code);
        void Show(List<CodigosDeArticuloPorProveedor> codes);

    }
}
