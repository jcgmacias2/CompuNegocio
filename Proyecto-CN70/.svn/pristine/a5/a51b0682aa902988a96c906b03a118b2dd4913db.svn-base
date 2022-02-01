using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Aprovi.Presenters
{
    public class AssociatedCompaniesListPresenter : BaseListPresenter
    {
        private readonly IAssociatedCompaniesListView _view;
        private IEmpresaAsociadaService _associatedCompanies;

        public AssociatedCompaniesListPresenter(IAssociatedCompaniesListView view, IEmpresaAsociadaService associatedCompanies)
            : base(view)
        {
            _view = view;
            _associatedCompanies = associatedCompanies;

            _view.Search += Search;

            //Estos eventos estan implementados en la clase base BaseListPresenter
            _view.Select += Select;
            _view.Quit += Quit;
            _view.GoFirst += GoFirst;
            _view.GoPrevious += GoPrevious;
            _view.GoNext += GoNext;
            _view.GoLast += GoLast;
        }

        private void Search()
        {
            List<EmpresasAsociada> associatedCompanies;

            try
            {
                if (_view.Parameter.isValid())
                    associatedCompanies = _associatedCompanies.List(_view.Parameter);
                else
                    associatedCompanies = _associatedCompanies.List();

                _view.Show(associatedCompanies);

                if (associatedCompanies.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

    }
}
