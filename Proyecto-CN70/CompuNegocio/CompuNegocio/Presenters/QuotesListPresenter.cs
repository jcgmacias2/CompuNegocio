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
    public class QuotesListPresenter: BaseListPresenter
    {
        private readonly IQuotesListView _view;
        private ICotizacionService _quotes;

        public QuotesListPresenter(IQuotesListView view, ICotizacionService quotes)
            : base(view)
        {
            _view = view;
            _quotes = quotes;

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
            List<Cotizacione> quotes;

            try
            {
                if (_view.Parameter.isValid())
                    quotes = _quotes.WithFolioOrClientLike(_view.Parameter);
                else
                    quotes = _quotes.List();

                _view.Show(quotes);

                if (quotes.Count > 0)
                    _view.GoToRecord(0);

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
