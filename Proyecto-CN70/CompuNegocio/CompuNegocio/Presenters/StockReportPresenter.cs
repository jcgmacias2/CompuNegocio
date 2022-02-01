using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;

namespace Aprovi.Presenters
{
    public class StockReportPresenter
    {
        private IStockReportView _view;
        private IArticuloService _items;
        private readonly IClasificacionService _classifications;

        public StockReportPresenter(IStockReportView view, IArticuloService itemsService, IClasificacionService classifications)
        {
            _view = view;
            _items = itemsService;
            _classifications = classifications;

            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;
            _view.OpenClassificationsList += OpenClassificationsList;
            _view.AddClassification += AddClassification;
        }

        private void AddClassification()
        {
            try
            {
                List<Clasificacione> classifications = _view.SelectedClassifications;

                classifications.Add(_view.Classification);
                _view.Clear();

                _view.Show(classifications);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OpenClassificationsList()
        {
            try
            {
                IClassificationsListView view = new ClassificationsListView();
                ClassificationsListPresenter presenter = new ClassificationsListPresenter(view,_classifications);

                view.ShowWindow();

                if (view.Classification.isValid() && view.Classification.idClasificacion.isValid())
                {
                    _view.Show(view.Classification);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Print()
        {
            try
            {
                IReportViewerView view;
                ReportViewerPresenter presenter;

                view = new ReportViewerView(Reports.FillReport(_items.Stock(_view.SelectedClassifications),_view.SelectedClassifications, _view.OnlyWithStock));
                presenter = new ReportViewerPresenter(view);

                view.Print();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Preview()
        {
            try
            {
                IReportViewerView view;
                ReportViewerPresenter presenter;

                view = new ReportViewerView(Reports.FillReport(_items.Stock(_view.SelectedClassifications), _view.SelectedClassifications, _view.OnlyWithStock));
                presenter = new ReportViewerPresenter(view);

                view.Preview();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
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
