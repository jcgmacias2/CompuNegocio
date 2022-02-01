using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IItemsCustomsApplicationView : IBaseView
    {
        event Action Add;
        event Action Remove;
        event Action Save;

        List<VMPedimento> CustomsApplications { get; }

        VMPedimento Current { get; }

        VMPedimento Selected { get; }

        VMArticulo Item { get; }

        void Clear();

        void Show(List<VMPedimento> customsApplications);
    }
}
