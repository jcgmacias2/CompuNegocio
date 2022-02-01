using Aprovi.Application.Helpers;
using Aprovi.Business.Helpers;
using Aprovi.Business.Services;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using Aprovi.Helpers;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using Microsoft.Practices.Unity;

namespace Aprovi.Presenters
{
    public class TransfersPresenter
    {
        private ITransfersView _view;
        private ITraspasoService _transfers;
        private ISolicitudDeTraspasoService _transferRequests;
        private IPedimentoService _customsApplications;
        private IEmpresaAsociadaService _associatedCompanies;
        private IEmpresaService _companies;
        private IArticuloService _items;

        public TransfersPresenter(ITransfersView view, ITraspasoService transfers, IEmpresaAsociadaService associatedCompanies, IEmpresaService companies, IArticuloService items, ISolicitudDeTraspasoService transferRequests, IPedimentoService customsApplications)
        {
            _view = view;

            _transfers = transfers;
            _associatedCompanies = associatedCompanies;
            _companies = companies;
            _items = items;
            _transferRequests = transferRequests;
            _customsApplications = customsApplications;

            _view.Save += Save;
            _view.Print += Print;
            _view.Reject += Reject;
            _view.New += New;
            _view.Quit += Quit;
            _view.SelectItem += SelectItem;
            _view.RemoveItem += RemoveItem;
            _view.AddItem += AddItem;
            _view.OpenItemsList += OpenItemsList;
            _view.FindItem += FindItem;
            _view.OpenList += OpenList;
            _view.Find += Find;
            _view.OpenDestinationAssociatedCompanyList += OpenDestinationAssociatedCompanyList;
            _view.FindDestinationAssociatedCompany += FindDestinationAssociatedCompany;
            _view.Load += Load;
            _view.Update += Update;
            _view.Approve += Approve;
            _view.LoadRemoteTransfer += LoadRemoteTransfer;
        }

