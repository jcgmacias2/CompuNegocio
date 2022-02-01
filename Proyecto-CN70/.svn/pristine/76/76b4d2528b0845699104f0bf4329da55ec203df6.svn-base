using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Helpers;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Presenters
{
    public class AdjustmentsPresenter
    {
        private IAdjustmentsView _view;
        private IAjusteService _adjustments;
        private ICatalogosEstaticosService _catalogs;
        private IArticuloService _items;
        private IPedimentoService _customsApplications;

        public AdjustmentsPresenter(IAdjustmentsView view, IAjusteService adjustments, ICatalogosEstaticosService catalogs, IArticuloService items, IPedimentoService customsApplications)
        {
            _view = view;
            _adjustments = adjustments;
            _catalogs = catalogs;
            _items = items;
            _customsApplications = customsApplications;

            _view.Find += Find;
            _view.OpenList += OpenList;
            _view.FindItem += FindItem;
            _view.OpenItemsList += OpenItemsList;
            _view.AddItem += AddItem;
            _view.RemoveItem += RemoveItem;
            _view.SelectItem += SelectItem;
            _view.New += New;
            _view.Quit += Quit;
            _view.Print += Print;
            _view.Save += Save;

            //Lleno el combo e inicializo
            _view.FillCombo(_catalogs.ListTiposDeAjuste());
            New();
        }

        private void Save()
        {
            try
            {
                var adjustment = _view.Adjustment;

                //Si el ajuste es de entrada y tiene artículos con pedimentos, debe especificar el pedimento
                if(adjustment.idTipoDeAjuste.Equals((int)TipoDeAjuste.Entrada) && adjustment.DetallesDeAjustes.Any(i => i.Articulo.importado))
                {
                    ICustomsApplicationView view;
                    CustomsApplicationPresenter presenter;

                    view = new CustomsApplicationView();
                    presenter = new CustomsApplicationPresenter(view);

                    view.ShowWindow();

                    adjustment.Pedimentos = new List<Pedimento>();
                    adjustment.Pedimentos.Add(view.CustomsApplication);
                }

                //Si el ajuste es de salida y tiene artículos con pedimentos, debe especificar los pedimentos asociados
                //Pedimentos
                if (adjustment.idTipoDeAjuste.Equals((int)TipoDeAjuste.Salida) && adjustment.DetallesDeAjustes.Any(a => a.Articulo.importado))
                {
                    ICustomsApplicationsExitView view = new CustomsApplicationsExitView(adjustment.DetallesDeAjustes.GetOnlyImported());
                    CustomsApplicationsExitPresenter presenter = new CustomsApplicationsExitPresenter(view, _customsApplications, _items);

                    view.ShowWindow();

                    //Debo asociar los pedimentos al detalle de mi remisión
                    //Itero entre cada uno de los artículos que necesitaba pedimentos
                    foreach (var p in view.Items)
                    {
                        //Obtengo el detalle correspondiente al artículo
                        var d = adjustment.DetallesDeAjustes.FirstOrDefault(i => i.idArticulo.Equals(p.Articulo.idArticulo));
                        //Le asigno a ese detalle los pedimentos y cantidades correspondientes
                        p.Pedimentos.ForEach(pa => d.PedimentoPorDetalleDeAjustes.Add(new PedimentoPorDetalleDeAjuste() { idPedimento = pa.IdPedimento, cantidad = pa.Cantidad }));
                    }
                }

                //Las validaciones mínimas ya estan en el service
                adjustment.idUsuarioRegistro = Session.LoggedUser.idUsuario;
                adjustment = _adjustments.Add(adjustment);

                _view.ShowMessage("Ajuste {0} registrado exitosamente", adjustment.folio);

                New();
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
                _view.ShowMessage("No es posible imprimir un ajuste que no ha sido registrada");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var adjustment = _adjustments.Find(_view.Adjustment.idAjuste);

                view = new ReportViewerView(Reports.FillReport(adjustment));
                presenter = new ReportViewerPresenter(view);

                view.Preview();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
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
                var adjustment = new Ajuste();
                adjustment.fechaHora = DateTime.Now;
                adjustment.folio = _adjustments.Next();
                _view.Show(adjustment);
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
                _view.ShowError("No se pueden editar los ajustes registradas");
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
                var adjustment = _view.Adjustment;
                adjustment.DetallesDeAjustes.Remove(item);

                //Muestro el ajuste sin el articulo
                _view.Show(adjustment);
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
                _view.ShowError("No se pueden editar los ajustes registradas");
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
                var adjustment = _view.Adjustment;
                adjustment.DetallesDeAjustes.Remove(item);
                //Muestro la venta sin el articulo
                _view.Show(adjustment);

                //Muestro en edición el artículo seleccionado
                _view.Show(item);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void AddItem()
        {
            if (!_view.CurrentItem.Articulo.idArticulo.isValid())
            {
                _view.ClearItem();
                return;
            }

            if (!_view.CurrentItem.Articulo.inventariado)
            {
                _view.ShowError("Los artículos no inventariados no pueden incluirse dentro de un ajuste de inventario");
                _view.ClearItem();
                return;
            }

            if (!_view.CurrentItem.cantidad.isValid())
            {
                _view.ShowError("La cantidad no puede ser cero");
                return;
            }

            if(!_view.Adjustment.idTipoDeAjuste.isValid())
            {
                _view.ShowError("Debe seleccionar el tipo de ajuste antes de agregar artículos");
                return;
            }

            try
            {
                //Me lo traigo para manipulación local
                var detail = _view.CurrentItem;

                //Si el ajuste es de salida valido que la existencia actual del artículo sea mayor o igual a la cantidad a sacar
                //Aplica para artículos inventariados y con modulo de control de ivnentario
                if (_view.Adjustment.idTipoDeAjuste.Equals((int)TipoDeAjuste.Salida) && Modulos.Control_de_Inventario.IsActive())
                {
                    var currentStock = _items.Stock(_view.CurrentItem.idArticulo);
                    if (currentStock < detail.cantidad)
                    {
                        _view.ShowMessage("Existencia insuficiente");
                        return;
                    }
                }

                //Si ya existe el artículo con el mismo precio unitario, sumo la cantidad a ese registro
                var adjustment = _view.Adjustment;
                var exists = adjustment.DetallesDeAjustes.FirstOrDefault(d => d.idArticulo.Equals(detail.idArticulo));
                if (exists.isValid())
                    exists.cantidad += detail.cantidad;
                else //Si no existe en el detalle actual, agrego el detalle a la lista
                    adjustment.DetallesDeAjustes.Add(detail);

                //Limpio el artículo en edición
                _view.ClearItem();

                //Muestra el nuevo ajuste completo con la nueva lista de detalles
                _view.Show(adjustment);
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
                _view.ShowError("No se pueden editar los ajustes registrados");
                return;
            }

            try
            {
                IItemsListView view;
                ItemsListPresenter presenter;

                view = new ItemsListView(true);
                presenter = new ItemsListPresenter(view, _items);

                view.ShowWindow();

                if (!view.Item.idArticulo.isValid())
                    return;

                //La lista de articulos ahora regresa una viewModel, se debe obtener el item correspondiente
                var item = _items.Find(view.Item.idArticulo);

                if (!item.inventariado)
                    throw new Exception("No es posible agregar artículos no inventariados en un ajuste");

                _view.Show(new DetallesDeAjuste() { idArticulo = view.Item.idArticulo, Articulo = item });

                //Se muestra el stock
                _view.ShowStock(_items.Stock(item.idArticulo));
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

            try
            {
                var item = _items.Find(_view.CurrentItem.Articulo.codigo);

                if (!item.isValid())
                {
                    _view.ShowError("No existe ningún artículo con ese código");
                    _view.ClearItem();
                    return;
                }

                if (!item.inventariado)
                    throw new Exception("No es posible agregar artículos no inventariados en un ajuste");

                //Si lo encontré muestro el artículo en un detalle
                _view.Show(new DetallesDeAjuste() { idArticulo = item.idArticulo, Articulo = item });

                //Se muestra el stock
                _view.ShowStock(_items.Stock(item.idArticulo));
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
                IAdjustmentsListView view;
                AdjustmentsListPresenter presenter;

                view = new AdjustmentsListView();
                presenter = new AdjustmentsListPresenter(view, _adjustments);

                view.ShowWindow();

                if (view.Adjustment.idAjuste.isValid())
                    _view.Show(view.Adjustment);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            if (!_view.Adjustment.folio.isValid())
                return;

            try
            {
                var adjustment = _adjustments.Find(_view.Adjustment.folio);

                if (adjustment.isValid())
                    _view.Show(adjustment);
                else
                    _view.Show(new Ajuste() { folio = _view.Adjustment.folio, fechaHora = DateTime.Now });
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

    }
}
