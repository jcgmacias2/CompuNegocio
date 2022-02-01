using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using System;
using System.Collections.Generic;

namespace Aprovi.Presenters
{
    public class BanksListPresenter : BaseListPresenter
    {
        private readonly IBanksListView _view;
        private IBancoService _banks;

        public BanksListPresenter(IBanksListView view, IBancoService banksService)
            : base(view)
        {
            _view = view;
            _banks = banksService;

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
            List<Banco> banks;

            try
            {
                if (_view.Parameter.isValid())
                    banks = _banks.List(_view.Parameter);
                else
                    banks = _banks.List();

                _view.Show(banks);

                if (banks.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
