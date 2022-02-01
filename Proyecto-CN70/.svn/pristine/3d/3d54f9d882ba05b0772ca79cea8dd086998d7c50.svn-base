using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Aprovi.Presenters
{
    public class NotePresenter
    {
        private INoteView _view;

        public NotePresenter(INoteView view, string nota)
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
