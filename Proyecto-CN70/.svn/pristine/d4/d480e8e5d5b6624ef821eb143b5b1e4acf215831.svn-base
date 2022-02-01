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
    public class UnitsOfMeasureListPresenter : BaseListPresenter
    {
        private readonly IUnitsOfMeasureListView _view;
        private IUnidadDeMedidaService _units;

        public UnitsOfMeasureListPresenter(IUnitsOfMeasureListView view, IUnidadDeMedidaService unitsOfMeasureService) : base(view)
        {
            _view = view;
            _units = unitsOfMeasureService;

            _view.Search += Search;

            // Estos eventos estan implementados en la clase base BaseListPresenter
            _view.Select += Select;
            _view.Quit += Quit;
            _view.GoFirst += GoFirst;
            _view.GoPrevious += GoPrevious;
            _view.GoNext += GoNext;
            _view.GoLast += GoLast;
        }

        private void Search()
        {
            List<UnidadesDeMedida> units;

            try
            {
                if (_view.Parameter.isValid())
                    units = _units.Like(_view.Parameter);
                else
                    units = _units.List();

                _view.Show(units);

                if (units.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

    }
}
