using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IClientsItemsCodesView
    {
        event Action AddItemCode;
        event Action DeleteItemCode;
        event Action FindItem;
        event Action OpenItemsList;

        List<CodigosDeArticuloPorCliente> ItemCodes { get; }
        CodigosDeArticuloPorCliente CurrentItemCode { get; }
        CodigosDeArticuloPorCliente SelectedItemCode { get; }

        void ClearItemCode();
        void Show(CodigosDeArticuloPorCliente code);
        void Show(List<CodigosDeArticuloPorCliente> codes);

    }
}
