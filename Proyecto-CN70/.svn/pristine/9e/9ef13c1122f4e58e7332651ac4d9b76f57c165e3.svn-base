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
    public class ItemsListPresenter : BaseListPresenter
    {
        private readonly IItemsListView _view;
        private IArticuloService _items;

        public ItemsListPresenter(IItemsListView view, IArticuloService itemsService) : base(view)
        {
            _view = view;
            _items = itemsService;

            _view.Search += Search;

            //Estos eventos estan implementados en la clase base BaseListPresenter
            _view.Select += Select;
            _view.Quit += Quit;
            _view.GoFirst += GoFirst;
            _view.GoPrevious += GoPrevious;
            _view.GoNext += GoNext;
            _view.GoLast += GoLast;
        }

        private void Search()
        {
            List<VMArticulo> items;

            try
            {
                if (_view.Parameter.isValid())
                    items = _items.GetItemsForListWithNameLike(_view.Parameter);
                else
                    items = _items.GetItemsForList();

                if (_view.OnlyActives)
                    items = items.Where(a => a.activo).ToList();

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
