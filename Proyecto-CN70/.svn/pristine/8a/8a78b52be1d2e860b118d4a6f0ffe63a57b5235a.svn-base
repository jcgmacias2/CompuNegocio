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
    public class ClassificationsListPresenter : BaseListPresenter
    {
        private readonly IClassificationsListView _view;
        private IClasificacionService _classifications;

        public ClassificationsListPresenter(IClassificationsListView view, IClasificacionService classificationsService)
            : base(view)
        {
            _view = view;
            _classifications = classificationsService;

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
            List<Clasificacione> classifications;

            try
            {
                if (_view.Parameter.isValid())
                    classifications = _classifications.WithNameLike(_view.Parameter);
                else
                    classifications = _classifications.List();

                _view.Show(classifications);

                if (classifications.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
