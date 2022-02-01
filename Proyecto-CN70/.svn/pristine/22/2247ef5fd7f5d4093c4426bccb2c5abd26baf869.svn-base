using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Application.Helpers;
using Aprovi.Business.ViewModels;
using Microsoft.Practices.ObjectBuilder2;

namespace Aprovi.Presenters
{
    public class InvoiceBillsOfSalePresenter
    {
        private IInvoiceBillsOfSaleView _view;
        private ICatalogosEstaticosService _catalogs;
        private IConfiguracionService _configs;
        private IUsosCFDIService _cfdiUsages;
        private IClienteService _customers;
        private IRemisionService _billsOfSale;
        private IFacturaService _invoices;
        private IArticuloService _items;
        private IAbonoDeFacturaService _invoicePayments;
        private IListaDePrecioService _pricesList;
        private ICuentaBancariaService _bankAccounts;
        private ICuentaPredialService _predialAccounts;
        private IEnvioDeCorreoService _emailService;
        private ICotizacionService _quotes;
        private IPedimentoService _customs;
        private IUsuarioService _users;
        private ISeguridadService _security;
        private IPagoService _payments;
        private INotaDeCreditoService _creditNotes;
        private INotaDeDescuentoService _discountNotes;
        private List<VMRemision> _selectedBillsOfSale;

        public InvoiceBillsOfSalePresenter(IInvoiceBillsOfSaleView view, ICatalogosEstaticosService catalogs, IConfiguracionService configs,IUsosCFDIService cfdiUsages, IClienteService customers, IRemisionService billsOfSale, IFacturaService invoices, IArticuloService items, IAbonoDeFacturaService invoicePayments, IListaDePrecioService pricesList, ICuentaBancariaService bankAccounts, ICuentaPredialService predialAccounts, IEnvioDeCorreoService emailService, ICotizacionService quotes, IPedimentoService customs, IUsuarioService users, ISeguridadService security,IPagoService payments, INotaDeCreditoService creditNotes, INotaDeDescuentoService discountNotes, List<VMRemision> selectedBillsOfSale)
        {
            _view = view;
            _catalogs = catalogs;
            _configs = configs;
            _cfdiUsages = cfdiUsages;
            _customers = customers;
            _billsOfSale = billsOfSale;
            _invoices = invoices;
            _items = items;
            _invoicePayments = invoicePayments;
            _pricesList = pricesList;
            _bankAccounts = bankAccounts;
            _predialAccounts = predialAccounts;
            _emailService = emailService;
            _quotes = quotes;
            _customs = customs;
            _users = users;
            _security = security;
            _payments = payments;
            _creditNotes = creditNotes;
            _discountNotes = discountNotes;

            _view.Save += Save;
            _view.Quit += Quit;
            _view.OpenListCustomers += OpenListCustomers;
            _view.Return += Return;
            _view.ChangeCurrency += ChangeCurrency;
            _view.FindCustomer += FindCustomer;
            _view.FindUser += FindUser;
            _view.OpenUsersList += OpenUsersList;

            List<Regimene> regimenes = _configs.GetDefault().Regimenes.Where(x => x.activo).ToList();
            _view.FillCombos(_catalogs.ListMonedas(), regimenes, _catalogs.ListMetodosDePago(), _cfdiUsages.List());

            //Carga los datos por default
            //Aqui solo tengo el resumen de las remisiones, este proceso requiere los registros completos
            _selectedBillsOfSale = new List<VMRemision>();
            foreach (var bos in selectedBillsOfSale)
            {
                _selectedBillsOfSale.Add(new VMRemision(_billsOfSale.Find(bos.idRemision)));
            }
            _selectedBillsOfSale = _selectedBillsOfSale.ToCurrency(new Moneda() { idMoneda = (int)Monedas.Pesos });
            _view.SetExchangeRate(Session.Configuration.tipoDeCambio);
            _view.SetCurrency(new Moneda() { idMoneda = (int)Monedas.Pesos });
            _view.Show(Session.LoggedUser);
        }

