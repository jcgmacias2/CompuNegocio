using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    /// <summary>
    /// Esta clase contiene la implementación de los eventos básicos generales de una View que presenta una lista
    /// </summary>
    public class BaseListPresenter
    {
        private readonly IBaseListView _listView;

        /// <summary>
        /// Este BaseListPresenter necesita un IBaseListView sobre el cual actuar
        /// </summary>
        /// <param name="ListView">Interfaz que herede de IBaseListView y que será implementada por la Vista</param>
        public BaseListPresenter(IBaseListView ListView)
        {
            _listView = ListView;
        }

        public void Quit()
        {
            try
            {
                _listView.GoToRecord(-1); //Avoid selection since it's just closing
                _listView.CloseWindow();
            }
            catch (Exception ex)
            {
                _listView.ShowError(ex.Message);
            }
        }

        public void GoFirst()
        {
            try
            {
                _listView.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _listView.ShowError(ex.Message);
            }
        }

        public void GoNext()
        {
            try
            {
                if (_listView.CurrentRecord < _listView.TotalRecords)
                    _listView.GoToRecord(_listView.CurrentRecord + 1);
                else
                    _listView.GoToRecord(_listView.TotalRecords - 1);
            }
            catch (Exception ex)
            {
                _listView.ShowError(ex.Message);
            }
        }

        public void GoLast()
        {
            try
            {
                _listView.GoToRecord(_listView.TotalRecords - 1); // base 0 index
            }
            catch (Exception ex)
            {
                _listView.ShowError(ex.Message);
            }
        }

        public void GoPrevious()
        {
            try
            {
                if (_listView.CurrentRecord > 0)
                    _listView.GoToRecord(_listView.CurrentRecord - 1);
                else
                    _listView.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _listView.ShowError(ex.Message);
            }
        }

        public void Select()
        {
            try
            {
                if (_listView.CurrentRecord < 0)
                {
                    _listView.ShowMessage("No hay ningún registro seleccionado");
                }

                _listView.CloseWindow();
            }
            catch (Exception ex)
            {
                _listView.ShowError(ex.Message);
            }
        }

    }
}
