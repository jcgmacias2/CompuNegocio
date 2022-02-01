using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;

namespace Aprovi.Presenters
{
    public class PaymentFormsPresenter
    {
        private readonly IPaymentFormsView _view;
        private IFormaPagoService _paymentForms;

        public PaymentFormsPresenter(IPaymentFormsView view, IFormaPagoService payments)
        {
            _view = view;
            _paymentForms = payments;

            _view.Find += Find;
            _view.New += New;
            _view.Deactivate += Delete;
            _view.Update += Update;
            _view.OpenList += OpenList;
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

        private void OpenList()
        {
            try
            {
                IPaymentFormsListView view;
                PaymentFormsListPresenter presenter;

                view = new PaymentFormsListView();
                presenter = new PaymentFormsListPresenter(view, _paymentForms, false);

                view.ShowWindow();

                //Si seleccionó alguno lo muestro
                if (view.PaymentForm.idFormaPago.isValid())
                    _view.Show(view.PaymentForm);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Update()
        {
            try
            {
                //Actualizo la forma de pago
                var paymentMethod = _view.PaymentForm;
                paymentMethod.activa = true;
                _paymentForms.Update(paymentMethod);

                //Envio mensaje al usuario
                _view.ShowMessage(string.Format("Forma de pago {0} actualizado exitosamente", _view.PaymentForm.descripcion));

                //Limpio la pantalla para una nueva
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Delete()
        {
            if(!_view.IsDirty)
            {
                _view.ShowError("No hay ningúna forma de pago seleccionada para eliminar");
                return;
            }

            try
            {
                //Solo inactiva
                _paymentForms.Delete(_view.PaymentForm);

                //Envio mensaje al usuario
                _view.ShowMessage("Forma de pago removido exitosamente");

                //Limpio la pantalla
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void New()
        {
            try
            {
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            if (!_view.PaymentForm.descripcion.isValid())
                return;

            try
            {
                var paymentForm = _paymentForms.Find(_view.PaymentForm.descripcion);

                if (paymentForm.isValid() && paymentForm.idFormaPago.isValid() && !paymentForm.activa)
                    _view.ShowMessage("Esta forma de pago esta marcado como inactivo, para reactivarlo solo de click en Guardar");

                if (!paymentForm.isValid())
                    paymentForm = new FormasPago() { descripcion = _view.PaymentForm.descripcion };

                _view.Show(paymentForm);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
