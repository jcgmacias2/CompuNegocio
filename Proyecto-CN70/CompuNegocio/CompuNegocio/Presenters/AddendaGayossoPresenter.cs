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
    public class AddendaGayossoPresenter
    {
        private IAddendaGayossoView _view;

        public AddendaGayossoPresenter(IAddendaGayossoView view)
        {
            _view = view;

            _view.Close += Close;
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
