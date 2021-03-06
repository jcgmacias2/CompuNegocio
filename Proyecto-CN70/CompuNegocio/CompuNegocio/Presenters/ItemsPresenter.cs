using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Linq;
using Aprovi.Application.Helpers;
using Aprovi.Business.Helpers;
using Aprovi.Business.ViewModels;

namespace Aprovi.Presenters
{
    public class ItemsPresenter
    {
        private readonly IItemsView _view;
        private IArticuloService _items;
        private IImpuestoService _taxes;
        private ICatalogosEstaticosService _catalogs;
        private IUnidadDeMedidaService _unitsOfMeasure;
        private IClasificacionService _classifications;
        private IEquivalenciaService _equivalencies;
        private IProductoServicioService _productServices;
        private IAjusteService _adjustments;
        private IPedimentoService _customsApplications;
        private IProveedorService _suppliers;
        private IClienteService _customers;
        private ICodigoDeArticuloPorProveedorService _supplierCodes;
        private ICodigoDeArticuloPorClienteService _customerCodes;

        public ItemsPresenter(IItemsView view, IArticuloService itemsService, IImpuestoService taxesService, ICatalogosEstaticosService catalogs, IUnidadDeMedidaService unitsOfMeasureService, IClasificacionService classificationsService, IEquivalenciaService equivalenciesService, IProductoServicioService productsServicesService, IAjusteService adjustments, IPedimentoService customsApplications, IProveedorService suppliers, ICodigoDeArticuloPorProveedorService codes, IClienteService customers, ICodigoDeArticuloPorClienteService customerCodes)
        {
            _view = view;
            _items = itemsService;
            _taxes = taxesService;
            _catalogs = catalogs;
            _unitsOfMeasure = unitsOfMeasureService;
            _classifications = classificationsService;
            _equivalencies = equivalenciesService;
            _productServices = productsServicesService;
            _adjustments = adjustments;
            _customsApplications = customsApplications;
            _suppliers = suppliers;
            _supplierCodes = codes;
            _customers = customers;
            _customerCodes = customerCodes;

            _view.Quit += Quit;
            _view.New += New;
            _view.Find += Find;
            _view.Delete += Delete;
            _view.Save += Save;
            _view.Update += Update;
            _view.OpenList += OpenList;
            _view.OpenEquivalencies += OpenEquivalencies;
            _view.AddClassification += AddClasification;
            _view.DeleteClassification += DeleteClassification;
            _view.AddTax += AddTax;
            _view.CalculateByUtility += CalculateByUtility;
            _view.FindProductService += FindProductService;
            _view.OpenProductServiceList += OpenProductServiceList;
            _view.DeleteTax += DeleteTax;
            _view.ChangeCurrency += ChangeCurrency;
            _view.AddSupplierCode += AddSupplierCode;
            _view.DeleteSupplierCode += DeleteSupplierCode;
            _view.FindSupplier += FindSupplier;
            _view.OpenSuppliersList += OpenSuppliersList;
            _view.AddCustomerCode += AddCustomerCode;
            _view.DeleteCustomerCode += DeleteCustomerCode;
            _view.FindCustomer += FindCustomer;
            _view.OpenCustomersList += OpenCustomersList;

            _view.FillCombos(_catalogs.ListMonedas(), _unitsOfMeasure.List(), _catalogs.ListComisiones());
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
                    _view.Show(new CodigosDeArticuloPorProveedor() { codigo = _view.CurrentSupplierCode.codigo, Proveedore = view.Supplier });
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindSupplier()
        {
            try
            {
                if (!_view.CurrentSupplierCode.isValid() || !_view.CurrentSupplierCode.codigo.isValid())
                    throw new Exception("Debe capturar el código alterno antes de seleccionar un proveedor");

                var current = _view.CurrentSupplierCode;

                if (!current.Proveedore.isValid() || !current.Proveedore.codigo.isValid())
                {
                    current.Proveedore = new Proveedore();
                    _view.Show(current);
                    return;
                }

                var supplier = _suppliers.Find(current.Proveedore.codigo);

                if (!supplier.isValid())
                {
                    current.Proveedore = new Proveedore();
                    _view.Show(current);
                    throw new Exception("No existe ningún proveedor con este código");
                }
                else
                {
                    current.Proveedore = supplier;
                    _view.Show(current);
                }

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void DeleteSupplierCode()
        {
            try
            {
                var selected = _view.SelectedSupplierCode;

                if (!selected.isValid() || !selected.idCodigoDeArticuloPorProveedor.isValid())
                    return;

                _supplierCodes.Remove(selected);

                _view.ShowMessage("Código removido exitosamente");
                _view.Show(_view.Item.CodigosDeArticuloPorProveedors.ToList());
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void AddSupplierCode()
        {
            try
            {
                var current = _view.CurrentSupplierCode;

                if (!current.idArticulo.isValid())
                    throw new Exception("Debe existir un artículo selecionado para agregar códigos alternos");

                if (!current.codigo.isValid())
                    throw new Exception("Debe capturar el código alterno para el producto");

                //El proveedor es opcional
                if (!current.Proveedore.isValid() || !current.Proveedore.idProveedor.isValid())
                    current.Proveedore = null;

                var codes = _view.SupplierCodes;
                //Si existe, cambio el código nuevo por el anterior
                //Si no existe, reviso si ya existe el código aún sin proveedor
                var code = new CodigosDeArticuloPorProveedor();
                var item = _view.Item;
                //Si el proveedor es válido
                if (current.Proveedore.isValid())
                {
                    //Reviso si ya existe un código para este proveedor
                    code = codes.FirstOrDefault(c => c.idProveedor.HasValue && c.idProveedor.Equals(current.Proveedore.idProveedor));

                    //Si ya existe solo lo actualizo, si no entonces lo agrego
                    if (code.isValid())
                    {
                        code.codigo = current.codigo;
                        _supplierCodes.Update(code);
                    }
                    else
                    {
                        code = _supplierCodes.Add(new CodigosDeArticuloPorProveedor() { idArticulo = current.idArticulo, codigo = current.codigo, idProveedor = current.Proveedore.idProveedor, Proveedore = current.Proveedore });
                        item.CodigosDeArticuloPorProveedors.Add(code);
                    }

                }
                else //Si el proveedor es null
                {
                    //Reviso si existe el código entre los que no tienen proveedor
                    code = codes.FirstOrDefault(c => !c.idProveedor.HasValue && c.codigo.Equals(current.codigo, StringComparison.InvariantCultureIgnoreCase));
                    //Si existe mando un aviso
                    if (code.isValid())
                        throw new Exception("Este código ya existe");

                    //Si no, entonces si lo agrego
                    code = _supplierCodes.Add(new CodigosDeArticuloPorProveedor() { idArticulo = current.idArticulo, codigo = current.codigo });
                    item.CodigosDeArticuloPorProveedors.Add(code);
                }


                _view.ShowMessage("Código alterno registrado exitosamente");
                _view.Show(item.CodigosDeArticuloPorProveedors.ToList());
                _view.ClearSupplierCode();
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
                IClientsListView view;
                ClientsListPresenter presenter;

                view = new ClientsListView();
                presenter = new ClientsListPresenter(view, _customers);

                view.ShowWindow();

                //Si seleccionó alguno lo muestro
                if (view.Client.idCliente.isValid())
                    _view.Show(new CodigosDeArticuloPorCliente() { codigo = _view.CurrentSupplierCode.codigo, Cliente = view.Client });
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindCustomer()
        {
            try
            {
                if (!_view.CurrentCustomerCode.isValid() || !_view.CurrentCustomerCode.codigo.isValid())
                    throw new Exception("Debe capturar el código alterno antes de seleccionar un cliente");

                var current = _view.CurrentCustomerCode;

                if (!current.Cliente.isValid() || !current.Cliente.codigo.isValid())
                {
                    current.Cliente = new Cliente();
                    _view.Show(current);
                    return;
                }

                var customer = _customers.Find(current.Cliente.codigo);

                if (!customer.isValid())
                {
                    current.Cliente = new Cliente();
                    _view.Show(current);
                    throw new Exception("No existe ningún cliente con este código");
                }
                else
                {
                    current.Cliente = customer;
                    _view.Show(current);
                }

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void DeleteCustomerCode()
        {
            try
            {
                var selected = _view.SelectedCustomerCode;

                if (!selected.isValid() || !selected.idCodigoDeArticuloPorCliente.isValid())
                    return;

                _customerCodes.Remove(selected);

                _view.ShowMessage("Código removido exitosamente");
                _view.Show(_view.Item.CodigosDeArticuloPorClientes.ToList());
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void AddCustomerCode()
        {
            try
            {
                var current = _view.CurrentCustomerCode;

                if (!current.idArticulo.isValid())
                    throw new Exception("Debe existir un artículo selecionado para agregar códigos alternos");

                if (!current.codigo.isValid())
                    throw new Exception("Debe capturar el código alterno para el producto");

                //El cliente es opcional
                if (!current.Cliente.isValid() || !current.Cliente.idCliente.isValid())
                    current.Cliente = null;

                var codes = _view.CustomerCodes;
                //Si existe, cambio el código nuevo por el anterior
                //Si no existe, reviso si ya existe el código aún sin cliente
                var code = new CodigosDeArticuloPorCliente();
                var item = _view.Item;
                //Si el cliente es válido
                if (current.Cliente.isValid())
                {
                    //Reviso si ya existe un código para este cliente
                    code = codes.FirstOrDefault(c => c.idCliente.HasValue && c.idCliente.Equals(current.Cliente.idCliente));

                    //Si ya existe solo lo actualizo, si no entonces lo agrego
                    if (code.isValid())
                    {
                        code.codigo = current.codigo;
                        _customerCodes.Update(code);
                    }
                    else
                    {
                        code = _customerCodes.Add(new CodigosDeArticuloPorCliente() { idArticulo = current.idArticulo, codigo = current.codigo, idCliente = current.Cliente.idCliente, Cliente = current.Cliente});
                        item.CodigosDeArticuloPorClientes.Add(code);
                    }

                }
                else //Si el cliente es null
                {
                    //Reviso si existe el código entre los que no tienen cliente
                    code = codes.FirstOrDefault(c => !c.idCliente.HasValue && c.codigo.Equals(current.codigo, StringComparison.InvariantCultureIgnoreCase));
                    //Si existe mando un aviso
                    if (code.isValid())
                        throw new Exception("Este código ya existe");

                    //Si no, entonces si lo agrego
                    code = _customerCodes.Add(new CodigosDeArticuloPorCliente() { idArticulo = current.idArticulo, codigo = current.codigo });
                    item.CodigosDeArticuloPorClientes.Add(code);
                }


                _view.ShowMessage("Código alterno registrado exitosamente");
                _view.Show(item.CodigosDeArticuloPorClientes.ToList());
                _view.ClearCustomerCode();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void ChangeCurrency()
        {
            try
            {
                var item = _view.Item;
                var lastCurrency = _view.LastCurrency;
                var newCurrency = item.Moneda;

                //Alguna de las monedas no es valida
                if (!lastCurrency.isValid() || !lastCurrency.idMoneda.isValid() || !newCurrency.isValid() || !newCurrency.idMoneda.isValid())
                {
                    return;
                }

                //No se cambio la moneda
                if (newCurrency.idMoneda == lastCurrency.idMoneda)
                {
                    return;
                }

                //Si se esta editando un articulo, se verifica si el articulo tiene transacciones relacionadas
                if (_view.IsDirty && _items.HasTransactions(item))
                {
                    _view.ShowError("No es posible actualizar la moneda a un artículo con transacciones registradas");
                    _view.Show(lastCurrency);
                    return;
                }

                item.costoUnitario = item.costoUnitario.ToDocumentCurrency(lastCurrency, newCurrency, Session.Configuration.tipoDeCambio);

                //Se debe cambiar la moneda al articulo tambien
                item.Moneda = newCurrency;
                item.idMoneda = newCurrency.idMoneda;

                _view.Show(item);

                //Se deben actualizar los precios
                CalculateByUtility();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void DeleteTax()
        {
            try
            {
                //var taxes = _view.Taxes;
                var selectedTax = _view.SelectedTax;

                if (!selectedTax.isValid())
                {
                    _view.ShowError("No se ha seleccionado un impuesto a eliminar");
                    return;
                }

                //Se asigna la lista de impuestos nueva al articulo
                _view.Item.Impuestos.Remove(selectedTax);

                _view.Show(_view.Item.Impuestos.ToList());
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void OpenProductServiceList()
        {
            try
            {
                IProductsServicesListView view;
                ProductsServicesListPresenter presenter;

                view = new ProductsServicesListView();
                presenter = new ProductsServicesListPresenter(view, _productServices);

                view.ShowWindow();

                if (view.ProductService.isValid() && view.ProductService.idProductoServicio.isValid())
                    _view.Show(view.ProductService);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindProductService()
        {
            if (!_view.Item.isValid())
                return;

            if (!_view.Item.ProductosServicio.codigo.isValid())
                return;

            try
            {
                var productService = _productServices.Find(_view.Item.ProductosServicio.codigo);

                if (!productService.isValid() || !productService.idProductoServicio.isValid())
                    throw new Exception("No se encontró ningún producto o servicio con este código");

                _view.Show(productService);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void CalculateByUtility()
        {
            if (!_view.Item.isValid())
                return;

            if (!_view.Item.costoUnitario.isValid())
            {
                _view.ShowMessage("El artículo debe tener un costo registrado");
                return;
            }

            try
            {
                var item = _view.Item;

                //Debo calcular precio sin impuesto y precio con impuestos
                item.PrecioA = Operations.CalculatePriceWithoutTaxes(item.costoUnitario, item.UtilidadA);
                item.PrecioConImpuestosA = item.PrecioA + Operations.CalculateTaxes(item.PrecioA, item.Impuestos.ToList());

                item.PrecioB = Operations.CalculatePriceWithoutTaxes(item.costoUnitario, item.UtilidadB);
                item.PrecioConImpuestosB = item.PrecioB + Operations.CalculateTaxes(item.PrecioB, item.Impuestos.ToList());

                item.PrecioC = Operations.CalculatePriceWithoutTaxes(item.costoUnitario, item.UtilidadC);
                item.PrecioConImpuestosC = item.PrecioC + Operations.CalculateTaxes(item.PrecioC, item.Impuestos.ToList());

                item.PrecioD = Operations.CalculatePriceWithoutTaxes(item.costoUnitario, item.UtilidadD);
                item.PrecioConImpuestosD = item.PrecioD + Operations.CalculateTaxes(item.PrecioD, item.Impuestos.ToList());

                _view.Show(item);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void AddTax()
        {
            try
            {
                ITaxesListView view;
                TaxesListPresenter presenter;

                view = new TaxesListView();
                presenter = new TaxesListPresenter(view, _taxes, false);

                view.ShowWindow();

                //Si no se selecciono nada aqui termina
                if (!view.Tax.isValid() || !view.Tax.idImpuesto.isValid())
                    return;

                //Reviso que no sea un impuesto repetido
                if (!_view.Item.Impuestos.isValid() || _view.Item.Impuestos.ToList().Exists(i => i.idImpuesto.Equals(view.Tax.idImpuesto)))
                    return;

                //Agrego el impuesto a la lista de impuestos del artículo
                var item = _view.Item;
                item.Impuestos.Add(view.Tax);

                //Si es un artículo en edición guardo el cambio hasta la base de datos
                if (_view.IsDirty)
                {
                    item.activo = true;
                    _items.Update(item);
                }

                //Debo calcular precio sin impuesto y precio con impuestos
                item.PrecioA = Operations.CalculatePriceWithoutTaxes(item.costoUnitario, item.UtilidadA);
                item.PrecioConImpuestosA = item.PrecioA + Operations.CalculateTaxes(item.PrecioA, item.Impuestos.ToList());

                item.PrecioB = Operations.CalculatePriceWithoutTaxes(item.costoUnitario, item.UtilidadB);
                item.PrecioConImpuestosB = item.PrecioB + Operations.CalculateTaxes(item.PrecioB, item.Impuestos.ToList());

                item.PrecioC = Operations.CalculatePriceWithoutTaxes(item.costoUnitario, item.UtilidadC);
                item.PrecioConImpuestosC = item.PrecioC + Operations.CalculateTaxes(item.PrecioC, item.Impuestos.ToList());

                item.PrecioD = Operations.CalculatePriceWithoutTaxes(item.costoUnitario, item.UtilidadD);
                item.PrecioConImpuestosD = item.PrecioD + Operations.CalculateTaxes(item.PrecioD, item.Impuestos.ToList());

                //Actualizo el artículo que se esta mostrando
                _view.Show(item);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void AddClasification()
        {
            try
            {
                IClassificationsListView view;
                ClassificationsListPresenter presenter;

                view = new ClassificationsListView();
                presenter = new ClassificationsListPresenter(view, _classifications);

                view.ShowWindow();

                //Si no selecciono nada aquí termina
                if (!view.Classification.isValid() || !view.Classification.idClasificacion.isValid())
                    return;

                //Reviso que no sea una clasificación repetida
                if (!_view.Item.Clasificaciones.isValid() || _view.Item.Clasificaciones.ToList().Exists(c => c.idClasificacion.Equals(view.Classification.idClasificacion)))
                    return;

                //Agrego la clasificación a la lista de clasificaciones del artículo
                var item = _view.Item;
                item.Clasificaciones.Add(view.Classification);

                //Si es un artículo en edición guardo el cambio hasta la base de datos
                if (_view.IsDirty)
                {
                    item.activo = true;
                    _items.Update(item);
                }

                //Envío mensaje al usuario
                _view.ShowMessage("Clasificación agregada exitosamente");

                //Actualizo el artículo que se esta mostrando
                _view.Show(item.Clasificaciones.ToList());
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void DeleteClassification()
        {
            if (!_view.CurrentClassification.isValid())
            {
                _view.ShowError("No hay una clasificación seleccionada en edición");
                return;
            }

            try
            {
                //Remuevo la clasificación de la lista de clasificaciones del artículo
                var item = _view.Item;
                item.Clasificaciones.Remove(_view.CurrentClassification);

                //Si es un artículo en edición guardo el cambio hasta la base de datos
                if (_view.IsDirty)
                {
                    item.activo = true;
                    _items.Update(item);
                }

                //Envío mensaje al usuario
                _view.ShowMessage("Clasificación removida exitosamente");

                //Actualizo la lista que se esta mostrando
                _view.Show(item.Clasificaciones.ToList());
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
                IItemsListView view;
                ItemsListPresenter presenter;

                view = new ItemsListView(false);
                presenter = new ItemsListPresenter(view, _items);

                view.ShowWindow();

                //Si seleccionó uno lo muestro
                if (view.Item.idArticulo.isValid())
                {
                    //La lista de articulos ahora regresa una viewModel, se debe obtener el item correspondiente
                    var item = _items.Find(view.Item.idArticulo);

                    _view.Show(new VMArticulo(item, item.inventariado ? _items.Stock(view.Item.idArticulo) : 0.0m));
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenEquivalencies()
        {
            if (!_view.IsDirty)
            {
                _view.ShowMessage("Las equivalencias solo se pueden editar para articulos registrados");
                return;
            }

            try
            {
                IEquivalenciesView view;
                EquivalenciesPresenter presenter;

                //Lo busco para obtener sus propiedades registradas, ya que tomaba la unidade medida del combo (puede estar cambiada)
                var item = _items.Find(_view.Item.idArticulo);
                item.Equivalencias = _equivalencies.List(item.idArticulo);

                view = new EquivalenciesView(item);
                presenter = new EquivalenciesPresenter(view, _equivalencies, _unitsOfMeasure);

                view.ShowWindow();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Update()
        {
            string error;

            if (!IsItemValid(_view.Item, out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                var importActivated = false;
                var measureUnitChanged = false;
                var item = _view.Item;
                item.activo = true;

                //Si importado esta marcado y esta inventariado y antes no lo tenía activo mi bandera
                importActivated = item.inventariado && item.importado && !_items.HasCustomsApplicationActive(item);

                //Se verifica si se cambio la unidad de medida
                var dbItem = _items.Find(item.idArticulo);
                measureUnitChanged = item.idUnidadDeMedida != dbItem.idUnidadDeMedida;

                //Hago la actualización tal cual es solicitada
                _items.Update(item.ToArticulo());

                _view.ShowMessage(string.Format("Artículo {0} actualizado exitosamente", _view.Item.codigo));

                //Corrección de equivalencias
                //Si se modifico la unidad de medida del articulo y este tiene equivalencias
                if (measureUnitChanged && !dbItem.Equivalencias.IsEmpty())
                {
                    IEquivalenciesView view;
                    EquivalenciesPresenter presenter;

                    view = new EquivalenciesView(dbItem);
                    presenter = new EquivalenciesPresenter(view, _equivalencies, _unitsOfMeasure);

                    view.ShowWindow();
                }

                //Activación de Pedimentos
                //Si el artículo tiene existencias
                if (importActivated && item.Existencia > 0.0m)
                {
                    //Deberá generarse un ajuste de salida por la cantidad de existencia en ese momento para que quede en 0.
                    var exitAdjustment = _adjustments.GenerateExit(_view.Item);
                    exitAdjustment.idUsuarioRegistro = Session.LoggedUser.idUsuario;
                    _adjustments.Add(exitAdjustment);

                    //-- Se le solicitará al usuario que capture los datos de al menos 1 pedimento por la cantidad de existencia previa.
                    //-- Utilizando estos datos se registrará un ajuste que incluya pedimento.
                    IItemsCustomsApplicationView view;
                    ItemsCustomsApplicationPresenter presenter;

                    view = new ItemsCustomsApplicationView(item);
                    presenter = new ItemsCustomsApplicationPresenter(view, _adjustments, _customsApplications);

                    view.ShowWindow();
                }
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Save()
        {
            string error;

            if (!IsItemValid(_view.Item, out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                var item = _view.Item;
                item.activo = true;
                _items.Add(item.ToArticulo());

                _view.ShowMessage(string.Format("Artículo {0} agregado exitosamente", _view.Item.codigo));
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Delete()
        {
            if (!_view.Item.isValid() || !_view.Item.idArticulo.isValid())
            {
                _view.ShowMessage("No existe artículo seleccionado para eliminar");
                return;
            }

            try
            {
                if (_items.CanDelete(_view.Item))
                {
                    _items.Delete(_view.Item);
                }
                else
                {
                    var local = _view.Item;
                    local.activo = false;
                    _items.Update(local.ToArticulo());
                }

                _view.ShowMessage(string.Format("Artículo {0} removido exitosamente", _view.Item.codigo));
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            if (!_view.Item.isValid() || !_view.Item.codigo.isValid())
                return;

            try
            {
                VMArticulo itemVM = null;

                var item = _items.Find(_view.Item.codigo);

                if (item.isValid() && item.idArticulo.isValid() && !item.activo)
                    _view.ShowMessage("El artículo ya existe pero esta marcado como inactivo, para reactivarlo solo de click en Guardar");

                if (!item.isValid() || !item.idArticulo.isValid())
                    itemVM = new VMArticulo() { codigo = _view.Item.codigo, inventariado = true, UtilidadA = 100.0m, UtilidadB = 100.0m, UtilidadC = 100.0m, UtilidadD = 100.0m };
                else//Se obtiene el articulo de la base de datos
                    itemVM = new VMArticulo(item, item.inventariado ? _items.Stock(item.idArticulo) : 0.0m);

                _view.Show(itemVM);
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

        private bool IsItemValid(VMArticulo item, out string error)
        {

            if (!item.codigo.isValid())
            {
                error = "El código no es válido";
                return false;
            }

            if(!item.idProductoServicio.isValid())
            {
                error = "El código del producto o servicio no es válido";
                return false;
            }

            if (!item.descripcion.isValid())
            {
                error = "La descripción no es válida";
                return false;
            }

            if(!item.idMoneda.isValid())
            {
                error = "Debe especificar la moneda";
                return false;
            }

            if (!item.costoUnitario.isValid())
            {
                error = "El costo no puede ser menor a 0.0";
                return false;
            }

            //Validar que las utilidades no sean mayores que la A
            if(item.UtilidadB > item.UtilidadA)
            {
                error = "La utilidad B no puede ser mayor a la utilidad A";
                return false;
            }

            if (item.UtilidadC > item.UtilidadA)
            {
                error = "La utilidad C no puede ser mayor a la utilidad A";
                return false;
            }

            if (item.UtilidadD > item.UtilidadA)
            {
                error = "La utilidad D no puede ser mayor a la utilidad A";
                return false;
            }

            //Solo pueden llevar pedimento los artículos inventariados
            if(item.importado && !item.inventariado)
            {
                error = "Solo pueden llevar pedimento los artículos inventariados";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }
}
