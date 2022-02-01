using Aprovi.Business.Services;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class ReceiptTypePresenter
    {
        private IReceiptTypeView _view;
        private ISerieService _series;

        public ReceiptTypePresenter(IReceiptTypeView view, ISerieService seriesService, ICatalogosEstaticosService catalogsService)
        {
            _view = view;
            _series = seriesService;

            _view.FillCombo(catalogsService.ListTiposDeComprobante());
            _view.Save += Save;
            _view.Quit += Quit;
        }

        private void Quit()
        {
            try
            {
                _view.Selected = false;
                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Save()
        {
            if (!_view.ReceiptType.isValid() || !_view.ReceiptType.idTipoDeComprobante.isValid())
            {
                _view.ShowError("Debe seleccionar el tipo de comprobante a relacionar");
                return;
            }

            try
            {
                _view.Selected = true;
                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
