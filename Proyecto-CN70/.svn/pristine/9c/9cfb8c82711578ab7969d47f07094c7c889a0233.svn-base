using Aprovi.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;

namespace Aprovi.Views
{
    public interface IOrdersReportView : IBaseView
    {
        event Action FindOrder;
        event Action FindCustomer;
        event Action OpenOrdersList;
        event Action OpenCustomersList;
        event Action Quit;
        event Action Preview;
        event Action Print;

        VMPedido Order { get; }
        Cliente Customer { get; }
        Reportes_Pedidos ReportType { get; }

        void Show(VMPedido order);
        void Show(Cliente customer);
    }
}
