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
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using Aprovi.Business.Helpers;
using Aprovi.Business.ViewModels;

namespace Aprovi.Presenters
{
    public class PurchasesPresenter
    {
        private readonly IPurchasesView _view;
        private ICompraService _purchases;
        private IAbonoDeCompraService _payments;
        private IArticuloService _items;
        private IProveedorService _suppliers;
        private IImpuestoService _taxes;
        private IEquivalenciaService _equivalencies;
        private IUnidadDeMedidaService _unitsOfMeasure;
        private ICatalogosEstaticosService _catalogs;
        private IFormaPagoService _paymentForms;
        private IOrdenDeCompraService _purchaseOrders;

        public PurchasesPresenter(IPurchasesView view, ICompraService purchasesService, IAbonoDeCompraService paymentsService, IArticuloService itemsService, IProveedorService suppliersService, IImpuestoService taxesService, ICatalogosEstaticosService catalogsService, IEquivalenciaService equivalenciesService, IUnidadDeMedidaService unitsOfMeasureService, IFormaPagoService payments, IOrdenDeCompraService purchaseOrders)
        {
            _view = view;
            _purchases = purchasesService;
            _payments = paymentsService;
            _items = itemsService;
            _suppliers = suppliersService;
            _taxes = taxesService;
            _equivalencies = equivalenciesService;
            _unitsOfMeasure = unitsOfMeasureService;
            _catalogs = catalogsService;
            _paymentForms = payments;
            _purchaseOrders = purchaseOrders;

            _view.FindSupplier += FindSupplier;
            _view.OpenSuppliersList += OpenSuppliersList;
            _view.FindItem += FindItem;
            _view.OpenItemsList += OpenItemsList;
            _view.AddItem += AddItem;
            _view.SelectItem += SelectItem;
            _view.RemoveItem += RemoveItem;
            _view.ViewTaxDetails += ViewTaxDetails;
            _view.Find += Find;
            _view.OpenList += OpenList;
            _view.Quit += Quit;
            _view.New += New;
            _view.Cancel += Cancel;
            _view.OpenPayments += OpenPayments;
            _view.Save += Save;
            _view.Print += Print;
            _view.OpenPurchaseOrdersList += OpenPurchaseOrdersList;
            _view.FindPurchaseOrder += FindPurchaseOrder;
            _view.UpdateCharges += ViewOnUpdateCharges;
            _view.ImportCFDI += ViewOnImportCfdi;

            _view.FillMonedas(catalogsService.ListMonedas());
        }

