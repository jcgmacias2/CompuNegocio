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
    public class StationsListPresenter : BaseListPresenter
    {
        private IStationsListView _view;
        private IEstacionService _stations;

        public StationsListPresenter(IStationsListView view, IEstacionService stationsService) : base(view)
        {
            _view = view;
            _stations = stationsService;

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
            List<Estacione> stations;

            try
            {
                if (_view.Parameter.isValid())
                    stations = _stations.WithDescriptionLike(_view.Parameter);
                else
                    stations = _stations.List();

                _view.Show(stations);

                if (stations.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
