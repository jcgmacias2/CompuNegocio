using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Aprovi.Views
{
    /// <summary>
    /// Esta clase es la clase base que será heredada en las View's para tener el comportamiento generico
    /// </summary>
    public class BaseListView : BaseView, IBaseListView
    {
        private DataGrid _grid;
        private TextBox _search;

        public BaseListView()
        {

        }

        #region Properties that allow the class that inherits the BaseListView make late assignment

        /// <summary>
        /// Control grid donde se encuentra la lista de busqueda
        /// </summary>
        protected DataGrid Grid { get { return _grid; } set { _grid = value; } }

        /// <summary>
        /// Control textbox donde se captura el criterio de busqueda
        /// </summary>
        protected TextBox SearchBox { get { return _search; } set { _search = value; } }

        #endregion

        #region Properties implemented in order to fulfill Interface contract

        public string Parameter
        {
            get { return _search.Text; }
        }

        public int CurrentRecord
        {
            get { return _grid.SelectedIndex; }
        }

        public int TotalRecords
        {
            get { return _grid.Items.Count; }
        }

        public void GoToRecord(int index)
        {
            _grid.SelectedIndex = index;
            if (index >= 0)
                _grid.ScrollIntoView(_grid.SelectedItem);
        }

        #endregion
    }
}
