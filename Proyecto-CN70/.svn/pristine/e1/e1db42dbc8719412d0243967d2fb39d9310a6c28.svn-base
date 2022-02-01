using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;

namespace Aprovi.Views
{
    public interface IBillsOfSaleSelectListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;
        event Action SelectAll;
        event Action DeselectAll;

        List<VMRemision> SelectedBillsOfSale { get; }
        List<VMRemision> BillsOfSale { get; }

        void Show(List<VMRemision> billsOfSale);
    }
}
