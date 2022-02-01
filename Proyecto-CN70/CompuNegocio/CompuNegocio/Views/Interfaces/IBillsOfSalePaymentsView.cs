using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IBillsOfSalePaymentsView
    {
        event Action Add;
        event Action Remove;

        AbonosDeRemision Payment { get; }
        AbonosDeRemision Selected { get; }

        void Show(List<AbonosDeRemision> payments);
    }
}
