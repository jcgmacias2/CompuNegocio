using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class PricesListPresenter : BaseListPresenter
    {
        private readonly IPricesListView _view;
        private IListaDePrecioService _pricesLists;

        public PricesListPresenter(IPricesListView view, IListaDePrecioService pricesListsService):base(view)
        {
            _view = view;
            _pricesLists = pricesListsService;

            _view.Search += Search;
            _view.FillCombo(Enum.GetValues(typeof(TipoDeBusqueda)).Cast<TipoDeBusqueda>().ToList());

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
            List<ListasDePrecio> pricesLists;

            try
            {
                if (_view.Parameter.isValid())
                {
                    switch (_view.SearchType)
                    {
                        case TipoDeBusqueda.Lista_De_Precios:
                            pricesLists = _pricesLists.WithCodeLike(_view.Parameter);
                            break;
                        case TipoDeBusqueda.Artículos:
                            pricesLists = _pricesLists.WithItemLike(_view.Parameter);
                            break;
                        case TipoDeBusqueda.Clientes:
                            pricesLists = _pricesLists.WithClientLike(_view.Parameter);
                            break;
                        default:
                            pricesLists = new List<ListasDePrecio>();
                            break;
                    }
                }
                else
                {
                    pricesLists = _pricesLists.List();
                }

                _view.Show(pricesLists);

                if(pricesLists.Count>0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
