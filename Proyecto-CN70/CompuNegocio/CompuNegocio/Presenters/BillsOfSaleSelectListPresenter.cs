using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;

namespace Aprovi.Presenters
{
    public class BillsOfSaleSelectListPresenter: BaseListPresenter
    {
        private readonly IBillsOfSaleSelectListView _view;
        private IRemisionService _billsOfSale;

        public BillsOfSaleSelectListPresenter(IBillsOfSaleSelectListView view, IRemisionService billsOfSale)
            : base(view)
        {
            _view = view;
            _billsOfSale = billsOfSale;

            _view.Search += Search;
            _view.SelectAll += ViewOnSelectAll;
            _view.DeselectAll += ViewOnDeselectAll;
            _view.SearchDate += SearchDate;

            //Estos eventos estan implementados en la clase base BaseListPresenter
            _view.Select += Select;
            _view.Quit += Quit;
            _view.GoFirst += GoFirst;
            _view.GoPrevious += GoPrevious;
            _view.GoNext += GoNext;
            _view.GoLast += GoLast;
        }

        private void ViewOnDeselectAll()
        {
            try
            {
                List<VMRemision> billsOfSale = _view.BillsOfSale;

                billsOfSale.ForEach(x => x.Selected = false);

                _view.Show(billsOfSale);
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void ViewOnSelectAll()
        {
            try
            {
                List<VMRemision> billsOfSale = _view.BillsOfSale;

                billsOfSale.ForEach(x => x.Selected = true);

                _view.Show(billsOfSale);
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Search()
        {
            List<VwResumenPorRemision> billsOfSale;

            try
            {
                if (_view.Parameter.isValid())
                    billsOfSale = _billsOfSale.ActiveWithFolioOrClientLike(_view.Parameter);
                else
                    billsOfSale = _billsOfSale.ListActive();

                _view.Show(billsOfSale.Select(bos => new VMRemision(bos)).ToList());

                if (billsOfSale.Count > 0)
                    _view.GoToRecord(0);

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void SearchDate()
        {
            List<VwResumenPorRemision> billsOfSale;

            try
            {

                if (_view.Start > DateTime.Now)
                {
                    _view.ShowError("La fecha de inicio no puede ser mayor al dia de hoy");
                    return;
                }

                if (_view.End < _view.Start)
                {
                    _view.ShowError("La fecha de fin no puede ser menor que la de inicio");
                    return;
                }

                billsOfSale = _billsOfSale.ActiveWithDateLike(_view.Start, _view.End);

                _view.Show(billsOfSale.Select(bos => new VMRemision(bos)).ToList());

                if (billsOfSale.Count > 0)
                    _view.GoToRecord(0);

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