        private void OpenUsersList()
        {
            try
            {
                IUsersListView usersView = new UsersListView();
                UsersListPresenter usersPresenter = new UsersListPresenter(usersView,_users);

                usersView.ShowWindow();

                if (usersView.User.isValid() && usersView.User.idUsuario.isValid())
                {
                    _view.Show(usersView.User);
                }
                else
                {
                    _view.Show(new Usuario());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FindUser()
        {
            try
            {
                var user = _users.Find(_view.Invoice.Usuario1.nombreDeUsuario);

                if (user.isValid() && user.idUsuario.isValid())
                {
                    _view.Show(user);
                }
                else
                {
                    _view.ShowError("No se encontró un usuario con el nombre de usuario proporcionado");
                    _view.Show(new Usuario());
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FindCustomer()
        {
            if (_view.Invoice.Cliente.isValid())
            {
                var customer = _customers.Find(_view.Invoice.Cliente.codigo);

                if (customer.isValid())
                {
                    _view.Show(customer);
                }
                else
                {
                    _view.ShowError("No se encontró un cliente con el código proporcionado");
                }
            }
        }

        private void ChangeCurrency()
        {
            try
            {
                //Se genera la factura en la moneda seleccionada
                FormasPago formaPago = _catalogs.ListFormasDePago().FirstOrDefault(x => x.idFormaPago == (int)Formas_De_Pago.Efectivo);//Se requiere el objeto para los abonos
                string invoiceSerie = Session.SerieFacturas.identificador;
                VMFactura selections = _view.Invoice;

                VMFactura invoice = _billsOfSale.ToInvoice(_selectedBillsOfSale,invoiceSerie,
                    _invoices.Next(invoiceSerie),
                    selections.tipoDeCambio,
                    new Cliente(),
                    selections.Moneda,
                    new Regimene(),
                    new MetodosPago(),
                    new UsosCFDI(),
                    Session.Configuration.Estacion.Empresa,
                    Session.LoggedUser,
                    formaPago
                    );

                _view.Show(invoice);
                _view.Show(_selectedBillsOfSale);
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Return()
        {
            try
            {
                IBillsOfSaleSelectListView view;
                BillsOfSaleSelectListPresenter presenter;

                view = new BillsOfSaleSelectListView();
                presenter = new BillsOfSaleSelectListPresenter(view,_billsOfSale);

                _view.CloseWindow();
                view.ShowWindow();
            }
            catch (Exception e)
            {
                _view.ShowMessage(e.Message);
            }
        }

        private void OpenListCustomers()
        {
            try
            {
                IClientsListView view;
                ClientsListPresenter presenter;

                view = new ClientsListView();
                presenter = new ClientsListPresenter(view, _customers);

                view.ShowWindow();

                if (view.Client.isValid() && view.Client.idCliente.isValid())
                {
                    _view.Show(view.Client);
                }
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

        private void Save()
        {
            string error;

            if (!IsInvoiceValid(_view.Invoice, out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                IInvoicesView invoiceView = new InvoicesView();
                InvoicesPresenter invoicePresenter = new InvoicesPresenter(invoiceView, _invoices, _catalogs, _customers, _items, _invoicePayments, _pricesList, _cfdiUsages, _configs, _bankAccounts, _predialAccounts, _emailService, _quotes, _customs, _users, _security,_payments, _creditNotes, _discountNotes);

                _view.CloseWindow();
                invoiceView.ShowWindowIndependent();

                VMFactura invoice = _view.Invoice;

                //Se anexan las remisiones relacionadas
                invoice.Remisiones = _selectedBillsOfSale.Cast<Remisione>().ToList();

                invoice.UpdateAccount();

                invoiceView.Show(invoice);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private bool IsInvoiceValid(VMFactura invoice, out string error)
        {
            if(!invoice.isValid())
            {
                error = "Factura inválida";
                return false;
            }

            if(!invoice.serie.isValid())
            {
                error = "Serie de factura inválida";
                return false;
            }

            if (!invoice.folio.isValid())
            {
                error = "Folio de factura inválida";
                return false;
            }

            if (invoice.tipoDeCambio< 0.0m)
            {
                error = "Tasa de cambio inválido";
                return false;
            }

            if(!invoice.idCliente.isValid())
            {
                error = "Cliente inválido";
                return false;
            }

            if(!invoice.idMoneda.isValid())
            {
                error = "Moneda inválida";
                return false;
            }

            if (!invoice.idRegimen.isValid())
            {
                error = "Régimen inválido";
                return false;
            }

            if (!invoice.idMetodoPago.isValid())
            {
                error = "Metodo de pago inválido";
                return false;
            }

            if (!invoice.idUsoCFDI.isValid())
            {
                error = "Uso de CFDI inválido";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }
}
