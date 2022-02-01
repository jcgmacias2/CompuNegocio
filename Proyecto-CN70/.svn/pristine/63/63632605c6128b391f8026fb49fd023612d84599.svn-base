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
    public class AddendaCalimaxPresenter
    {
        private IAddendaCalimaxView _view;
        private readonly ICatalogosEstaticosService _catalogos;

        public AddendaCalimaxPresenter(IAddendaCalimaxView view, ICatalogosEstaticosService catalogos)
        {
            _view = view;
            _catalogos = catalogos;

            _view.Close += Close;

            _view.FillCombos(_catalogos.ListDirectorio());
        }

        private void Close()
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