        private void LoadRemoteTransfer()
        {
            try
            {
                var transferRequest = _view.Transfer.TransferRequest;

                //Se crea un contenedor con la base de datos remota
                AproviContainer remoteContainer = new AproviContainer(transferRequest.EmpresasAsociada.GetConnectionString());
                var container = remoteContainer.Container;

                var remoteTransfers = container.Resolve<ITraspasoService>();

                var transfer = remoteTransfers.FindByIdForTransfer(transferRequest.idTraspaso);

                //Se buscan las empresas asociadas en la base de datos local
                var localOriginAssociatedCompany = _associatedCompanies.FindByDatabaseName(transfer.EmpresasAsociada1.baseDeDatos);
                var localDestinationAssociatedCompany = _associatedCompanies.FindByDatabaseName(transfer.EmpresasAsociada.baseDeDatos);

                var vmTransfer = new VMTraspaso(transfer);

                //El traspaso debe tener las empresas asociadas de la base de datos local
                vmTransfer.folio = _transfers.Next();
                vmTransfer.TransferRequest = transferRequest;
                vmTransfer.EmpresasAsociada = localDestinationAssociatedCompany;
                vmTransfer.EmpresasAsociada1 = localOriginAssociatedCompany;
                vmTransfer.idEmpresaAsociadaOrigen = localOriginAssociatedCompany.idEmpresaAsociada;
                vmTransfer.idEmpresaAsociadaDestino = localDestinationAssociatedCompany.idEmpresaAsociada;

                _view.Show(vmTransfer);

                remoteContainer.Container.Dispose();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Approve()
        {
            try
            {
                //Esta es la parte de la integracion
                var transfer = _view.Transfer;
                var t = transfer.ToTraspaso();

                //Se debe verificar que se haya aceptado al menos 1 producto
                if (!t.DetallesDeTraspasoes.Any(x => x.cantidadAceptada.GetValueOrDefault(0m) > 0m))
                {
                    _view.ShowError("No se puede recibir un traspaso sin artículos aceptados");
                    return;
                }

                //Se establece el usuario que procesa el traspaso
                t.idUsuarioRegistro = Session.LoggedUser.idUsuario;

                //Para la integracion se deben efectuar los siguientes pasos:
                //Se verifica si se deben reasignar pedimentos
                if (t.DetallesDeTraspasoes.Any(x => x.cantidadAceptada != x.cantidadEnviada && x.cantidadAceptada.GetValueOrDefault(0m) != 0m && x.Articulo.importado))
                {
                    //Se vuelve a solicitar que se asignen los pedimentos
                    //Items a ajustar
                    var itemsToAdjust = t.DetallesDeTraspasoes.ReadjustImported();

                    ICustomsApplicationsExitView viewPedimentos = new CustomsApplicationsExitView(itemsToAdjust);
                    CustomsApplicationsEntrancePresenter presenterPedimentos = new CustomsApplicationsEntrancePresenter(viewPedimentos, _customsApplications, _items);

                    viewPedimentos.ShowWindow();

                    //Itero entre cada uno de los artículos que necesitaba pedimentos
                    foreach (var p in viewPedimentos.Items)
                    {
                        //Obtengo el detalle correspondiente al artículo
                        var d = transfer.DetallesDeTraspasoes.FirstOrDefault(i => i.idArticulo.Equals(p.Articulo.idArticulo));
                        //Por cada pedimento existente, se debe buscar la cantidad a asignar
                        foreach (var pdt in d.PedimentoPorDetalleDeTraspasoes)
                        {
                            var adjustedItem = p.Pedimentos.FirstOrDefault(x => x.IdPedimento == pdt.idPedimento);

                            if (adjustedItem.isValid())
                            {
                                pdt.cantidad = adjustedItem.Cantidad;
                            }
                            else
                            {
                                pdt.cantidad = 0m;
                            }
                        }
                    }
                }

                //Registrar el traspaso en la base de datos local
                t = _transfers.Approve(t);

                //Se deben registrar los detalles de la aprobacion en la base de datos local y remota
                AproviContainer remoteContainer = new AproviContainer(transfer.EmpresasAsociada1.GetConnectionString());
                var container = remoteContainer.Container;

                var remoteTransfers = container.Resolve<ITraspasoService>();

                var remoteTransfer = remoteTransfers.FindById(transfer.TransferRequest.idTraspaso);

                //Se asigna la fechaHora y folio a el traspaso original
                transfer.folio = t.folio;
                transfer.fechaHora = t.fechaHora;

                //Se actualiza el detalle del traspaso remoto
                remoteTransfers.EndLocalIntegration(remoteTransfer, transfer.ToTraspaso());

                //Se actualiza el folio y fechahora remotos
                _transfers.EndRemoteIntegration(t, remoteTransfer);

                container.Dispose();

                //Se debe eliminar la solicitud de traspaso de la base de datos local
                _transferRequests.Delete(transfer.TransferRequest);

                _view.ShowMessage("Traspaso procesado exitosamente");
                _view.CloseWindow();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void FindDestinationAssociatedCompany()
        {
            try
            {
                var transfer = _view.Transfer;

                if (!transfer.EmpresasAsociada.nombre.isValid())
                {
                    //Se limpia la empresa asociada actual por si habia una seleccionada anteriormente
                    transfer.EmpresasAsociada = new EmpresasAsociada();
                    _view.Show(transfer);
                    return;
                }

                var exists = _associatedCompanies.Find(transfer.EmpresasAsociada.nombre);

                if (exists.isValid())
                {
                    transfer.EmpresasAsociada = exists;
                    _view.Show(transfer);
                }
                else
                {
                    transfer.EmpresasAsociada = new EmpresasAsociada();
                    _view.Show(transfer);
                    _view.ShowError("No se encontró una empresa asociada con el nombre proporcionado");
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void OpenDestinationAssociatedCompanyList()
        {
            try
            {
                IAssociatedCompaniesListView view;
                AssociatedCompaniesListPresenter presenter;

                view = new AssociatedCompaniesListView();
                presenter = new AssociatedCompaniesListPresenter(view, _associatedCompanies);

                view.ShowWindow();

                if (view.AssociatedCompany.isValid() && view.AssociatedCompany.idEmpresaAsociada.isValid())
                {
                    var vm = _view.Transfer;
                    vm.EmpresasAsociada = view.AssociatedCompany;

                    _view.Show(vm);
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Reject()
        {
            try
            {
                var transfer = _view.Transfer;

                AproviContainer remoteContainer = new AproviContainer(transfer.EmpresasAsociada1.GetConnectionString());
                var container = remoteContainer.Container;
                var remoteTransfers = container.Resolve<ITraspasoService>();

                //Se cambia el status del traspaso en la base de datos origen
                remoteTransfers.Reject(transfer);

                //Se elimina la solicitud de traspaso
                _transferRequests.Delete(transfer.TransferRequest);

                _view.ShowMessage("Traspaso rechazado");
                _view.CloseWindow();

                container.Dispose();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Update()
        {
            try
            {
                //Se obtiene el pedido original
                VMTraspaso transfer = _view.Transfer;

                //Se verifica que la orden no haya sido surtida o cancelada
                if (transfer.idEstatusDeTraspaso != (int) StatusDeTraspaso.Registrado)
                {
                    throw new Exception("No se puede modificar un traspaso procesado");
                }

                if (transfer.TransferRequest.isValid() && transfer.TransferRequest.folio.isValid())
                {
                    throw new Exception("No se pueden agregar o eliminar artículos al procesar un traspaso");
                }

                _transfers.Update(transfer.ToTraspaso(), transfer.Detalle);

                _view.ShowMessage("Traspaso modificado exitosamente");
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
                //Se debe verificar que la empresa actual ya cuente con una empresa asociada
                var associatedCompany = Session.Station.Empresa.EmpresasAsociadas.FirstOrDefault();

                if (!associatedCompany.isValid())
                {
                    _view.CloseWindow();
                    throw new Exception("La empresa actual no cuenta con una empresa asociada");
                }

                VMTraspaso transfer = GetDefault();

                _view.Show(transfer);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private VMTraspaso GetDefault()
        {
            //Establezco los defaults
            var associatedCompany = Session.Station.Empresa.EmpresasAsociadas.FirstOrDefault();
            var transfer = new VMTraspaso();
            transfer.fechaHora = DateTime.Now;
            transfer.EmpresasAsociada = new EmpresasAsociada();
            transfer.TransferRequest = new SolicitudesDeTraspaso();
            transfer.EmpresasAsociada1 = associatedCompany;
            transfer.idEmpresaAsociadaOrigen = associatedCompany.idEmpresaAsociada;
            transfer.idEstatusDeTraspaso = (int)StatusDeTraspaso.Nuevo;
            transfer.folio = _transfers.Next();
            transfer.tipoDeCambio = Session.Configuration.tipoDeCambio;
            return transfer;
        }

        private void Find()
        {
            if (!_view.Transfer.folio.isValid())
                return;

            try
            {
                var transfer = _transfers.FindByFolio(_view.Transfer.folio.ToInt());

                if (transfer.isValid()) //Voy a mostrar un pedido existente
                {
                    _view.Show(new VMTraspaso(transfer));
                }
                else
                {
                    _view.Show(GetDefault());
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void OpenList()
        {
            try
            {
                ITransfersListView view;
                TransfersListPresenter presenter;

                view = new TransfersListView();
                presenter = new TransfersListPresenter(view, _transfers);

                view.ShowWindow();

                if (view.Transfer.isValid() && view.Transfer.idTraspaso.isValid())
                {
                    _view.Show(new VMTraspaso(view.Transfer));
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindItem()
        {
            if (!_view.CurrentItem.Articulo.codigo.isValid())
                return;
            
            //Si se esta procesando el pedido, no se debe buscar el articulo
            if (_view.IsBeingProcessed)
            {
                return;
            }

            try
            {
                var item = _items.Find(_view.CurrentItem.Articulo.codigo);

                if (!item.isValid())
                {
                    _view.ShowError("No existe ningún artículo con ese código");
                    _view.ClearItem();
                    return;
                }

                if (!item.activo)
                {
                    _view.ClearItem();
                    return;
                }

                //Existencia del articulo
                if (item.inventariado)
                {
                    //Se muestra la existencia
                    _view.ShowStock(_items.Stock(item.idArticulo));
                }
                else
                {
                    //Se resetea la existencia
                    _view.ShowStock(0m);
                }

                //Si lo encontré preparo un Detalle de Transferencia
                var detail = GetDetail(item);

                //Lo muestro
                _view.Show(detail);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenItemsList()
        {
            try
            {
                IItemsListView view;
                ItemsListPresenter presenter;

                view = new ItemsListView(true);
                presenter = new ItemsListPresenter(view, _items);

                view.ShowWindow();

                if (view.Item.idArticulo.isValid())
                {
                    //La lista de articulos ahora regresa una viewModel, se debe obtener el item correspondiente
                    var item = _items.Find(view.Item.idArticulo);

                    //Existencia del articulo
                    if (item.inventariado)
                    {
                        //Se muestra la existencia
                        _view.ShowStock(_items.Stock(item.idArticulo));
                    }
                    else
                    {
                        //Se resetea la existencia
                        _view.ShowStock(0m);
                    }

                    _view.Show(GetDetail(item));
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void AddItem()
        {
            if (!_view.CurrentItem.idArticulo.isValid())
            {
                _view.ClearItem();
                return;
            }

            if (!_view.CurrentItem.cantidadEnviada.isValid())
            {
                _view.ShowError("La cantidad enviada no puede ser cero");
                return;
            }

            try
            {
                //Me lo traigo para manipulación local
                var transfer = _view.Transfer;
                var detail = _view.CurrentItem;
                var item = _items.Find(detail.idArticulo);

                //Valido que la existencia actual del artículo sea mayor o igual a la cantidad a traspasar (solo inventariados)
                
                //Se debe tomar en cuenta si es la sucursal que envia o recibe el traspaso
                if (_view.IsBeingProcessed)
                {
                    //Se esta procesando el traspaso

                    //Se valida que la cantidad recibida sea menor o igual a la enviada
                    if (detail.cantidadAceptada > detail.cantidadEnviada)
                    {
                        _view.ShowError("No se puede recibir una cantidad mayor a la enviada");
                        return;
                    }
                }
                else
                {
                    //Se esta creando o modificando el traspaso
                    if (item.inventariado)
                    {
                        var currentStock = _items.Stock(_view.CurrentItem.idArticulo);
                        if (currentStock < detail.cantidadEnviada)
                        {
                            _view.ShowMessage("Existencia insuficiente");
                            return;
                        }
                    }
                    else
                    {
                        throw new Exception("No se pueden traspasar artículos sin inventariar");
                    }
                }

                //Si ya existe el artículo, sumo la cantidad a ese registro
                var exists = transfer.Detalle.FirstOrDefault(d => d.idArticulo.Equals(detail.idArticulo));
                if (exists.isValid())
                    exists.cantidadEnviada += detail.cantidadEnviada;
                else
                {
                    //Si no existe en el detalle actual, agrego el detalle a la lista
                    //Se asigna el item al detalle
                    detail.Articulo = item;
                    transfer.Detalle.Add(detail);
                }

                //Se calcula el total de la cuenta
                transfer.UpdateAccount();

                //Limpio el artículo en edición
                _view.ClearItem();

                //Muestra la nueva venta completa con la nueva lista de detalles y la cuenta
                _view.Show(transfer);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void RemoveItem()
        {
            if (_view.IsDirty && _view.Transfer.idEstatusDeTraspaso != (int)StatusDeTraspaso.Nuevo && _view.Transfer.idEstatusDeTraspaso != (int)StatusDeTraspaso.Registrado)
            {
                _view.ShowError("No se pueden editar traspasos procesados");
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
                var transfer = _view.Transfer;

                //Solo se elimina de la coleccion
                transfer.Detalle.Remove(item);

                //Recalculo
                transfer.UpdateAccount();

                //Muestro la venta sin el articulo
                _view.Show(transfer);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void SelectItem()
        {
            if (!_view.SelectedItem.isValid() || !_view.SelectedItem.idArticulo.isValid())
            {
                _view.ShowError("No existe ningún detalle seleccionado");
                return;
            }

            try
            {
                var item = _view.SelectedItem;

                //Lo elimino del detalle
                var transfer = _view.Transfer;
                transfer.Detalle.Remove(item);

                transfer.UpdateAccount();

                //Si se esta surtiendo el pedido, por defecto la cantidad es la misma que la cantidad enviada
                if (_view.IsBeingProcessed && !item.cantidadAceptada.HasValue)
                {
                    item.cantidadAceptada = item.cantidadEnviada;
                }

                //Muestro la venta sin el articulo
                _view.Show(transfer);

                //Muestro en edición el artículo seleccionado
                _view.Show(item);
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

        private void New()
        {
            try
            {
                //Establezco los defaults
                var transfer = GetDefault();

                //Limpia el detalle seleccionado por si hay alguno
                _view.ClearItem();

                _view.Show(transfer);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Print()
        {
            if (!_view.IsDirty)
            {
                _view.ShowMessage("No es posible imprimir un traspaso que no ha sido registrado");
                return;
            }

            ITransferPrintView view;
            TransferPrintPresenter presenter;

            view = new TransferPrintView(_view.Transfer);
            presenter = new TransferPrintPresenter(view, _transfers);

            view.ShowWindow();

            //Inicializo nuevamente
            Load();
        }

        private void Save()
        {
            string error;

            var transfer = _view.Transfer.ToTraspaso();
            if (!IsTransferValid(transfer, out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                //Le agrego quien la registra
                transfer.idUsuarioRegistro = Session.LoggedUser.idUsuario;
                transfer.Usuario = Session.LoggedUser;

                //Actualizo el folio
                transfer.folio = _transfers.Next();

                //Se obtiene el contexto para la base de datos remota
                try
                {
                    //Crea la cadena de conexion con los datos de la empresa destino
                    var remoteAssociatedCompany = _associatedCompanies.Find(transfer.EmpresasAsociada.idEmpresaAsociada);
                    var localAssociatedCompany = transfer.EmpresasAsociada1;

                    AproviContainer remoteContainer = new AproviContainer(remoteAssociatedCompany.GetConnectionString());

                    var container = remoteContainer.Container;

                    var remoteAssociatedCompanies = container.Resolve<IEmpresaAsociadaService>();
                    var remoteTransferRequests = container.Resolve<ISolicitudDeTraspasoService>();

                    //Se valida que la compañia origen se encuentre en la base de datos remota
                    if (!remoteAssociatedCompanies.CompanyExistsByDatabaseName(localAssociatedCompany))
                    {
                        _view.ShowError("La empresa asociada local no existe en las empresas asociadas destino");
                        return;
                    }

                    //Se valida la lista de codigos de articulo
                    var items = transfer.DetallesDeTraspasoes.Select(x => x.Articulo).ToList();
                    var missingItems = _items.GetMissingItems(items);

                    if (!missingItems.IsEmpty())
                    {
                        _view.ShowError(string.Format("Los artículos: {0} no existen en la empresa asociada destino", string.Join(",",missingItems.Select(x=>x.codigo).ToList())));
                        return;
                    }

                    //Se verifica si se hay articulos importados
                    if (items.Any(x=>x.importado))
                    {
                        ICustomsApplicationsExitView viewPedimentos = new CustomsApplicationsExitView(transfer.DetallesDeTraspasoes.GetOnlyImported());
                        CustomsApplicationsExitPresenter presenterPedimentos = new CustomsApplicationsExitPresenter(viewPedimentos, _customsApplications, _items);

                        viewPedimentos.ShowWindow();

                        //Debo asociar los pedimentos al detalle de mi remisión
                        //Itero entre cada uno de los artículos que necesitaba pedimentos
                        foreach (var p in viewPedimentos.Items)
                        {
                            //Obtengo el detalle correspondiente al artículo
                            var d = transfer.DetallesDeTraspasoes.FirstOrDefault(i => i.idArticulo.Equals(p.Articulo.idArticulo));
                            //Le asigno a ese detalle los pedimentos y cantidades correspondientes
                            p.Pedimentos.ForEach(pa => d.PedimentoPorDetalleDeTraspasoes.Add(new PedimentoPorDetalleDeTraspaso() { idPedimento = pa.IdPedimento, cantidad = pa.Cantidad, Pedimento = _customsApplications.Find(pa.IdPedimento) }));
                        }
                    }

                    //Se registra el traspaso localmente
                    var dbTransfer = _transfers.Add(transfer);
    
                    //Se registra la solicitud de traspaso
                    remoteTransferRequests.Add(dbTransfer);

                    //El contenedor ya no se utilizara
                    container.Dispose();
                    
                }
                catch (EntityException)
                {
                    _view.ShowError("Base de datos desactualizada");
                    return;
                }

                _view.ShowMessage("Traspaso registrado exitosamente");

                //Inicializo nuevamente
                Load();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        #region Private Utility Functions

        private bool IsTransferValid(Traspaso transfer, out string error)
        {

            if (!transfer.idEmpresaAsociadaOrigen.isValid())
            {
                error = "La empresa asociada origen no es válida";
                return false;
            }

            if (!transfer.idEmpresaAsociadaDestino.isValid())
            {
                error = "La empresa asociada destino no es válida";
                return false;
            }

            if (transfer.idEmpresaAsociadaOrigen == transfer.idEmpresaAsociadaDestino)
            {
                error = "La empresa asociada destino no puede ser igual a la empresa asociada origen";
                return false;
            }

            if (!transfer.folio.isValid())
            {
                error = "El folio no es válido";
                return false;
            }

            if (!transfer.descripcion.isValid())
            {
                error = "La descripcion no es válida";
                return false;
            }

            if (!transfer.tipoDeCambio.isValid())
            {
                error = "El tipo de cambio no es válido";
                return false;
            }

            if (transfer.DetallesDeTraspasoes.Count.Equals(0))
            {
                error = "Los conceptos no son válidos";
                return false;
            }

            error = string.Empty;
            return true;
        }

        private DetallesDeTraspaso GetDetail(Articulo item)
        {
            //Si lo encontré preparo un Detalle de Pedido
            var detail = new DetallesDeTraspaso();
            detail.Articulo = item;
            detail.idArticulo = item.idArticulo;
            detail.cantidadEnviada = 0.0m;

            detail.idMoneda = item.idMoneda;
            detail.costoUnitario = item.costoUnitario;

            return detail;
        }

        #endregion
    }
}
