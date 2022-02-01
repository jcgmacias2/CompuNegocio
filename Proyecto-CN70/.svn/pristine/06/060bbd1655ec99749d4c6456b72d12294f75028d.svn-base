using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Presenters
{
    public class CFDIUsesListPresenter : BaseListPresenter
    {
        private readonly ICFDIUsesListView _view;
        private IUsosCFDIService _uses;
        private bool _showOnlyActive;

        public CFDIUsesListPresenter(ICFDIUsesListView view, IUsosCFDIService uses, bool onlyShowActive)
            : base(view)
        {
            _view = view;
            _uses = uses;

            _showOnlyActive = onlyShowActive;
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
            List<UsosCFDI> uses;

            try
            {
                if (_view.Parameter.isValid())
                    uses = _uses.List(_view.Parameter);
                else
                    uses = _uses.List();

                if (_showOnlyActive)
                    uses = uses.Where(p => p.activo).ToList();

                _view.Show(uses);

                if (uses.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
