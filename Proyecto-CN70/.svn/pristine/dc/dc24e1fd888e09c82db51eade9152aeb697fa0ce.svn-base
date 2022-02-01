using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IItemsCodesForCustomersView
    {
        event Action AddCustomerCode;
        event Action DeleteCustomerCode;
        event Action FindCustomer;
        event Action OpenCustomersList;

        List<CodigosDeArticuloPorCliente> CustomerCodes { get; }
        CodigosDeArticuloPorCliente CurrentCustomerCode { get; }
        CodigosDeArticuloPorCliente SelectedCustomerCode { get; }

        void ClearCustomerCode();
        void Show(CodigosDeArticuloPorCliente code);
        void Show(List<CodigosDeArticuloPorCliente> codes);

    }
}
