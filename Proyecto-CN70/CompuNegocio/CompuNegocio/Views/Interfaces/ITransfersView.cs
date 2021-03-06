using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface ITransfersView : IBaseView
    {
        event Action Find;
        event Action OpenList;
        event Action Load;
        event Action FindItem;
        event Action OpenItemsList;
        event Action AddItem;
        event Action RemoveItem;
        event Action SelectItem;
        event Action Quit;
        event Action Reject;
        event Action New;
        event Action Print;
        event Action Save;
        event Action Update;
        event Action Approve;
        event Action OpenDestinationAssociatedCompanyList;
        event Action FindDestinationAssociatedCompany;
        event Action LoadRemoteTransfer;

        VMTraspaso Transfer { get; }
        bool IsDirty { get; }
        bool IsBeingProcessed { get; }
        DetallesDeTraspaso CurrentItem { get; }
        DetallesDeTraspaso SelectedItem { get; }

        void Show(VMTraspaso transfer);
        void Show(DetallesDeTraspaso detail);
        void ShowStock(decimal stock);
        void ClearItem();
    }
}
