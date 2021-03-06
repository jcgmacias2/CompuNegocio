using Aprovi.Business.Services;
using Aprovi.Application.ViewModels;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;

namespace Aprovi.Presenters
{
    public class ItemSelectionPresenter : BaseListPresenter
    {
        private readonly IItemSelectionView _view;
        private List<VMArticulo> _items;

        public ItemSelectionPresenter(IItemSelectionView view, List<VMArticulo> items) : base(view)
        {
            _view = view;

            _view.Search += Search;

            //Estos eventos estan implementados en la clase base BaseListPresenter
            _view.Select += Select;
            _view.Quit += Quit;
            _view.GoFirst += GoFirst;
            _view.GoPrevious += GoPrevious;
            _view.GoNext += GoNext;
            _view.GoLast += GoLast;

            _items = items;
        }

        private void Search()
        {
            List<VMArticulo> items;

            try
            {
                if (_view.Parameter.isValid())
                    items = _items.Where(x => x.descripcion.ToLower().Contains(_view.Parameter.ToLower())).ToList();
                else
                    items = _items;

                _view.Show(items);

                if (items.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
