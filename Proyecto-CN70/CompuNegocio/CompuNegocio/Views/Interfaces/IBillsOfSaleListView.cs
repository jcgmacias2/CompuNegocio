﻿using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IBillsOfSaleListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        Remisione BillOfSale { get; }

        void Show(List<Remisione> billsOfSale);
    }
}
