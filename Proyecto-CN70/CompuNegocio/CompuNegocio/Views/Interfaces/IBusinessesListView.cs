﻿using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IBusinessesListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        Empresa Business { get; }

        void Show(List<Empresa> businesses);
    }
}
