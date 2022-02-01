using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class InvoiceNotePresenter
    {
        private IInvoiceNoteView _view;

        public InvoiceNotePresenter(IInvoiceNoteView view,DatosExtraPorFactura nota)
        {
            _view = view;

            _view.Quit += Quit;
            _view.Show(nota);
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
