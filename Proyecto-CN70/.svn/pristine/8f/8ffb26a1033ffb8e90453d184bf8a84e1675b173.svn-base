using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Application.ViewModels;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;

namespace Aprovi.Presenters
{
    public class PurchasePaymentsPresenter
    {
        private readonly IPurchasePaymentsView _view;
        private IAbonoDeCompraService _payments;
        private ICompraService _purchases;

        public PurchasePaymentsPresenter(IPurchasePaymentsView view, IAbonoDeCompraService purchasePaymentsService, ICatalogosEstaticosService catalogsService, IFormaPagoService payments, ICompraService purchasesService)
        {
            _view = view;
            _payments = purchasePaymentsService;
            _purchases = purchasesService;


            _view.OpenPurchasesList += OpenPurchasesList;
            _view.AddPayment += AddPayment;
            _view.Quit += Quit;
            _view.CancelPayment += CancelPayment;

            //Pongo el siguiente folio
            _view.Clear(_payments.GetNextFolio());
            _view.FillCombos(catalogsService.ListMonedas(), payments.List());
        }

        private void CancelPayment()
        {
            if(_view.IsPurchaseDirty)
            {
                if (!_view.CurrentPayment.idAbonoDeCompra.isValid())
                {
                    _view.ShowError("No existe ningún abono seleccionado para edición");
                    return;
                }
            }

            try
            {
                //Cancelo el abono si la compra ya esta registrada
                if(_view.IsPurchaseDirty)
                    _payments.Cancel(_view.CurrentPayment);

                //Elimino el abono de la lista de abonos
                var purchase = _view.Purchase;
                purchase.AbonosDeCompras.Remove(_view.CurrentPayment);

                //Mando mensaje al usuario
                _view.ShowMessage(string.Format("Abono {0} cancelado exitosamente", _view.CurrentPayment.folio));

                //Actualizo la lista de abonos mostrandose
                _view.Show(purchase);
                
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

        private void AddPayment()
        {
            string error;

            if(!IsPaymentValid(_view.Payment,out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                //Tomo el pago y la compra para editarlos localmente
                var payment = _view.Payment;
                var purchase = _view.Purchase;

                //Válido que lo que vaya a abonar no sea mayor que el saldo pendiente
                if (payment.monto.ToDocumentCurrency(payment.Moneda, purchase.Moneda, payment.tipoDeCambio) > purchase.Saldo)
                {
                    _view.ShowError("No puede abonar más del saldo pendiente");
                    return;
                }

                //Para agregarlo le asigno el usuario que esta registrandolo
                payment.idUsuarioRegistro = Session.LoggedUser.idUsuario;

                //Si es una compra registrada mando el cambio hasta la base de datos
                if(_view.IsPurchaseDirty)
                    payment = _payments.Add(payment);
                else //Solo lo agrego el abono a la lista de abonos
                    purchase.AbonosDeCompras.Add(payment);

                //Limpio el espacio para capturar mas abonos
                _view.Clear(_payments.GetNextFolio());

                //Mando mensaje al usuario
                _view.ShowMessage("El abono ha sido registrado exitosamente");

                //Muestro la lista actualizada de abonos
                _view.Show(purchase);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenPurchasesList()
        {
            try
            {
                IPurchasesListView view;
                PurchasesListPresenter presenter;

                view = new PurchasesListView();
                presenter = new PurchasesListPresenter(view, _purchases);

                view.ShowWindow();

                if (view.Purchase.idCompra.isValid())
                    _view.Show(new VMCompra(view.Purchase));
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private bool IsPaymentValid(AbonosDeCompra payment, out string error)
        {
            if(!payment.folio.isValid())
            {
                error = "El folio no es válido";
                return false;
            }

            if(!payment.monto.isValid())
            {
                error = "El monto del abono no es válido";
                return false;
            }

            if(!payment.idMoneda.isValid())
            {
                error = "La moneda no es válida";
                return false;
            }

            if(!payment.idFormaPago.isValid())
            {
                error = "La forma de pago no es válida";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }
}
