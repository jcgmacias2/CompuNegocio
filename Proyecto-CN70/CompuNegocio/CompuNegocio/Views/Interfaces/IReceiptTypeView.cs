using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IReceiptTypeView : IBaseView
    {
        event Action Save;
        event Action Quit;

        TiposDeComprobante ReceiptType { get; }
        bool Selected { get; set; }

        void Show(Series serie);
        void FillCombo(List<TiposDeComprobante> receiptTypes);
    }
}
