using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class TaxesListPresenter : BaseListPresenter
    {
        private readonly ITaxesListView _view;
        private IImpuestoService _taxes;
        private bool _showInactive;

        public TaxesListPresenter(ITaxesListView view, IImpuestoService taxesService, bool showInactive) : base(view)
        {
            _view = view;
            _taxes = taxesService;
            _showInactive = showInactive;

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
            List<Impuesto> taxes;

            try
            {
                if (_view.Parameter.isValid())
                    taxes = _taxes.WithNameLike(_view.Parameter);
                else
                    taxes = _taxes.List(_showInactive);

                _view.Show(taxes);

                if (taxes.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
