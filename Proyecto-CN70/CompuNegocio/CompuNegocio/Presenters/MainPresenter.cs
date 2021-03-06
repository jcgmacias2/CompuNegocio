using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using CompuNegocio;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace Aprovi.Presenters
{
    public class MainPresenter
    {
        private readonly IMainView _view;
        private UnityContainer _container;

        public MainPresenter(IMainView view, UnityContainer container)
        {
            _view = view;
            _container = container;

            _view.OpenSignIn += OpenSignIn;
            _view.OpenTaxes += OpenTaxes;
            _view.OpenUnitsOfMeasure += OpenUnitsOfMeasure;
            _view.OpenItems += OpenItems;
            _view.OpenUsers += OpenUsers;
            _view.OpenClients += OpenClients;
            _view.OpenClassifications += OpenClassifications;
            _view.Quit += Close;
            _view.SignOut += SignOut;
            _view.OpenPrices += OpenPrices;
            _view.OpenPaymentMethods += OpenPaymentMethods;
            _view.OpenSuppliers += OpenSuppliers;
            _view.OpenPurchases += OpenPurchases;
            _view.OpenPurchasePayments += OpenPurchasePayments;
            _view.OpenGeneralProperties += OpenGeneralProperties;
            _view.OpenBusinesses += OpenBusinesses;
            _view.OpenStations += OpenStations;
            _view.OpenInvoices += OpenInvoices;
            _view.OpenFiscalPayments += OpenFiscalPayments;
            _view.OpenBillsOfSale += OpenBillsOfSale;
            _view.OpenPaymentsByPeriod += OpenPaymentsByPeriodReport;
            _view.OpenAdjustments += OpenAdjustments;
            _view.OpenStockReport += OpenStockReport;
            _view.OpenAdjustmentsReport += OpenAdjustmentsReport;
            _view.OpenInvoiceReport += OpenInvoiceReport;
            _view.OpenDatabaseConfig += OpenDatabaseConfig;
            _view.OpenPurchasesByPeriodReport += OpenPurchasesByPeriodReport;
            _view.OpenPayableBalancesReport += OpenPayableBalancesReport;
            _view.OpenSupplierStatementReport += OpenSupplierStatementReport;
            _view.OpenStockFlowReport += OpenStockFlowReport;
            _view.OpenKardexReport += OpenKardexReport;
            _view.OpenItemsTransfer += OpenItemsTransfer;
            _view.OpenCollectableBalancesReport += OpenCollectableBalancesReport;
            _view.OpenClientStatementReport += OpenClientStatementReport;
            _view.OpenMigrationTools += OpenMigrationTools;
            _view.OpenBanks += OpenBanks;
            _view.OpenBankAccounts += OpenBankAccounts;
            _view.OpenProductsServices += OpenProductsServices;
            _view.OpenCFDIUses += OpenCFDIUses;
            _view.OpenPropertyAccounts += OpenPropertyAccounts;
            _view.OpenFiscalPaymentReport += OpenFiscalPaymentReport;
            _view.OpenGuardianManualSend += OpenGuardianManualSend;
            _view.OpenHomologationTool += OpenHomologationTool;
            _view.OpenQuotes += OpenQuotes;
            _view.OpenBillsOfSalePerPeriodReport += OpenBillsOfSalePerPeriodReport;
            _view.OpenSalesPerPeriodReport += ViewOnOpenSalesPerPeriodReport;
            _view.OpenCommissionsPerPeriodReport += ViewOnOpenCommissionsPerPeriodReport;
            _view.OpenOrders += ViewOnOpenOrders;
            _view.OpenInvoiceBillsOfSale += OpenInvoiceBillsOfSale;
            _view.OpenOrdersReport += OpenOrdersReport;
            _view.OpenPurchaseOrders += OpenPurchaseOrders;
            _view.OpenClientPayments += OpenClientPayments;
            _view.OpenQuotesPerPeriodReport += OpenQuotesPerPeriodReport;
            _view.OpenAssociatedCompanies += OpenAssociatedCompanies;
            _view.OpenTransfers += ViewOnOpenTransfers;
            _view.OpenTransferRequestsList += OpenTransferRequestsList;
            _view.OpenTransfersByPeriodReport += OpenTransfersByPeriodReport;
            _view.OpenSalesPerItemReport += ViewOnOpenSalesPerItemReport;
            _view.OpenCreditNotes += ViewOnOpenCreditNotes;
            _view.OpenCreditNotesByPeriodReport += ViewOnOpenCreditNotesByPeriodReport;
            _view.OpenDiscountNotes += ViewOnOpenDiscountNotes;
            _view.OpenTaxesByPeriodReport += ViewOnOpenTaxesByPeriodReport;
            _view.OpenAppraisalReport += _view_OpenAppraisalReport;
            _view.OpenCompanyStatusReport += ViewOnOpenCompanyStatusReport;
            _view.OpenSoldItemsCostReport += ViewOnOpenSoldItemsCostReport;
            _view.OpenPriceListsReport += ViewOnOpenPriceListsReport;
            _view.OpenDiscountNotesReport += ViewOnOpenDiscountNotesReport;
        }

        #region General

        private void OpenDatabaseConfig()
        {
            try
            {
                //Si cae en esta excepción le abro la ventana de configuración del servidor
                IConnectionUpdateView view;
                ConnectionUpdatePresenter connectionPresenter;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "ConfigurationPresenter", true))
                    return;

                view = new ConnectionUpdateView();
                connectionPresenter = new ConnectionUpdatePresenter(view, _container.Resolve<IConfiguracionService>());

                view.ShowWindow();

                //Cierro el sistema para que cargue la fuente de datos adecuada
                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        #endregion

        #region Inicio de sesión

        private void OpenSignIn()
        {
            try
            {
                IAuthenticationView view;
                AuthenticationPresenter presenter;

                //Si el sistema esta en modo configuración, la autenticación se hace a través del api
                view = new AuthenticationView(Session.Configuration.Mode.Equals(Ambiente.Configuration));
                presenter = new AuthenticationPresenter(view, _container.Resolve<IUsuarioService>(), _container.Resolve<ISeguridadService>(), _container.Resolve<IComprobantFiscaleService>(), _container.Resolve<ILicenciaService>());

                _view.HideWindow();
                view.ShowWindow();

                //Si después de mostrar el inicio de sesión no hay un usuario en sesión cierro el sistema
                if (!Session.IsUserLogged)
                { 
                    _view.CloseWindow();
                    return;
                }

                //Muestro el menu
                _view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        #endregion

        #region Menu Archivo

        private void OpenCFDIUses()
        {
            try
            {
                ICFDIUsesView view;
                CFDIUsesPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "BanksPresenter", true))
                    return;

                view = new CFDIUsesView();
                presenter = new CFDIUsesPresenter(view, _container.Resolve<IUsosCFDIService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenBankAccounts()
        {
            try
            {
                IBankAccountsView view;
                BankAccountsPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "BanksPresenter", true))
                    return;

                view = new BankAccountsView();
                presenter = new BankAccountsPresenter(view, _container.Resolve<ICuentaBancariaService>(),_container.Resolve<IBancoService>(), _container.Resolve<ICatalogosEstaticosService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenBanks()
        {
            try
            {
                IBanksView view;
                BanksPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "BanksPresenter", true))
                    return;

                view = new BanksView();
                presenter = new BanksPresenter(view, _container.Resolve<IBancoService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenGuardianManualSend()
        {
            try
            {
                IGuardianManualSendView view;
                GuardianManualSendPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Total, "ConfigurationPresenter", true))
                    return;

                view = new GuardianManualSendView();
                presenter = new GuardianManualSendPresenter(view, _container.Resolve<IEnvioDeCorreoService>(), _container.Resolve<IComprobanteEnviadoService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        //KOWI
        private void OpenItemsTransfer()
        {
            try
            {
                IScaleItemsTransferView view;
                ScaleItemsTransferPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "ItemsPresenter", true))
                    return;

                view = new ScaleItemsTransferView();
                presenter = new ScaleItemsTransferPresenter(view, _container.Resolve<IBasculaService>(), _container.Resolve<IClasificacionService>(), _container.Resolve<IArticuloService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenUnitsOfMeasure()
        {
            try
            {
                IUnitsOfMeasureView view;
                UnitsOfMeasurePresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "UnitsOfMeasurePresenter", true))
                    return;

                view = new UnitsOfMeasureView();
                presenter = new UnitsOfMeasurePresenter(view, _container.Resolve<IUnidadDeMedidaService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenUsers()
        {
            try
            {
                IUsersView view;
                UsersPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "UsersPresenter", true))
                    return;

                view = new UsersView();
                presenter = new UsersPresenter(view, _container.Resolve<IUsuarioService>(), _container.Resolve<IPrivilegioService>(), _container.Resolve<ICatalogosEstaticosService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenClients()
        {
            try
            {
                IClientsView view;
                ClientsPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "ClientsPresenter", true))
                    return;

                view = new ClientsView(Modulos.Envio_De_Correos.IsActive());
                presenter = new ClientsPresenter(view, _container.Resolve<IClienteService>(), _container.Resolve<ICatalogosEstaticosService>(), _container.Resolve<IListaDePrecioService>(),_container.Resolve<ICuentaDeCorreoService>(), _container.Resolve<IArticuloService>(), _container.Resolve<IUsuarioService>(), _container.Resolve<IUsosCFDIService>(), _container.Resolve<ICodigoDeArticuloPorClienteService>(), _container.Resolve<IRegimenService>());

                view.ShowWindowIndependent();

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenClassifications()
        {
            try
            {
                IClassificationsView view;
                ClassificationsPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "ClassificationsPresenter", true))
                    return;

                view = new ClassificationsView();
                presenter = new ClassificationsPresenter(view, _container.Resolve<IClasificacionService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenPrices()
        {
            try
            {
                IPricesView view;
                PricesPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "PricesPresenter", true))
                    return;

                view = new PricesView();
                presenter = new PricesPresenter(view, _container.Resolve<IListaDePrecioService>(), _container.Resolve<IArticuloService>(), _container.Resolve<IClienteService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenPaymentMethods()
        {
            try
            {
                IPaymentFormsView view;
                PaymentFormsPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "PaymentFormsPresenter", true))
                    return;

                view = new PaymentFormsView();
                presenter = new PaymentFormsPresenter(view, _container.Resolve<IFormaPagoService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenSuppliers()
        {
            try
            {
                ISuppliersView view;
                SuppliersPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "SuppliersPresenter", true))
                    return;

                view = new SuppliersView();
                presenter = new SuppliersPresenter(view, _container.Resolve<IProveedorService>(), _container.Resolve<ICatalogosEstaticosService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenAssociatedCompanies()
        {
            try
            {
                IAssociatedCompaniesView view;
                AssociatedCompaniesPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "AssociatedCompaniesPresenter", true))
                    return;

                view = new AssociatedCompaniesView();
                presenter = new AssociatedCompaniesPresenter(view, _container.Resolve<IEmpresaAsociadaService>(), _container.Resolve<IEmpresaService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        #endregion

        #region Menu Inventario

        private void OpenProductsServices()
        {
            try
            {
                IProductsServicesView view;
                ProductsServicesPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "ItemsPresenter", true))
                    return;

                view = new ProductsServicesView();
                presenter = new ProductsServicesPresenter(view, _container.Resolve<IProductoServicioService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenItems()
        {
            try
            {
                IItemsView view;
                ItemsPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "ItemsPresenter", true))
                    return;

                view = new ItemsView();
                presenter = new ItemsPresenter(view, _container.Resolve<IArticuloService>(), _container.Resolve<IImpuestoService>(), _container.Resolve<ICatalogosEstaticosService>(), _container.Resolve<IUnidadDeMedidaService>(), _container.Resolve<IClasificacionService>(), _container.Resolve<IEquivalenciaService>(), _container.Resolve<IProductoServicioService>(),_container.Resolve<IAjusteService>(),_container.Resolve<IPedimentoService>(), _container.Resolve<IProveedorService>(), _container.Resolve<ICodigoDeArticuloPorProveedorService>(), _container.Resolve<IClienteService>(), _container.Resolve<ICodigoDeArticuloPorClienteService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenAdjustments()
        {
            try
            {
                IAdjustmentsView view;
                AdjustmentsPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "AdjustmentsPresenter", true))
                    return;

                view = new AdjustmentsView();
                presenter = new AdjustmentsPresenter(view, _container.Resolve<IAjusteService>(), _container.Resolve<ICatalogosEstaticosService>(), _container.Resolve<IArticuloService>(),_container.Resolve<IPedimentoService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenPropertyAccounts()
        {
            try
            {
                IPropertyAccountView view;
                PropertyAccountPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "ItemsPresenter", true))
                    return;

                view = new PropertyAccountView();
                presenter = new PropertyAccountPresenter(view, _container.Resolve<ICuentaPredialService>());

                view.ShowWindow();
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OpenTransferRequestsList()
        {
            try
            {
                ITransferRequestsListView view;
                TransferRequestsListPresenter presenter;

                view = new TransferRequestsListView();
                presenter = new TransferRequestsListPresenter(view, _container.Resolve<ISolicitudDeTraspasoService>());

                view.ShowWindow();

                var request = view.TransferRequest;

                if (!request.isValid() || !request.idTraspaso.isValid() || !request.idEmpresaAsociadaOrigen.isValid())
                {
                    return;
                }

                ITransfersView transfersView;
                TransfersPresenter transfersPresenter;

                transfersView = new TransfersView(request);
                transfersPresenter = new TransfersPresenter(transfersView, _container.Resolve<ITraspasoService>(), _container.Resolve<IEmpresaAsociadaService>(), _container.Resolve<IEmpresaService>(), _container.Resolve<IArticuloService>(), _container.Resolve<ISolicitudDeTraspasoService>(), _container.Resolve<IPedimentoService>());

                transfersView.ShowWindow();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void ViewOnOpenTransfers()
        {
            try
            {
                ITransfersView view;
                TransfersPresenter presenter;

                view = new TransfersView();
                presenter = new TransfersPresenter(view, _container.Resolve<ITraspasoService>(), _container.Resolve<IEmpresaAsociadaService>(), _container.Resolve<IEmpresaService>(), _container.Resolve<IArticuloService>(), _container.Resolve<ISolicitudDeTraspasoService>(), _container.Resolve<IPedimentoService>());

                view.ShowWindow();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        #endregion

        #region Menu Ventas

        private void OpenInvoices()
        {
            try
            {
                IInvoicesView view;
                InvoicesPresenter presenter;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "InvoicesPresenter", true))
                    return;

                view = new InvoicesView();
                presenter = new InvoicesPresenter(view, _container.Resolve<IFacturaService>(), _container.Resolve<ICatalogosEstaticosService>(), _container.Resolve<IClienteService>(), _container.Resolve<IArticuloService>(), _container.Resolve<IAbonoDeFacturaService>(), _container.Resolve<IListaDePrecioService>(), _container.Resolve<IUsosCFDIService>(), _container.Resolve<IConfiguracionService>(), _container.Resolve<ICuentaBancariaService>(), _container.Resolve<ICuentaPredialService>(), _container.Resolve<IEnvioDeCorreoService>(), _container.Resolve<ICotizacionService>(), _container.Resolve<IPedimentoService>(), _container.Resolve<IUsuarioService>(), _container.Resolve<ISeguridadService>(), _container.Resolve<IPagoService>(), _container.Resolve<INotaDeCreditoService>(), _container.Resolve<INotaDeDescuentoService>());

                view.ShowWindowIndependent();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenFiscalPayments()
        {
            try
            {
                IFiscalPaymentsView view;
                FiscalPaymentsPresenter presenter;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Total, "InvoicesPresenter", true))
                    return;

                view = new FiscalPaymentsView();
                presenter = new FiscalPaymentsPresenter(view, _container.Resolve<IAbonoDeFacturaService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenBillsOfSale()
        {
            try
            {
                IBillsOfSaleView view;
                BillsOfSalePresenter presenter;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "BillsOfSalePresenter", true))
                    return;

                view = new BillsOfSaleView();
                presenter = new BillsOfSalePresenter(view, _container.Resolve<IRemisionService>(), _container.Resolve<ICatalogosEstaticosService>(), _container.Resolve<IClienteService>(), _container.Resolve<IArticuloService>(),
                    _container.Resolve<IListaDePrecioService>(), _container.Resolve<IAbonoDeRemisionService>(), _container.Resolve<IFacturaService>(), _container.Resolve<IAbonoDeFacturaService>(), _container.Resolve<ICuentaBancariaService>(), _container.Resolve<IUsosCFDIService>(), _container.Resolve<IConfiguracionService>(), _container.Resolve<IEnvioDeCorreoService>(), _container.Resolve<ICotizacionService>(), _container.Resolve<IPedimentoService>(), _container.Resolve<IUsuarioService>(), _container.Resolve<ISeguridadService>());

                view.ShowWindowIndependent();

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenQuotes()
        {
            try
            {
                IQuotesView view;
                QuotesPresenter presenter;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Total, "QuotesPresenter", true))
                    return;

                view = new QuotesView();
                presenter = new QuotesPresenter(view,_container.Resolve<IRemisionService>(),_container.Resolve<ICatalogosEstaticosService>(),_container.Resolve<IClienteService>(),_container.Resolve<IArticuloService>(),_container.Resolve<IListaDePrecioService>(),_container.Resolve<IFacturaService>(),_container.Resolve<IConfiguracionService>(),_container.Resolve<IEnvioDeCorreoService>(),_container.Resolve<ICotizacionService>());

                view.ShowWindowIndependent();

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void ViewOnOpenOrders()
        {
            try
            {
                IOrdersView view;
                OrdersPresenter presenter;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Total, "OrdersPresenter", true))
                    return;

                view = new OrdersView();
                presenter = new OrdersPresenter(view,_container.Resolve<IPedidoService>(),_container.Resolve<ICatalogosEstaticosService>(),_container.Resolve<IClienteService>(),_container.Resolve<IArticuloService>(),_container.Resolve<IImpuestoService>(),_container.Resolve<IFacturaService>(),_container.Resolve<IRemisionService>(),_container.Resolve<IAbonoDeFacturaService>(),_container.Resolve<IUsosCFDIService>(),_container.Resolve<IListaDePrecioService>(),_container.Resolve<IAbonoDeRemisionService>(),_container.Resolve<ICuentaBancariaService>(),_container.Resolve<IConfiguracionService>(),_container.Resolve<ICuentaPredialService>(),_container.Resolve<IEnvioDeCorreoService>(),_container.Resolve<ICotizacionService>(),_container.Resolve<IPedimentoService>(), _container.Resolve<IUsuarioService>(), _container.Resolve<ISeguridadService>(), _container.Resolve<IPagoService>(), _container.Resolve<INotaDeCreditoService>(), _container.Resolve<INotaDeDescuentoService>());

                view.ShowWindowIndependent();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenInvoiceBillsOfSale()
        {
            try
            {
                IBillsOfSaleSelectListView viewSelectList;
                BillsOfSaleSelectListPresenter presenterSelectList;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "InvoicesPresenter", true))
                    return;

                viewSelectList = new BillsOfSaleSelectListView();
                presenterSelectList = new BillsOfSaleSelectListPresenter(viewSelectList, _container.Resolve<IRemisionService>());

                viewSelectList.ShowWindow();

                if (!(viewSelectList.SelectedBillsOfSale.Count > 0))
                    throw new Exception("No se ha seleccionado ninguna remisión");

                IInvoiceBillsOfSaleView toInvoiceView;
                InvoiceBillsOfSalePresenter toInvoicePresenter;

                toInvoiceView = new InvoiceBillsOfSaleView();
                toInvoicePresenter = new InvoiceBillsOfSalePresenter(toInvoiceView, _container.Resolve<ICatalogosEstaticosService>(), _container.Resolve<IConfiguracionService>(), _container.Resolve<IUsosCFDIService>(), _container.Resolve<IClienteService>(), _container.Resolve<IRemisionService>(), _container.Resolve<IFacturaService>(), _container.Resolve<IArticuloService>(), _container.Resolve<IAbonoDeFacturaService>(), _container.Resolve<IListaDePrecioService>(), _container.Resolve<ICuentaBancariaService>(), _container.Resolve<ICuentaPredialService>(), _container.Resolve<IEnvioDeCorreoService>(), _container.Resolve<ICotizacionService>(), _container.Resolve<IPedimentoService>(), _container.Resolve<IUsuarioService>(), _container.Resolve<ISeguridadService>(), _container.Resolve<IPagoService>(), _container.Resolve<INotaDeCreditoService>(), _container.Resolve<INotaDeDescuentoService>(), viewSelectList.SelectedBillsOfSale);

                toInvoiceView.ShowWindow();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OpenClientPayments()
        {
            try
            {
                IClientPaymentsView view;
                ClientPaymentsPresenter presenter;

                //Valido que tenga acceso
                //if (!Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "ClientPaymentsPresenter"))
                //    return;

                view = new ClientPaymentsView();
                presenter = new ClientPaymentsPresenter(view,_container.Resolve<IClienteService>(), _container.Resolve<ICatalogosEstaticosService>(),_container.Resolve<ICuentaBancariaService>(), _container.Resolve<IFacturaService>(), _container.Resolve<IAbonoDeFacturaService>(), _container.Resolve<IEnvioDeCorreoService>(), _container.Resolve<IPagoService>(), _container.Resolve<IConfiguracionService>());

                view.ShowWindow();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void ViewOnOpenCreditNotes()
        {
            try
            {
                ICreditNotesView view;
                CreditNotesPresenter presenter;

                view = new CreditNotesView();
                presenter = new CreditNotesPresenter(view, _container.Resolve<INotaDeCreditoService>(), _container.Resolve<ICatalogosEstaticosService>(), _container.Resolve<IClienteService>(), _container.Resolve<IArticuloService>(), _container.Resolve<IListaDePrecioService>(), _container.Resolve<IConfiguracionService>(), _container.Resolve<ICuentaBancariaService>(), _container.Resolve<IEnvioDeCorreoService>(), _container.Resolve<IPedimentoService>(), _container.Resolve<IUsuarioService>(), _container.Resolve<ISeguridadService>());

                view.ShowWindow();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void ViewOnOpenDiscountNotes()
        {
            try
            {
                IDiscountNotesView view;
                DiscountNotesPresenter presenter;

                view = new DiscountNotesView();
                presenter = new DiscountNotesPresenter(view, _container.Resolve<IClienteService>(),_container.Resolve<INotaDeDescuentoService>(),_container.Resolve<IConfiguracionService>(), _container.Resolve<ICatalogosEstaticosService>());

                view.ShowWindow();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        #endregion

        #region Menu Compras

        private void OpenPurchases()
        {
            try
            {
                IPurchasesView view;
                PurchasesPresenter presenter;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "PurchasesPresenter", true))
                    return;

                view = new PurchasesView();
                presenter = new PurchasesPresenter(view, _container.Resolve<ICompraService>(), _container.Resolve<IAbonoDeCompraService>(), _container.Resolve<IArticuloService>(), _container.Resolve<IProveedorService>(), _container.Resolve<IImpuestoService>(), _container.Resolve<ICatalogosEstaticosService>(), _container.Resolve<IEquivalenciaService>(), _container.Resolve<IUnidadDeMedidaService>(), _container.Resolve<IFormaPagoService>(), _container.Resolve<IOrdenDeCompraService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenPurchasePayments()
        {
            try
            {
                IPurchasePaymentsView view;
                PurchasePaymentsPresenter presenter;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "PurchasesPresenter", true))
                    return;

                view = new PurchasePaymentsView();
                presenter = new PurchasePaymentsPresenter(view, _container.Resolve<IAbonoDeCompraService>(), _container.Resolve<ICatalogosEstaticosService>(), _container.Resolve<IFormaPagoService>(), _container.Resolve<ICompraService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenPurchaseOrders()
        {
            try
            {
                IPurchaseOrdersView view;
                PurchaseOrdersPresenter presenter;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "PurchaseOrdersPresenter", true))
                    return;

                view = new PurchaseOrdersView();
                presenter = new PurchaseOrdersPresenter(view,_container.Resolve<IOrdenDeCompraService>(),_container.Resolve<ICatalogosEstaticosService>(),_container.Resolve<IArticuloService>(),_container.Resolve<IImpuestoService>(),_container.Resolve<IUnidadDeMedidaService>(),_container.Resolve<ICompraService>(),_container.Resolve<IAbonoDeCompraService>(),_container.Resolve<IEquivalenciaService>(),_container.Resolve<IFormaPagoService>(), _container.Resolve<IProveedorService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
        #endregion

        #region Reportes

        private void ViewOnOpenTaxesByPeriodReport()
        {
            try
            {
                ITaxesPerPeriodReportView view;
                TaxesPerPeriodReportPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "Reports", true))
                    return;

                view = new TaxesPerPeriodReportView();
                presenter = new TaxesPerPeriodReportPresenter(view, _container.Resolve<IFacturaService>(), _container.Resolve<INotaDeCreditoService>(), _container.Resolve<IImpuestoService>(), _container.Resolve<ICatalogosEstaticosService>());

                view.ShowWindow();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void OpenTransfersByPeriodReport()
        {
            try
            {
                ITransfersByPeriodReportView view;
                TransfersByPeriodReportPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "Reports", true))
                    return;

                view = new TransfersByPeriodReportView();
                presenter = new TransfersByPeriodReportPresenter(view, _container.Resolve<ITraspasoService>(), _container.Resolve<IEmpresaAsociadaService>(), _container.Resolve<IArticuloService>());

                view.ShowWindow();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OpenClientStatementReport()
        {
            try
            {
                IClientStatementReportView view;
                ClientStatementReportPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "Reports", true))
                    return;

                view = new ClientStatementReportView();
                presenter = new ClientStatementReportPresenter(view, _container.Resolve<IClienteService>(), _container.Resolve<ISaldosPorClienteService>());

                view.ShowWindow();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OpenCollectableBalancesReport()
        {
            try
            {
                ICollectableBalancesReportView view;
                CollectableBalancesReportPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "Reports", true))
                    return;

                view = new CollectableBalancesReportView();
                presenter = new CollectableBalancesReportPresenter(view,_container.Resolve<ISaldosPorClienteService>(),_container.Resolve<IUsuarioService>(), _container.Resolve<IClienteService>(), _container.Resolve<IFacturaService>(), _container.Resolve<IRemisionService>());

                view.ShowWindow();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OpenKardexReport()
        {
            try
            {
                IKardexReportView view;
                KardexReportPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "Reports", true))
                    return;

                view = new KardexReportView();
                presenter = new KardexReportPresenter(view, _container.Resolve<IArticuloService>());

                view.ShowWindow();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OpenStockFlowReport()
        {
            try
            {
                IStockFlowReportView view;
                StockFlowReportPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "Reports", true))
                    return;

                view = new StockFlowReportView();
                presenter = new StockFlowReportPresenter(view, _container.Resolve<IArticuloService>(),_container.Resolve<IClasificacionService>());

                view.ShowWindowIndependent();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenSupplierStatementReport()
        {
            try
            {
                ISupplierStatementReportView view;
                SupplierStatementReportPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "Reports", true))
                    return;

                view = new SupplierStatementReportView();
                presenter = new SupplierStatementReportPresenter(view, _container.Resolve<IProveedorService>(), _container.Resolve<ISaldosPorProveedorService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenPayableBalancesReport()
        {
            try
            {
                IPayableBalancesReportView view;
                PayableBalancesReportPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "Reports", true))
                    return;

                view = new PayableBalancesReportView();
                presenter = new PayableBalancesReportPresenter(view, _container.Resolve<ISaldosPorProveedorService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenPurchasesByPeriodReport()
        {
            try
            {
                IPurchasesByPeriodReportView view;
                PurchasesByPeriodReportPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "Reports", true))
                    return;

                view = new PurchasesByPeriodReportView();
                presenter = new PurchasesByPeriodReportPresenter(view, _container.Resolve<IProveedorService>(), _container.Resolve<ICompraService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenPaymentsByPeriodReport()
        {
            try
            {
                IPaymentsByPeriodReportView view;
                PaymentsByPeriodReportPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "Reports", true))
                    return;

                view = new PaymentsByPeriodReportView();
                presenter = new PaymentsByPeriodReportPresenter(view, _container.Resolve<IAbonosService>(), _container.Resolve<IEmpresaService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenStockReport()
        {
            try
            {
                IStockReportView view;
                StockReportPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "Reports", true))
                    return;

                view = new StockReportView();
                presenter = new StockReportPresenter(view, _container.Resolve<IArticuloService>(),_container.Resolve<IClasificacionService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenAdjustmentsReport()
        {
            try
            {
                IAdjustmentsReportView view;
                AdjustmentsReportPresenter presenter;

                //Valido que tenga acceso
                if (!Session.IsUserLogged || !Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "Reports", true))
                    return;

                view = new AdjustmentsReportView();
                presenter = new AdjustmentsReportPresenter(view, _container.Resolve<IAjusteService>(), _container.Resolve<ICatalogosEstaticosService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenInvoiceReport()
        {
            try
            {
                IInvoicePrintView view;
                InvoicePrintPresenter presenter;

                view = new InvoicePrintView();
                presenter = new InvoicePrintPresenter(view, _container.Resolve<IFacturaService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenFiscalPaymentReport()
        {
            try
            {
                IFiscalPaymentPrintView view;
                FiscalPaymentPrintPresenter presenter;

                view = new FiscalPaymentPrintView();
                presenter = new FiscalPaymentPrintPresenter(view, _container.Resolve<IAbonoDeFacturaService>(), _container.Resolve<IPagoService>());

                view.ShowWindow();
            }
            catch(Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenBillsOfSalePerPeriodReport()
        {
            try
            {
                IBillsOfSalePerPeriodReportView view;
                BillsOfSalePerPeriodReportPresenter presenter;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Total, "BillsOfSalePresenter", true))
                    return;

                view = new BillsOfSalePerPeriodReportView();
                presenter = new BillsOfSalePerPeriodReportPresenter(view, _container.Resolve<IRemisionService>(),_container.Resolve<ICatalogosEstaticosService>());

                view.ShowWindow();

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void ViewOnOpenSalesPerPeriodReport()
        {
            try
            {
                ISalesPerPeriodReportView view;
                SalesPerPeriodReportPresenter presenter;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Total, "InvoicesPresenter", true))
                    return;

                view = new SalesPerPeriodReportView();
                presenter = new SalesPerPeriodReportPresenter(view, _container.Resolve<IRemisionService>(), _container.Resolve<IFacturaService>());

                view.ShowWindow();

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void ViewOnOpenCommissionsPerPeriodReport()
        {
            try
            {
                ICommissionsPerPeriodReportView view;
                CommissionsPerPeriodReportPresenter presenter;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Total, "InvoicesPresenter", true))
                    return;

                view = new CommissionsPerPeriodReportView();
                presenter = new CommissionsPerPeriodReportPresenter(view, _container.Resolve<IRemisionService>(), _container.Resolve<IFacturaService>(), _container.Resolve<IUsuarioService>());

                view.ShowWindow();

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenOrdersReport()
        {
            try
            {
                IOrdersReportView view;
                OrdersReportPresenter presenter;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Total, "OrdersPresenter", true))
                    return;

                view = new OrdersReportView();
                presenter = new OrdersReportPresenter(view, _container.Resolve<IPedidoService>(), _container.Resolve<IClienteService>(), _container.Resolve<IConfiguracionService>());

                view.ShowWindow();

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenQuotesPerPeriodReport()
        {
            try
            {
                IQuotesPerPeriodReportView view;
                QuotesPerPeriodReportPresenter presenter;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Total, "QuotesPresenter", true))
                    return;

                view = new QuotesPerPeriodReportView();
                presenter = new QuotesPerPeriodReportPresenter(view, _container.Resolve<ICotizacionService>(), _container.Resolve<IClienteService>());

                view.ShowWindow();

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void ViewOnOpenSalesPerItemReport()
        {
            try
            {
                ISalesPerItemReportView view;
                SalesPerItemReportPresenter presenter;

                view = new SalesPerItemReportView();
                presenter = new SalesPerItemReportPresenter(view, _container.Resolve<IArticuloService>(), _container.Resolve<IClasificacionService>(), _container.Resolve<IVentasPorArticuloService>());

                view.ShowWindow();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void ViewOnOpenCreditNotesByPeriodReport()
        {
            try
            {
                ICreditNotesByPeriodReportView view;
                CreditNotesByPeriodReportPresenter presenter;

                view = new CreditNotesByPeriodReportView();
                presenter = new CreditNotesByPeriodReportPresenter(view, _container.Resolve<IClienteService>(), _container.Resolve<INotaDeCreditoService>());

                view.ShowWindow();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void _view_OpenAppraisalReport()
        {
            try
            {
                IItemsAppraisalReportView view;
                ItemsAppraisalReportPresenter presenter;

                view = new ItemsAppraisalReportView();
                presenter = new ItemsAppraisalReportPresenter(view, _container.Resolve<IArticuloService>(), _container.Resolve<IClasificacionService>());

                view.ShowWindow();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void ViewOnOpenCompanyStatusReport()
        {
            try
            {
                ICompanyStatusReportView view;
                CompanyStatusReportPresenter presenter;

                view = new CompanyStatusReportView();
                presenter = new CompanyStatusReportPresenter(view, _container.Resolve<IEstadoDeLaEmpresaService>());

                view.ShowWindow();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void ViewOnOpenSoldItemsCostReport()
        {
            try
            {
                ISoldItemsCostReportView view;
                SoldItemsCostReportPresenter presenter;

                view = new SoldItemsCostReportView();
                presenter = new SoldItemsCostReportPresenter(view, _container.Resolve<ICostoDeLoVendidoService>());

                view.ShowWindow();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void ViewOnOpenPriceListsReport()
        {
            try
            {
                IPriceListsReportView view;
                PriceListsReportPresenter presenter;

                view = new PriceListsReportView();
                presenter = new PriceListsReportPresenter(view, _container.Resolve<IListasDePreciosService>(), _container.Resolve<ICatalogosEstaticosService>(), _container.Resolve<IClasificacionService>());

                view.ShowWindow();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void ViewOnOpenDiscountNotesReport()
        {
            try
            {
                IDiscountNotesPerPeriodReportView view;
                DiscountNotesPerPeriodReportPresenter presenter;

                view = new DiscountNotesPerPeriodReportView();
                presenter = new DiscountNotesPerPeriodReportPresenter(view, _container.Resolve<INotaDeDescuentoService>(), _container.Resolve<IClienteService>());

                view.ShowWindow();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }
        #endregion

        #region Menu Propiedades

        private void OpenGeneralProperties()
        {
            try
            {
                IConfigurationView view;
                ConfigurationPresenter presenter;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "ConfigurationPresenter", true))
                    return;

                view = new ConfigurationView(Modulos.Envio_De_Correos.IsActive());
                presenter = new ConfigurationPresenter(view, _container.Resolve<IConfiguracionService>(), _container.Resolve<IRegimenService>(), _container.Resolve<IUsuarioService>(), _container.Resolve<ISeguridadService>(), _container.Resolve<ICertificadoService>(), _container.Resolve<ISerieService>(), _container.Resolve<ICatalogosEstaticosService>(), _container.Resolve<IClienteService>(), _container.Resolve<IComprobantFiscaleService>(), _container.Resolve<ILicenciaService>(),_container.Resolve<ICuentaGuardianService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }  

        private void OpenTaxes()
        {
            try
            {
                ITaxesView view;
                TaxesPresenter presenter;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "TaxesPresenter", true))
                    return;

                view = new TaxesView();
                presenter = new TaxesPresenter(view, _container.Resolve<IImpuestoService>(), _container.Resolve<ICatalogosEstaticosService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenBusinesses()
        {
            try
            {
                IBusinessesView view;
                BusinessPresenter presenter;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "BusinessPresenter", true))
                    return;

                view = new BusinessesView();
                presenter = new BusinessPresenter(view, _container.Resolve<IEmpresaService>(), _container.Resolve<ILicenciaService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenStations()
        {
            try
            {
                IStationsView view;
                StationsPresenter presenter;

                //Valido que tenga acceso
                if (!Session.LoggedUser.HasAccess(AccesoRequerido.Total, "BusinessPresenter", true))
                    return;

                view = new StationsView();
                presenter = new StationsPresenter(view, _container.Resolve<IEstacionService>(), _container.Resolve<IEmpresaService>(), _container.Resolve<ICatalogosEstaticosService>(), _container.Resolve<IConfiguracionService>(),_container.Resolve<IDispositivoService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }  

        private void OpenMigrationTools()
        {
            try
            {
                IMigrationToolsView view;
                MigrationToolsPresenter presenter;

                //No requiere grado de acceso pues depende del modo en que se este corriendo el sistema

                view = new MigrationToolsView();
                presenter = new MigrationToolsPresenter(view, _container.Resolve<IArticuloService>(), _container.Resolve<IClienteService>(), _container.Resolve<IProveedorService>(),_container.Resolve<IClasificacionService>(),_container.Resolve<IUnidadDeMedidaService>(),_container.Resolve<IImpuestoService>(),_container.Resolve<IMigrationDataService>(),_container.Resolve<IAjusteService>());

                view.ShowWindow();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OpenHomologationTool()
        {
            try
            {
                IItemsHomologationToolView view;
                ItemsHomologationToolPresenter presenter;

                //No requiere grado de acceso pues depende del modo en que se este corriendo el sistema
                view = new ItemsHomologationToolView();
                presenter = new ItemsHomologationToolPresenter(view, _container.Resolve<IMigrationDataService>());

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        #endregion

        #region Menu Salir

        private void Close()
        {
            try
            {
                _view.CloseWindow();
                App.Current.Shutdown();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void SignOut()
        {
            IAuthenticationView view;
            AuthenticationPresenter presenter;

            view = new AuthenticationView(false);
            presenter = new AuthenticationPresenter(view, _container.Resolve<IUsuarioService>(), _container.Resolve<ISeguridadService>(), _container.Resolve<IComprobantFiscaleService>(), _container.Resolve<ILicenciaService>());

            //Elimino el usuario de session para que pueda cargar uno nuevo en caso que se autentique
            Session.LoggedUser = null;

            _view.HideWindow();
            view.ShowWindow();

            //Si después de mostrar el inicio de sesión no hay un usuario en sesión cierro el sistema
            if (!Session.IsUserLogged)
            {
                _view.CloseWindow();
                return;
            }

            //Muestro el menu
            _view.ShowWindow();
        }

        #endregion
    }
}
