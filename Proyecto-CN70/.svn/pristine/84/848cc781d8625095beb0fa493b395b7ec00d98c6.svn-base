using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    /// <summary>
    /// Esta interfaz debe ser implementada por un BaseListView que herede de BaseView
    /// </summary>
    public interface IBaseListView : IBaseView
    {

        #region General properties every ListView View must implement

        string Parameter { get; }
        int CurrentRecord { get; }
        int TotalRecords { get; }

        #endregion

        #region General method every ListView View must implement

        void GoToRecord(int index);

        #endregion
    }
}
