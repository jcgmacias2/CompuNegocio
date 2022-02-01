using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using System;
using System.Collections.Generic;

namespace Aprovi.Presenters
{
    public class BusinessesListPresenter : BaseListPresenter
    {
        private IBusinessesListView _view;
        private IEmpresaService _businesses;

        public BusinessesListPresenter(IBusinessesListView view, IEmpresaService businessesService) : base(view)
        {
            _view = view;
            _businesses = businessesService;

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
            List<Empresa> businesses;

            try
            {
                if (_view.Parameter.isValid())
                    businesses = _businesses.WithDescriptionLike(_view.Parameter);
                else
                    businesses = _businesses.List();

                _view.Show(businesses);

                if (businesses.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
