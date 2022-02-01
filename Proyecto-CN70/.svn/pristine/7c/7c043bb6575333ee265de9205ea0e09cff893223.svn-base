using Aprovi.Business.Services;
using Aprovi.Business.ViewModels;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class PricesCheckPresenter : BaseListPresenter
    {
        private IPricesCheckView _view;
        private IArticuloService _items;

        public PricesCheckPresenter(IPricesCheckView view, IArticuloService items) : base(view)
        {
            _view = view;
            _items = items;

            _view.Load += Load;
            _view.Search += Search;

            //Estos eventos estan implementados en la clase base BaseListPresenter
            _view.Select += Select;
            _view.Quit += Quit;
            _view.GoFirst += GoFirst;
            _view.GoPrevious += GoPrevious;
            _view.GoNext += GoNext;
            _view.GoLast += GoLast;
        }

        private void Load()
        {
            try
            {
                if (!_view.Client.isValid()) //Si no hay ningún cliente se va con el precio default
                    _view.Show(_items.List().ToViewModelList());
                else
                    _view.Show(_items.List().ToViewModelList(_view.Client));
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Search()
        {
            try
            {
                var items = _view.items;

                if (_view.Parameter.isValid())
                    _view.GoToRecord(0);
                else
                    _view.GoToRecord(items.FindIndex(a => a.codigo.Contains(_view.Parameter)));
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
