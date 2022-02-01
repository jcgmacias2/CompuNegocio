using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using System;
using System.Collections.Generic;


namespace Aprovi.Presenters
{
    public class AdjustmentsListPresenter : BaseListPresenter
    {
        private readonly IAdjustmentsListView _view;
        private IAjusteService _adjustments;

        public AdjustmentsListPresenter(IAdjustmentsListView view, IAjusteService adjustmentsService)
            : base(view)
        {
            _view = view;
            _adjustments = adjustmentsService;

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
            List<Ajuste> adjustments;

            try
            {
                if (_view.Parameter.isValid())
                    adjustments = _adjustments.Like(_view.Parameter);
                else
                    adjustments = _adjustments.List();

                _view.Show(adjustments);

                if (adjustments.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
