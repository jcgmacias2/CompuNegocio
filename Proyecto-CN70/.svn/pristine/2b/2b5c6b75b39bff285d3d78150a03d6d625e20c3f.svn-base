using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IItemsTaxesView
    {
        event Action AddTax;
        event Action DeleteTax;

        Impuesto SelectedTax { get; }

        void Show(List<Impuesto> taxes);
    }
}
