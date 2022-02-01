using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IAssociatedCompaniesListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        EmpresasAsociada AssociatedCompany { get; }

        void Show(List<EmpresasAsociada> associatedCompanies);
    }
}
