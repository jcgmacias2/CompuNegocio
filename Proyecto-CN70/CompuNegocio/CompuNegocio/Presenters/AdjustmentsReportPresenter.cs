using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class AdjustmentsReportPresenter
    {
        private IAdjustmentsReportView _view;
        private IAjusteService _adjustments;
        private ICatalogosEstaticosService _catalogs;

        public AdjustmentsReportPresenter(IAdjustmentsReportView view, IAjusteService adjustments, ICatalogosEstaticosService catalogs)
        {
            _view = view;
            _adjustments = adjustments;
            _catalogs = catalogs;

            _view.Quit += Quit;
            _view.Preview += Preview;

            _view.Fill(_catalogs.ListTiposDeAjuste());
        }

        private void Preview()
        {
            if (!_view.Type.isValid())
            {
                _view.ShowError("Debe seleccionar el tipo de ajustes a reportear");
                return;
            }

            if (_view.Start > DateTime.Now)
            {
                _view.ShowError("La fecha de inicio no puede ser mayor al dia de hoy");
                return;
            }

            if (_view.End < _view.Start)
            {
                _view.ShowError("La fecha de fin no puede ser menor que la de inicio");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                view = new ReportViewerView(Reports.FillReport(_adjustments.List(_view.Start, _view.End, _view.Type), string.Format("De {0} del {1} al {2}", _view.Type.descripcion, _view.Start.ToShortDateString(), _view.End.ToShortDateString())));
                presenter = new ReportViewerPresenter(view);

                view.Preview();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Quit()
        {
            try
            {
                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