        private void ViewOnImportCfdi()
        {
            try
            {
                //Se valida que se haya seleccionado un proveedor
                var vm = _view.Purchase;

                if (!vm.Proveedore.isValid() || !vm.Proveedore.idProveedor.isValid())
                {
                    _view.ShowError("Se debe seleccionar un proveedor para importar un CFDI");
                    return;
                }

                var xmlPath = _view.OpenFileFinder("Archivo XML (*.xml)|*.xml");

                if (!xmlPath.isValid())
                {
                    //No se selecciono ningun archivo
                    return;
                }

                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(xmlPath);

                    if (!doc.IsCFDI())
                    {
                        _view.ShowError("El archivo indicado no es un CFDI de ingresos válido");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _view.ShowError(string.Format("Ha ocurrido un error al importar el CFDI: {0}", ex.Message));
                    return;
                }

                //Se verifica que el rfc del proveedor coincida, de lo contrario se muestra mensaje de confirmacion
                string providerRfc = doc.GetIssuerRfcFromCFDI();

                if (!providerRfc.isValid())
                {
                    _view.ShowError("El CFDI no es válido");
                    return;
                }

                if (providerRfc != vm.Proveedore.rfc && _view.ShowMessageWithOptions("El rfc del proveedor no coincide con el proveedor seleccionado, ¿desea continuar?") != MessageBoxResult.Yes)
                {
                    return;
                }

                vm = (VMCompra) _purchases.Import(vm, doc);
                vm.UpdateAccount();

                _view.Show(vm);
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void ViewOnUpdateCharges()
        {
            try
            {
                var vm = _view.Purchase;

                //Se refresca el total de la cuenta
                vm.UpdateAccount();

                _view.Show(vm);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FindPurchaseOrder()
        {
            try
            {
                var folio = _view.Purchase.OrdenesDeCompra.folio;

                //if (!_view.Purchase.idProveedor.isValid())
                //{
                //    _view.ShowError("Se debe seleccionar un proveedor");
                //    return;
                //}

                if (!folio.isValid())
                {
                    return;
                }

                var purchaseOrder = _purchaseOrders.FindByFolio(folio);

                if (!purchaseOrder.isValid())
                {
                    _view.ShowError("No se encontró una orden de compra con los datos proporcionados");
                    return;
                }

                //Se obtiene los detalles de compras relacionadas
                var purchasesDetailsForOrder = _purchases.Find(purchaseOrder);
                var purchaseOrderVM = new VMOrdenDeCompra(purchaseOrder, purchasesDetailsForOrder);

                purchaseOrderVM.Detalles.ForEach(x=>x.Cantidad = x.Pendiente);

                _view.Show(purchaseOrderVM, purchaseOrderVM.Detalles.Select(x => x.ToDetalleDeCompra(_items)).ToList());
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void OpenPurchaseOrdersList()
        {
            try
            {
                IPurchaseOrdersListView view;
                PurchaseOrdersListPresenter presenter;

                //Por implementar filtro por proveedor
                view = new PurchaseOrdersListView();
                presenter = new PurchaseOrdersListPresenter(view, _purchaseOrders,_view.Purchase.idProveedor, true);

                view.ShowWindow();

                if (view.Order.isValid() && view.Order.idOrdenDeCompra.isValid())
                {
                    //Se obtiene los detalles de compras relacionadas
                    var purchasesDetailsForOrder = _purchases.Find(view.Order);
                    var purchaseOrder = new VMOrdenDeCompra(view.Order, purchasesDetailsForOrder);

                    purchaseOrder.Detalles.ForEach(x => x.Cantidad = x.Pendiente);

                    _view.Show(purchaseOrder, purchaseOrder.Detalles.Select(x=>x.ToDetalleDeCompra(_items)).ToList());
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Print()
        {
            try
            {
                IPurchasePrintView view;
                PurchasePrintPresenter presenter;

                view = new PurchasePrintView(_view.Purchase);
                presenter = new PurchasePrintPresenter(view, _purchases, _suppliers);

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Save()
        {
            string error;

            if (_view.IsDirty)
            {
                _view.ShowError("No se pueden editar las compras registradas");
                return;
            }

            if(!IsPurchaseValid(_view.Purchase, out error))
            {
                _view.ShowError(error);
                return;
            }                 

            try
            {
                var purchase = _view.Purchase.ToCompra();
                //Antes de agregar la compra le asigno el usuario que la va a registrar
                purchase.idUsuarioRegistro = Session.LoggedUser.idUsuario;
                //Y el estatus de la compra

                if (_view.Purchase.Saldo.Equals(0.0))
                    purchase.idEstatusDeCompra = (int)StatusDeCompra.Pagada;
                else
                    purchase.idEstatusDeCompra = (int)StatusDeCompra.Crédito;

                if(purchase.DetallesDeCompras.Any(i => i.Articulo.importado))
                {
                    ICustomsApplicationView view;
                    CustomsApplicationPresenter presenter;

                    view = new CustomsApplicationView();
                    presenter = new CustomsApplicationPresenter(view);

                    view.ShowWindow();

                    purchase.Pedimentos = new List<Pedimento>();
                    purchase.Pedimentos.Add(view.CustomsApplication);
                }

                //Debo registrar la compra
                _purchases.Add(purchase);

                //Le mando mensaje al usuario
                _view.ShowMessage(string.Format("Compra {0} registrada exitosamente al proveedor {1}", _view.Purchase.folio, _view.Purchase.Proveedore.codigo));

                //Limpio la ventana
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenPayments()
        {
            try
            {
                string error;
                IPurchasePaymentsView view;
                PurchasePaymentsPresenter presenter;

                //Verifico que la compra ya este lista para pagarse
                if(!IsPurchaseValid(_view.Purchase,out error))
                {
                    _view.ShowError(string.Format("Antes de abonar debe corregir lo siguiente: {0}", error));
                    return;
                }

                view = new PurchasePaymentsView(_view.Purchase);
                presenter = new PurchasePaymentsPresenter(view, _payments, _catalogs, _paymentForms,_purchases);

                view.ShowWindow();

                //Le paso a la compra los abonos que se hayan registrado
                var purchase = _view.Purchase;
                purchase.AbonosDeCompras = view.Purchase.AbonosDeCompras;

                //Actualizo la cuenta
                purchase.UpdateAccount();

                //Muestro la compra actualizada
                _view.Show(purchase);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Cancel()
        {
            if(!_view.IsDirty)
            {
                _view.ShowError("Debe seleccionar una compra");
                return;
            }

            try
            {
                //Se solicita el motivo de la cancelacion
                ICancellationView view;
                CancellationPresenter presenter;

                view = new CancellationView();
                presenter = new CancellationPresenter(view);

                view.ShowWindow();

                //La cancelo
                var purchase = _purchases.Cancel(_view.Purchase, view.Reason);

                //Envio mensaje al usuario
                _view.ShowMessage(string.Format("Compra {0} del proveedor {1} cancelada exitosamente", purchase.folio, purchase.Proveedore.codigo));

                //Actualizo la compra mostrada
                _view.Show(new VMCompra(purchase));

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
                IPurchasesListView view;
                PurchasesListPresenter presenter;

                //Le paso el id del proveedor para que solo me muestre compras de este.
                view = new PurchasesListView(_view.Purchase.idProveedor);
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

        private void Find()
        {
            VMCompra purchase;

            if(!_view.Purchase.Proveedore.idProveedor.isValid())
            {
                _view.ShowError("Debe seleccionar un proveedor para poder buscar una compra");
                return;
            }

            if (!_view.Purchase.folio.isValid())
                return;

            try
            {
                var local = _purchases.Find(_view.Purchase.Proveedore.idProveedor, _view.Purchase.folio);

                //Si no encontre ninguna, inicializo una con el proveedor y el folio
                if (local == null)
                    purchase = new VMCompra(_view.Purchase.Proveedore, _view.Purchase.folio);
                else //Si si la encontre inicializo el ViewModel a partir de la compra
                    purchase = new VMCompra(local);                    

                _view.Show(purchase);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void ViewTaxDetails()
        {
            try
            {
                if (_view.SelectedTax.idImpuesto < 0)
                {
                    _view.ShowError("No existe ningún impuesto seleccionado");
                    return;
                }

                _view.Show(_view.SelectedTax);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void RemoveItem()
        {
            if (_view.IsDirty)
            {
                _view.ShowError("No se pueden editar las compras registradas");
                return;
            }

            if (!_view.SelectedItem.idArticulo.isValid())
            {
                _view.ShowError("No existe ningún detalle seleccionado");
                return;
            }

            try
            {
                var item = _view.SelectedItem;

                //Lo elimino del detalle
                var purchase = _view.Purchase;
                purchase.DetallesDeCompras.Remove(item);

                //Recalculo
                purchase.UpdateAccount();

                //Muestro la compra sin el articulo
                _view.Show(purchase);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void SelectItem()
        {
            if (_view.IsDirty)
            {
                _view.ShowError("No se pueden editar las compras registradas");
                return;
            }

            if(!_view.SelectedItem.idArticulo.isValid())
            {
                _view.ShowError("No existe ningún detalle seleccionado");
                return;
            }

            try
            {
                var item = _view.SelectedItem;

                //Lo elimino del detalle
                var purchase = _view.Purchase;
                purchase.DetallesDeCompras.Remove(item);

                purchase.UpdateAccount();

                //Muestro la compra sin el articulo
                _view.Show(purchase);

                //Muestro en edición el artículo seleccionado
                _view.Show(item, _unitsOfMeasure.List(item.idArticulo));
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void AddItem()
        {
            if (_view.IsDirty)
            {
                _view.ShowError("No se pueden editar las compras registradas");
                return;
            }

            if(!_view.CurrentItem.idArticulo.isValid())
            {
                _view.ShowError("Artículo inválido");
                return;
            }

            if(!_view.CurrentItem.cantidad.isValid())
            {
                _view.ShowError("Cantidad inválida");
                return;
            }

            if(!_view.CurrentItem.costoUnitario.isValid())
            {
                _view.ShowError("Costo unitario inválido");
                return;
            }

            if(!_view.CurrentItem.idUnidadDeMedida.isValid())
            {
                _view.ShowError("Unidad de medida inválida");
                return;
            }

            try
            {
                //Obtengo de manera local la compra y el detalle a agregar
                var purchase = _view.Purchase;
                var detail = _view.CurrentItem;

                //Busco los impuestos que el articulo debe pagar
                detail.Articulo = _items.Find(detail.idArticulo);
                detail.Impuestos = detail.Articulo.Impuestos;

                //Si la compra viene de una orden de compra, se deben validar las cantidades con las de la orden de compra
                if (purchase.idOrdenDeCompra.HasValue && purchase.idOrdenDeCompra.Value.isValid())
                {
                    //Se obtiene el detalle de la orden de compra correspondiente
                    var purchaseOrder = _view.PurchaseOrder;

                    if (purchaseOrder.isValid() && purchaseOrder.idOrdenDeCompra.isValid())
                    {
                        var orderDetail = purchaseOrder.Detalles.FirstOrDefault(x => x.idArticulo == detail.idArticulo);

                        //Solo se deben agregar articulos de la orden de compra
                        if (!orderDetail.isValid())
                        {
                            _view.ShowError("Solo se pueden agregar artículos de la orden de compra");
                            return;
                        }

                        //Solo se puede agregar hasta la cantidad pendiente de surtir
                        //Se obtiene el total del detalle de la orden de compra en unidades
                        Articulo item = _items.Find(orderDetail.idArticulo);

                        //Se obtiene el total del detalle en unidades
                        var equivalenciaDetail = item.Equivalencias.ToList().FirstOrDefault(e => e.idUnidadDeMedida.Equals(detail.idUnidadDeMedida));
                        decimal totalDetailUnits = (equivalenciaDetail.isValid()?equivalenciaDetail.unidades:1m) * detail.cantidad;

                        if (totalDetailUnits > _purchaseOrders.PendingUnitsForOrderItem(purchase.idOrdenDeCompra.Value,item.idArticulo))
                        {
                            _view.ShowError("Solo se puede agregar una cantidad de artículos menor o igual a la cantidad de artículos pendientes de la orden de compra");
                            return;
                        }
                    }
                }

                //Agrego el artículo al detalle
                purchase.DetallesDeCompras.Add(detail);
                
                //Actualizo la cuenta a través de un extension
                purchase = purchase.UpdateAccount();

                //Muestro la compra
                _view.Show(purchase);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenItemsList()
        {
            if (_view.IsDirty)
            {
                _view.ShowError("No se pueden editar las compras registradas");
                return;
            }

            if (!_view.Purchase.tipoDeCambio.isValid())
            {
                _view.ShowError("Debe capturar el tipo de cambio de la compra antes de agregar artículos");
                return;
            }

            try
            {
                IItemsListView view;
                ItemsListPresenter presenter;

                view = new ItemsListView(true);
                presenter = new ItemsListPresenter(view, _items);

                view.ShowWindow();

                if(view.Item.idArticulo.isValid())
                {
                    //La lista de articulos ahora regresa una viewModel, se debe obtener el item correspondiente
                    var item = _items.Find(view.Item.idArticulo);

                    var itemDetail = GetDetail(item);
                    _view.Show(itemDetail, _unitsOfMeasure.List(itemDetail.idArticulo));
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindItem()
        {
            if (!_view.Purchase.idProveedor.isValid())
                return;

            if(_view.IsDirty)
            {
                _view.ShowError("No se pueden editar las compras registradas");
                return;
            }

            if(!_view.Purchase.idMoneda.isValid())
            {
                _view.ShowError("Debe seleccionar la moneda de la compra antes de agregar artículos");
                return;
            }

            if(!_view.Purchase.tipoDeCambio.isValid())
            {
                _view.ShowError("Debe capturar el tipo de cambio de la compra antes de agregar artículos");
                return;
            }

            if (!_view.CurrentItem.Articulo.codigo.isValid())
                return;

            try
            {
                //Toda esta parte se trata de obtener el artículo que el cliente busca
                var item = new VMArticulo();
                var items = _items.FindAllForSupplier(_view.CurrentItem.Articulo.codigo, _view.Purchase.idProveedor).Select(a => new VMArticulo(a)).ToList();

                if (items.IsEmpty())
                {
                    _view.ShowError("No existe ningún artículo con el código especificado");
                    _view.ClearItem();
                    return;
                }

                if(!items.HasSingleItem())
                {
                    //Se muestra los artículos que coinciden
                    IItemSelectionView view;
                    ItemSelectionPresenter presenter;

                    view = new ItemSelectionView();
                    presenter = new ItemSelectionPresenter(view, items);

                    view.ShowWindow();

                    //Se valida que se haya escogido algún artículo
                    if (!view.Item.isValid() || !view.Item.idArticulo.isValid())
                    {
                        _view.ClearItem();
                        throw new Exception("No se seleccionó ninguna de las coincidencias");
                    }

                    item = view.Item;
                }
                else
                {
                    item = items.First();
                }

                //Articulo item = _items.Find(_view.CurrentItem.Articulo.codigo); JL: Innecesario
                //VMArticulo itemVM = null;

                ////Si no se encontro el articulo por el código, se procede a buscar en los codigos alternos
                //if (!item.isValid())
                //{
                //    List<VMArticulo> items = _items.FindAllForProvider(_view.CurrentItem.Articulo.codigo, _view.Purchase.idProveedor).Select(x => new VMArticulo(x)).ToList();

                //    if (items.IsEmpty())
                //    {
                //        _view.ShowError("No existe ningún artículo con ese código");
                //        _view.ClearItem();
                //        return;
                //    }

                //    if (!items.HasSingleItem() && !items.IsEmpty()) JL: Si pasó la condición de arriba de item.IsEmpty() validar el inverso aqui es Innecesario
                //    {
                //        //Se muestra la vista de seleccion
                //        IItemSelectionView view;
                //        ItemSelectionPresenter presenter;

                //        view = new ItemSelectionView(true); JL: Innecesario especificarle si muestra activos o no
                //        presenter = new ItemSelectionPresenter(view, items);

                //        view.ShowWindow();

                //        itemVM = view.Item; JL: Porque asignarlo sin antes validar?

                //        //Se valida que se haya escogido algun articulo
                //        if (!itemVM.isValid() || !itemVM.idArticulo.isValid() && !items.IsEmpty())
                //        {
                //            _view.ClearItem();
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        itemVM = items.FirstOrDefault();
                //    }

                //    item = _items.Find(itemVM.idArticulo); JL: Innecesario buscar nuevamente el artículo
                //}

                ////Aqui ya se tiene el item, sea por el codigo original o el alterno
                //if (!item.activo) JL: Innecesario revisar si esta activo o no cuando puedes filtrarlo desde la vista
                //{
                //    _view.ClearItem();
                //    return;
                //}

                //Si tiene unidades de medida equivalentes hay que cargarlas en el combo
                var itemDetail = GetDetail(item);

                _view.Show(itemDetail, _unitsOfMeasure.List(itemDetail.idArticulo));
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenSuppliersList()
        {
            try
            {
                ISuppliersListView view;
                SuppliersListPresenter presenter;

                view = new SuppliersListView();
                presenter = new SuppliersListPresenter(view, _suppliers);

                view.ShowWindow();

                //Si seleccionó alguno lo muestro
                if (view.Supplier.idProveedor.isValid())
                    _view.Show(new VMCompra(view.Supplier));
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindSupplier()
        {
            if (!_view.Purchase.Proveedore.codigo.isValid())
                return;

            try
            {
                var supplier = _suppliers.Find(_view.Purchase.Proveedore.codigo);

                if(!supplier.isValid())
                {
                    _view.ShowError("No existe ningún proveedor con este código");
                    _view.Clear();
                    return;
                }

                var purchase = new VMCompra(supplier);

                _view.Show(purchase);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        #region Private Utility Functions

        private bool IsPurchaseValid(Compra purchase, out string error)
        {
            if (!_view.Purchase.idProveedor.isValid())
            {
                error = "El proveedor no es válido";
                return false;
            }

            if (!_view.Purchase.folio.isValid())
            {
                error = "El folio no es válido";
                return false;
            }

            if (!_view.Purchase.tipoDeCambio.isValid())
            {
                error = "El tipo de cambio no es válido";
                return false;
            }

            if (!_view.Purchase.fechaHora.isValid())
            {
                error = "La fecha no es válida";
                return false;
            }

            if (_view.Purchase.DetallesDeCompras.Count.Equals(0))
            {
                error = "La compra debe tener al menos un artículo";
                return false;
            }

            error = string.Empty;
            return true;
        }

        private DetallesDeCompra GetDetail(Articulo item)
        {
            //Si lo encontré preparo un Detalle de Venta
            var detail = new DetallesDeCompra();
            detail.Articulo = item;
            detail.idArticulo = item.idArticulo;
            detail.cantidad = 1.0m;

            //Si la moneda del artículo es distinta a la del documento, calcular el precio unitario en base a la moneda correcta
            detail.costoUnitario = item.costoUnitario.ToDocumentCurrency(item.Moneda, _view.Purchase.Moneda, _view.Purchase.tipoDeCambio);
            detail.Impuestos = item.Impuestos;
            detail.idUnidadDeMedida = item.idUnidadDeMedida;
            detail.UnidadesDeMedida = item.UnidadesDeMedida;

            return detail;
        }

        #endregion
    }
}
