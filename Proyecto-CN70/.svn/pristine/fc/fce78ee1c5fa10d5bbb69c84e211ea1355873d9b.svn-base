using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IAssociatedCompaniesView : IBaseView
    {
        event Action Find;
        event Action FindCompany;
        event Action OpenList;
        event Action OpenCompaniesList;
        event Action Quit;
        event Action Save;
        event Action Update;
        event Action Delete;
        event Action New;

        EmpresasAsociada AssociatedCompany { get; }

        void Clear();
        void Show(EmpresasAsociada associatedCompany);
    }
}
