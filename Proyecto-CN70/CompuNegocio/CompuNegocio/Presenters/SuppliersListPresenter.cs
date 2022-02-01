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
    public class SuppliersListPresenter : BaseListPresenter
    {
        private readonly ISuppliersListView _view;
        private IProveedorService _suppliers;

        public SuppliersListPresenter(ISuppliersListView view, IProveedorService suppliersService) :base(view)
        {
            _view = view;
            _suppliers = suppliersService;

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
            List<Proveedore> suppliers;

            try
            {
                if (_view.Parameter.isValid())
                    suppliers = _suppliers.WithCodeLike(_view.Parameter);
                else
                    suppliers = _suppliers.List();

                _view.Show(suppliers);

                if (suppliers.Count > 0)
                    _view.GoToRecord(0);

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
