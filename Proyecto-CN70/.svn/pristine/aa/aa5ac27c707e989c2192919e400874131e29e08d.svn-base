using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using Aprovi.Application.Helpers;

namespace Aprovi.Presenters
{
    public class DiscountNotesPresenter
    {
        private IDiscountNotesView _view;
        private IClienteService _customers;
        private INotaDeDescuentoService _discountNotes;
        private ICatalogosEstaticosService _catalogs;
        private IConfiguracionService _config;

        public DiscountNotesPresenter(IDiscountNotesView view,IClienteService customers,INotaDeDescuentoService discountNotes, IConfiguracionService config, ICatalogosEstaticosService catalogos)
        {
            _view = view;
            _customers = customers;
            _catalogs = catalogos;
            _discountNotes = discountNotes;
            _config = config;

            _view.Find += Find;
            _view.FindCustomer += FindCustomer;
            _view.New += New;
            _view.OpenList += OpenList;
            _view.OpenCustomersList += OpenCustomersList;
            _view.Cancel += Cancel;
            _view.Update += Update;
            _view.Save += Save;
            _view.Quit += Quit;
            _view.Load += Load;
            _view.AmountChanged += ViewOnAmountChanged;
            _view.Print += Print;

            _view.FillCombos(_catalogs.ListMonedas());
        }

        private void Print()
        {
            try
            {
                if (!_view.DiscountNote.isValid() || !_view.DiscountNote.idNotaDeDescuento.isValid())
                {
                    _view.ShowMessage("No es posible imprimir una nota de crédito que no ha sido registrada");
                    return;
                }

                try
                {
                    IDiscountNotePrintView view;
                    DiscountNotePrintPresenter presenter;

                    view = new DiscountNotePrintView(_view.DiscountNote);
                    presenter = new DiscountNotePrintPresenter(view, _discountNotes);

                    view.ShowWindow();

                    //Inicializo nuevamente
                    Load();
                }
                catch (Exception ex)
                {
                    _view.ShowError(ex.Message);
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void ViewOnAmountChanged()
        {
            try
            {
                var vm = _view.DiscountNote;

                vm.descripcion = string.Format("Descuento por {0}", vm.monto.ToDecimalString());

                _view.Show(vm);
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Load()
        {
            try
            {
                _view.Show(GetDefault());
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenCustomersList()
        {
            try
            {
                var vm = _view.DiscountNote;
                IClientsListView view;
                ClientsListPresenter presenter;

                view = new ClientsListView();
                presenter = new ClientsListPresenter(view, _customers);

                view.ShowWindow();

                if (!view.Client.isValid() || !view.Client.idCliente.isValid())
                {
                    _view.ShowError("El cliente seleccionado no es válido");

                    vm.Cliente = new Cliente();
                    _view.Show(vm);

                    return;
                }

                vm.Cliente = view.Client;
                _view.Show(vm);
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void FindCustomer()
        {
            try
            {
                var vm = _view.DiscountNote;

                var customer = _customers.Find(vm.Cliente.codigo);

                if (!customer.isValid() || !customer.idCliente.isValid())
                {
                    _view.ShowError("No existe un cliente con el código proporcionado");
                    vm.Cliente = new Cliente();
                    _view.Show(vm);
                    return;
                }

                vm.Cliente = customer;
                _view.Show(vm);
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
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

        private void Save()
        {
            try
            {
                string error;

                var discount = _view.DiscountNote;
                var config = Session.Configuration;

                discount.idUsuarioRegistro = Session.LoggedUser.idUsuario;
                discount.idEmpresa = config.Estacion.idEmpresa;


                if (!IsDiscountValid(discount, out error))
                {
                    _view.ShowError(error);
                    return;
                }

                _discountNotes.Add(discount);

                _view.ShowMessage("Nota de descuento {0} registrada exitosamente", _view.DiscountNote.folio);

                New();
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
                var discountNote = _discountNotes.Find(_view.DiscountNote.idNotaDeDescuento);

                if (discountNote.idFactura.HasValue || discountNote.idNotaDeCredito.HasValue)
                    throw new Exception("No es posible modificar una nota de descuento ya asociada");

                _discountNotes.Update(_view.DiscountNote);

                _view.ShowMessage("Nota de descuento {0} actualizada exitosamente", _view.DiscountNote.folio);

                New();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Cancel()
        {
            try
            {
                var discountNote = _discountNotes.Find(_view.DiscountNote.idNotaDeDescuento);

                if (discountNote.idFactura.HasValue || discountNote.idNotaDeCredito.HasValue)
                    throw new Exception("No es posible modificar una nota de descuento ya asociada");

                if (!_view.DiscountNote.idNotaDeDescuento.isValid())
                {
                    _view.ShowError("No se ha seleccionado una nota de descuento a eliminar");
                    return;
                }

                //Se solicita el motivo de la cancelacion
                ICancellationView view;
                CancellationPresenter presenter;

                view = new CancellationView();
                presenter = new CancellationPresenter(view);

                view.ShowWindow();

                _discountNotes.Cancel(_view.DiscountNote.idNotaDeDescuento, view.Reason);

                _view.ShowMessage("Nota de descuento eliminada exitosamente");
                _view.Clear();
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
                IDiscountNotesListView view;
                DiscountNotesListPresenter presenter;

                view = new DiscountNotesListView();
                presenter = new DiscountNotesListPresenter(view, _discountNotes);

                view.ShowWindow();

                if (view.DiscountNote.isValid() && view.DiscountNote.idNotaDeDescuento.isValid())
                    _view.Show(view.DiscountNote);
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
                _view.Show(GetDefault());
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            try
            {
                if (!_view.DiscountNote.folio.isValid())
                    return;

                var discount = _discountNotes.FindByFolio(_view.DiscountNote.folio);

                if (discount.isValid())
                    _view.Show(discount);
                else
                    _view.Show(new NotasDeDescuento() { folio = _view.DiscountNote.folio,fechaHora = DateTime.Now, Cliente = _view.DiscountNote.Cliente, tipoDeCambio = Session.Configuration.tipoDeCambio});
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private bool IsDiscountValid(NotasDeDescuento discountNote, out string error)
        {
            error = "";

            if (!discountNote.idCliente.isValid())
            {
                error = "El cliente no es válido";
                return false;
            }

            if (!discountNote.descripcion.isValid())
            {
                error = "La descripción no es válida";
                return false;
            }

            if (!discountNote.idEmpresa.isValid())
            {
                error = "La empresa no es válida";
                return false;
            }

            if (!discountNote.idMoneda.isValid())
            {
                error = "La moneda no es válida";
                return false;
            }

            if (!discountNote.idUsuarioRegistro.isValid())
            {
                error = "El usuario no es válido";
                return false;
            }

            if (!discountNote.monto.isValid())
            {
                error = "El monto no es válido";
                return false;
            }

            if (!discountNote.folio.isValid())
            {
                error = "El folio no es válido";
                return false;
            }

            return true;
        }

        private NotasDeDescuento GetDefault()
        {
            //Establezco los defaults
            var discountNote = new NotasDeDescuento();
            discountNote.Cliente = new Cliente();
            discountNote.fechaHora = DateTime.Now;
            discountNote.idEstatusDeNotaDeDescuento = (int)StatusDeNotaDeDescuento.Nueva;
            discountNote.folio = _discountNotes.Next();
            discountNote.tipoDeCambio = Session.Configuration.tipoDeCambio;

            return discountNote;
        }
    }
}
