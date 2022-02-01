using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IFiscalPaymentsListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        VwListaParcialidade FiscalPayment { get; }

        void Show(List<VwListaParcialidade> payments);
    }
}
