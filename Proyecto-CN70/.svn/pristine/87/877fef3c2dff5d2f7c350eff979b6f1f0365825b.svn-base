using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;

namespace Aprovi.Views
{
    public interface ICollectableBalancesReportView : IBaseView
    {
        event Action Quit;
        event Action Preview;
        event Action Print;
        event Action OpenSellersList;
        event Action OpenCustomersList;
        event Action FindSeller;
        event Action FindCustomer;

        VMAntiguedadSaldos Report { get; }

        void Show(Usuario seller);
        void Show(Cliente customer);
    }
}
