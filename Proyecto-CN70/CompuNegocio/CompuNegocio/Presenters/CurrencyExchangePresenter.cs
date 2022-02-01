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
    public class CurrencyExchangePresenter
    {
        ICurrencyExchangeView _view;
        ICatalogosEstaticosService _catalogs;

        public CurrencyExchangePresenter(ICurrencyExchangeView view, ICatalogosEstaticosService catalogs)
        {
            _catalogs = catalogs;
            _view = view;

            _view.Change += Change;
            _view.Quit += Quit;

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

        private void Change()
        {
            try
            {
                //Esta ventana solo se muestra si es una divisa distinta, pero de cualquier manera hay que validar
                //Si tiene una moneda distinta realizar el cambio de divisas y mostrarlo
                var payment = _view.Payment;

                //Primero le paso los datos originales a CambioDivisa
                var finalCurrency = _catalogs.ListMonedas().FirstOrDefault(m => !m.idMoneda.Equals(payment.idMoneda));
                payment.CambioDivisa = new CambioDivisa() { idMonedaDivisa = payment.idMoneda, Moneda = payment.Moneda, monto = payment.monto };
                //Ahora hago el cambio de moneda al abono original
                payment.monto = payment.monto.ToDocumentCurrency(payment.Moneda, finalCurrency, payment.tipoDeCambio).ToRoundedCurrency(payment.Moneda);
                payment.idMoneda = finalCurrency.idMoneda;
                payment.Moneda = finalCurrency;

                _view.Show(payment);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
