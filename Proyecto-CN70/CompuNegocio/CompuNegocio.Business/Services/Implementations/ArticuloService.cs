using Aprovi.Business.ViewModels;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class ArticuloService : IArticuloService
    {
        private IUnitOfWork _UOW;
        private IArticulosRepository _items;
        private IViewExistenciasRepository _stock;
        private IViewEntradasPorAjustesRepository _adjustmentsIn;
        private IViewSalidasPorAjustesCanceladosRepository _cancelledAdjustmentsOut;
        private IViewEntradasPorComprasRepository _purchasesIn;
        private IViewSalidasPorComprasCanceladasRepository _cancelledPurchasesOut;
        private IViewSalidasPorAjustesRepository _adjustmentsOut;
        private IViewEntradasPorAjustesCanceladosRepository _cancelledAdjustmentsIn;
        private IViewSalidasPorFacturasRepository _invoicesOut;
        private IViewEntradasPorFacturasCanceladasRepository _cancelledInvoicesIn;
        private IViewSalidasPorRemisionesRepository _billsOfSaleOut;
        private IViewEntradasPorRemisionesCanceladasRepository _cancelledBillsOfSaleIn;
        private IUnidadesDeMedidaRepository _unitsOfMeasure;
        private IImpuestosRepository _taxes;
        private IVFPDataExtractorRepository _vfpData;
        private IClasificacionesRepository _classifications;
        private IViewReporteInventarioFisicoRepository _inventarioFisico;
        private IViewArticulosVendidosRepository _articulosVendidos;
        private IViewEntradasPorTraspasosRepository _transfersIn;
        private IViewSalidasPorTraspasosRepository _transfersOut;
        private IViewCodigosDeArticuloPorProveedorRepository _itemsCodes;
        private IViewEntradasPorNotasDeCreditoRepository _creditNotesIn;
        private IViewSalidasPorNotasDeCreditoCanceladasRepository _creditNotesOut;
        private IViewReporteAvaluoRepository _appraisals;
        private IViewReporteEstatusDeLaEmpresaAvaluoRepository _companyAppraisal;

        public ArticuloService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _items = _UOW.Articulos;
            _stock = _UOW.Existencias;
            _adjustmentsIn = _UOW.EntradasPorAjustes;
            _cancelledAdjustmentsOut = _UOW.SalidasPorAjustesCancelados;
            _purchasesIn = _UOW.EntradasPorCompras;
            _cancelledPurchasesOut = _UOW.SalidasPorComprasCanceladas;
            _adjustmentsOut = _UOW.SalidasPorAjustes;
            _cancelledAdjustmentsIn = _UOW.EntradasPorAjustesCancelados;
            _invoicesOut = _UOW.SalidasPorFacturas;
            _cancelledInvoicesIn = _UOW.EntradasPorFacturasCanceladas;
            _billsOfSaleOut = _UOW.SalidasPorRemisiones;
            _cancelledBillsOfSaleIn = _UOW.EntradasPorRemisionesCanceladas;
            _unitsOfMeasure = _UOW.UnidadesDeMedida;
            _taxes = _UOW.Impuestos;
            _vfpData = _UOW.VFPDataExtractor;
            _classifications = _UOW.Clasificaciones;
            _inventarioFisico = _UOW.InventarioFisico;
            _articulosVendidos = _UOW.ArticulosVendidos;
            _transfersIn = _UOW.EntradasPorTraspasos;
            _transfersOut = _UOW.SalidasPorTraspasos;
            _itemsCodes = _UOW.CodigosDeArticulosPorProveedor;
            _creditNotesIn = _UOW.EntrasPorNotasDeCredito;
            _creditNotesOut = _UOW.SalidasPorNotasDeCredito;
            _appraisals = _UOW.Avaluo;
            _companyAppraisal = _UOW.EstatusDeLaEmpresaAvaluo;
        }

        public Articulo Add(Articulo item)
        {
            try
            {
                _UOW.Reload();
                item.ProductosServicio = null;
                _items.Add(item);
                _UOW.Save();
                return item;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Articulo> List()
        {
            try
            {
                return _items.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Articulo> List(Clasificacione classification)
        {
            try
            {
                return _items.List(classification.idClasificacion);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Articulo> WithNameLike(string name)
        {
            try
            {
                return _items.WithNameLike(name);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Articulo Find(int idItem)
        {
            try
            {
                return _items.Find(idItem);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Articulo Find(string code)
        {
            try
            {
                return _items.Find(code);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Articulo Update(Articulo item)
        {
            try
            {
                _UOW.Reload();
                var local = _items.Find(item.idArticulo);
                local.activo = item.activo;
                local.codigo = item.codigo;
                local.costoUnitario = item.costoUnitario;
                local.descripcion = item.descripcion;
                local.Impuestos = item.Impuestos;
                local.idProductoServicio = item.idProductoServicio;
                local.Precios = item.Precios;
                local.idTipoDeComision = item.idTipoDeComision;
                local.idTipoDeComision = item.idTipoDeComision;
                local.idMoneda = item.idMoneda;
                local.idUnidadDeMedida = item.idUnidadDeMedida;
                local.CodigosDeArticuloPorProveedors = item.CodigosDeArticuloPorProveedors;
                //Solo puede activarse si esta desactivado, pero no puede desactivarse.
                if(!local.importado)
                    local.importado = item.importado;

                _items.Update(local);
                _UOW.Save();

                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(Articulo item)
        {
            try
            {
                _items.Remove(item.idArticulo);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CanDelete(Articulo item)
        {
            try
            {
                return _items.CanDelete(item.idArticulo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public decimal Stock(int idItem)
        {
            try
            {
                var item = _items.Find(idItem);

                if (!item.inventariado)
                    return 0.0m;

                return _stock.Find(idItem).existencia.GetValueOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Articulo> GetMissingItems(List<Articulo> items)
        {
            try
            {
                List<Articulo> itemsThatDoesntExist = new List<Articulo>();

                foreach (var item in items)
                {
                    var localItem = Find(item.codigo);

                    if (!localItem.isValid())
                    {
                        itemsThatDoesntExist.Add(item);
                    }
                }

                return itemsThatDoesntExist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMInventario> Stock(List<Clasificacione> classifications)
        {
            try
            {
                var inventory = new List<VMInventario>();
                var classificationsIds = classifications.Select(x => x.idClasificacion).ToArray();
                var items = _inventarioFisico.WithClassifications(classificationsIds).ToVMInventario();

                foreach (var item in items)
                {
                    inventory.Add(item);
                }

                return inventory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMFlujoPorArticulo> StockFlow(DateTime start, DateTime end)
        {
            try
            {
                var flow = new List<VMFlujoPorArticulo>();
                start = start.ToLastMidnight();
                end = end.ToNextMidnight();

                //Primero busco todos los artículos que han tenido movimiento en el periodo y que son inventariados
                var items = _items.WithFlow(start, end);

                foreach (var item in items.Where(i => i.inventariado))
                {

                    var i = new VMFlujoPorArticulo(item);

                    i.EntradasAjuste = _adjustmentsIn.GetTotal(item.idArticulo, start, end) - _cancelledAdjustmentsOut.GetTotal(item.idArticulo, start, end);
                    i.EntradasCompras = _purchasesIn.GetTotal(item.idArticulo, start, end) - _cancelledPurchasesOut.GetTotal(item.idArticulo, start, end);
                    i.EntradasTotales = i.EntradasAjuste + i.EntradasCompras;

                    i.SalidasAjustes = _adjustmentsOut.GetTotal(item.idArticulo, start, end) - _cancelledAdjustmentsIn.GetTotal(item.idArticulo, start, end);
                    i.SalidasFacturas = _invoicesOut.GetTotal(item.idArticulo, start, end) - _cancelledInvoicesIn.GetTotal(item.idArticulo, start, end);
                    i.SalidasRemisiones = _billsOfSaleOut.GetTotal(item.idArticulo, start, end) - _cancelledBillsOfSaleIn.GetTotal(item.idArticulo, start, end);
                    i.SalidasTotales = i.SalidasAjustes + i.SalidasFacturas + i.SalidasVentas + i.SalidasRemisiones;

                    i.Existencia = _stock.Find(item.idArticulo).existencia.GetValueOrDefault();

                    //WriteToFile(i);
                    flow.Add(i);
                }

                return flow;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMFlujoPorArticulo> StockFlow(DateTime start, DateTime end, Clasificacione classification)
        {
            try
            {
                var flow = new List<VMFlujoPorArticulo>();
                var items = _items.List(classification.idClasificacion);
                start = start.ToLastMidnight();
                end = end.ToNextMidnight();
                foreach (var item in items)
                {
                    //Cuando no lleva control de inventario no es necesario siquiera que aparezca aqui
                    if (!item.inventariado)
                        continue;

                    var i = new VMFlujoPorArticulo(item);

                    i.EntradasAjuste = _adjustmentsIn.GetTotal(item.idArticulo, start, end) - _cancelledAdjustmentsOut.GetTotal(item.idArticulo, start, end);
                    i.EntradasCompras = _purchasesIn.GetTotal(item.idArticulo, start, end) - _cancelledPurchasesOut.GetTotal(item.idArticulo, start, end);
                    i.EntradasTotales = i.EntradasAjuste + i.EntradasCompras;

                    i.SalidasAjustes = _adjustmentsOut.GetTotal(item.idArticulo, start, end) - _cancelledAdjustmentsIn.GetTotal(item.idArticulo, start, end);
                    i.SalidasFacturas = _invoicesOut.GetTotal(item.idArticulo, start, end) - _cancelledInvoicesIn.GetTotal(item.idArticulo, start, end);
                    i.SalidasRemisiones = _billsOfSaleOut.GetTotal(item.idArticulo, start, end) - _cancelledBillsOfSaleIn.GetTotal(item.idArticulo, start, end);
                    i.SalidasTotales = i.SalidasAjustes + i.SalidasFacturas + i.SalidasVentas + i.SalidasRemisiones;

                    i.Existencia = _stock.Find(item.idArticulo).existencia.GetValueOrDefault();

                    flow.Add(i);
                }

                return flow;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMDetalleKardex> StockFlow(Articulo item, DateTime start, DateTime end)
        {
            try
            {
                var kardex = new List<VMDetalleKardex>();

                //Cuando no lleva control de inventario no es necesario llenar nada
                if (!item.inventariado)
                    return kardex;

                start = start.ToLastMidnight();
                end = end.ToNextMidnight();

                //Obtengo todos los movimientos
                var entradasPorCompras = _purchasesIn.List(item.idArticulo, start, end);
                var salidasPorComprasCanceladas = _cancelledPurchasesOut.List(item.idArticulo, start, end);
                var entradasPorAjustes = _adjustmentsIn.List(item.idArticulo, start, end);
                var salidasPorAjustesCancelados = _cancelledAdjustmentsOut.List(item.idArticulo, start, end);
                var salidasPorAjustes = _adjustmentsOut.List(item.idArticulo, start, end);
                var entradasPorAjustesCancelados = _cancelledAdjustmentsIn.List(item.idArticulo, start, end);
                var salidasPorFacturas = _invoicesOut.List(item.idArticulo, start, end);
                var entradasPorFacturasCanceladas = _cancelledInvoicesIn.List(item.idArticulo, start, end);
                var salidasPorRemisiones = _billsOfSaleOut.List(item.idArticulo, start, end);
                var entradasPorRemisionesCanceladas = _cancelledBillsOfSaleIn.List(item.idArticulo, start, end);
                var salidasPorTraspasos = _transfersOut.List(item.idArticulo, start, end);
                var entradasPorTraspasos = _transfersIn.List(item.idArticulo, start, end);
                var entradasPorNotasDeCredito = _creditNotesIn.List(item.idArticulo, start, end);
                var salidasPorNotasDeCredito = _creditNotesOut.List(item.idArticulo, start, end);

                //Convierto todos los movimientos a detalles del kardex
                entradasPorCompras.ForEach(e => kardex.Add(new VMDetalleKardex(e)));
                salidasPorComprasCanceladas.ForEach(s => kardex.Add(new VMDetalleKardex(s)));
                entradasPorAjustes.ForEach(e => kardex.Add(new VMDetalleKardex(e)));
                salidasPorAjustesCancelados.ForEach(s => kardex.Add(new VMDetalleKardex(s)));
                salidasPorAjustes.ForEach(s => kardex.Add(new VMDetalleKardex(s)));
                entradasPorAjustesCancelados.ForEach(e => kardex.Add(new VMDetalleKardex(e)));
                salidasPorFacturas.ForEach(s => kardex.Add(new VMDetalleKardex(s)));
                entradasPorFacturasCanceladas.ForEach(e => kardex.Add(new VMDetalleKardex(e)));
                salidasPorRemisiones.ForEach(s => kardex.Add(new VMDetalleKardex(s)));
                entradasPorRemisionesCanceladas.ForEach(e => kardex.Add(new VMDetalleKardex(e)));
                salidasPorTraspasos.ForEach(s => kardex.Add(new VMDetalleKardex(s)));
                entradasPorTraspasos.ForEach(e => kardex.Add(new VMDetalleKardex(e)));
                entradasPorNotasDeCredito.ForEach(e => kardex.Add(new VMDetalleKardex(e)));
                salidasPorNotasDeCredito.ForEach(s => kardex.Add(new VMDetalleKardex(s)));


                //Ordeno los movimientos por la fecha
                kardex.Sort((VMDetalleKardex d1, VMDetalleKardex d2) => DateTime.Compare(d1.fechaHora, d2.fechaHora));

                return kardex;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void WriteToFile(VMFlujoPorArticulo flujo)
        {
            try
            {
                //Realizo una copia del template
                // var templateCopy = File.Copy("Original","MyNewReport")

                //cargo en excel la nueva copia del template
                // excel = new FileInfo("MyNewReport");

                var excel = new FileInfo(@"D:\Reports\Flujo.xlsx");
                //Creo el archivo en memoria
                using (ExcelPackage file = new ExcelPackage(excel))
                {
                    //Se obtiene la hoja de excel
                    ExcelWorksheet sheet = file.Workbook.Worksheets[1];

                    //La celda inicial del reporte es A2, pero si estoy rellenando un reporte debo insertar en el renglon de abajo
                    int r = 1;
                    var hasData = true;
                    //reviso si A2 está vacío
                    while (hasData)
                    {
                        r++;
                        //Reviso la evaluacion del siguiente renglón para ver si llegué al final
                        hasData = sheet.Cells[string.Format("A{0}", r)].Value.isValid();
                    }

                    //Ya tengo el renglon en el que debo registrar la info
                    sheet.Cells[string.Format("A{0}", r)].Value = flujo.Codigo;
                    sheet.Cells[string.Format("B{0}", r)].Value = flujo.Descripcion;
                    sheet.Cells[string.Format("C{0}", r)].Value = flujo.EntradasAjuste;
                    sheet.Cells[string.Format("D{0}", r)].Value = flujo.EntradasCompras;
                    sheet.Cells[string.Format("E{0}", r)].Value = flujo.EntradasTotales;
                    sheet.Cells[string.Format("F{0}", r)].Value = flujo.SalidasAjustes;
                    sheet.Cells[string.Format("G{0}", r)].Value = flujo.SalidasFacturas;
                    sheet.Cells[string.Format("H{0}", r)].Value = flujo.SalidasRemisiones;
                    sheet.Cells[string.Format("I{0}", r)].Value = flujo.SalidasTotales;
                    sheet.Cells[string.Format("J{0}", r)].Value = flujo.Existencia;

                    file.Save();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VMFlujoPorArticulo StockFlow(DateTime start, DateTime end, Articulo item)
        {
            try
            {
                var itemFlow = new VMFlujoPorArticulo(item);
                //Cuando no lleva control de inventario no es necesario llenar nada
                if (!item.inventariado)
                    return itemFlow;

                start = start.ToLastMidnight();
                end = end.ToNextMidnight();

                itemFlow.EntradasAjuste = _adjustmentsIn.GetTotal(item.idArticulo, start, end) - _cancelledAdjustmentsOut.GetTotal(item.idArticulo, start, end);
                itemFlow.EntradasCompras = _purchasesIn.GetTotal(item.idArticulo, start, end) - _cancelledPurchasesOut.GetTotal(item.idArticulo, start, end);
                itemFlow.EntradasTotales = itemFlow.EntradasAjuste + itemFlow.EntradasCompras;

                itemFlow.SalidasAjustes = _adjustmentsOut.GetTotal(item.idArticulo, start, end) - _cancelledAdjustmentsIn.GetTotal(item.idArticulo, start, end);
                itemFlow.SalidasFacturas = _invoicesOut.GetTotal(item.idArticulo, start, end) - _cancelledInvoicesIn.GetTotal(item.idArticulo, start, end);
                itemFlow.SalidasRemisiones = _billsOfSaleOut.GetTotal(item.idArticulo, start, end) - _cancelledBillsOfSaleIn.GetTotal(item.idArticulo, start, end);
                itemFlow.SalidasTotales = itemFlow.SalidasAjustes + itemFlow.SalidasFacturas + itemFlow.SalidasVentas + itemFlow.SalidasRemisiones;

                itemFlow.Existencia = _stock.Find(item.idArticulo).existencia.GetValueOrDefault();

                return itemFlow;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Articulo> Import(List<Articulo> items, List<VMEquivalenciaUnidades> units, List<VMEquivalenciaClasificacion> classifications, Impuesto vat)
        {
            try
            {

                var integrated = new List<Articulo>();

                //Por cada artículos en el catálogo voy a procesarlo para agregarlo a la base de datos
                foreach (var item in items)
                {
                    var newItem = new Articulo();
                    //Si ya existe me lo salto
                    newItem = _items.SearchAll(item.codigo);
                    if (newItem.isValid())
                        continue;

                    //Si llegó aqui es nuevo
                    newItem = new Articulo();
                    newItem.activo = true;
                    newItem.codigo = item.codigo;
                    newItem.idMoneda = item.idMoneda;
                    newItem.costoUnitario = item.costoUnitario;
                    newItem.descripcion = item.descripcion;
                    newItem.idProductoServicio = (int)ProductosServicios.No_Existe_En_El_Catalogo;
                    newItem.Precios = item.Precios;

                    //Busco la unidad de medida equivalente respecto a la descripción de la que trae
                    var equivalencia = units.FirstOrDefault(u => u.Descripcion.Equals(item.UnidadesDeMedida.descripcion.Trim()));
                    newItem.idUnidadDeMedida = equivalencia.IdUnidadDeMedida;

                    //Los artículos que en el sistema anterior tenian como unidad de medida "SERVICIO" no van inventariados
                    newItem.inventariado = !equivalencia.Descripcion.Equals("SERVICIO");

                    //Si tiene impuesto le asigno el que se especifico
                    if (item.Impuestos.Count > 0)
                    {
                        newItem.Impuestos = new List<Impuesto>();
                        newItem.Impuestos.Add(vat);
                    }

                    //Si tiene clasificaciones le asigno las correspondientes según la equivalencia
                    foreach (var c in item.Clasificaciones)
                    {
                        //Obtengo la equivalencia
                        var equivalenciaClasificacion = classifications.FirstOrDefault(cl => cl.Clave.Substring(2).Equals(c.descripcion.Trim()));
                        //Como la equivalencia de clasificación no es obligatoria, voy a validar que no sea null
                        if (!equivalenciaClasificacion.isValid())
                            continue;
                        //De la equivalencia obtengo la clasificación
                        var classification = _classifications.Find(equivalenciaClasificacion.IdClasificacion);
                        newItem.Clasificaciones.Add(classification);
                    }

                    //Agrego el nuevo artículo
                    integrated.Add(_items.Add(newItem));
                }

                //Una vez que he procesado todos, entonces hago persistentes los cambios
                _UOW.Save();

                return integrated;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UnidadesDeMedida> GetItemUnits(Articulo item)
        {
            try
            {
                if (item.Equivalencias.IsEmpty())
                {
                    //Regresa la unidad del articulo
                    return item.Equivalencias.Select(x => x.UnidadesDeMedida).Distinct<UnidadesDeMedida>().ToList();
                }
                else
                {
                    //Regresa las unidades del articulo
                    return new List<UnidadesDeMedida>(){ item.UnidadesDeMedida };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwArticulosVendido> GetSoldItems(Cliente customer, string filter)
        {
            try
            {
                if (filter.isValid())
                {
                    return _articulosVendidos.Like(customer.idCliente, filter);
                }
                else
                {
                    return _articulosVendidos.List(customer.idCliente);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public bool HasTransactions(Articulo item)
        {
            try
            {
                var articuloDb = _items.Find(item.idArticulo);

                if (!articuloDb.isValid())
                {
                    //Un articulo que no existe no puede tener transacciones relacionadas
                    return false;
                }

                if (articuloDb.DetallesDeCotizacions.Count > 0)
                {
                    return true;
                }

                if (articuloDb.DetallesDeOrdenDeCompras.Count > 0)
                {
                    return true;
                }

                if (articuloDb.DetallesDePedidoes.Count > 0)
                {
                    return true;
                }

                if (articuloDb.DetallesDeRemisions.Count > 0)
                {
                    return true;
                }

                if (articuloDb.DetallesDeCompras.Count > 0)
                {
                    return true;
                }

                if (articuloDb.DetallesDeFacturas.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMArticulo> GetItemsForList()
        {
            try
            {
                return _items.GetItemsForList().Select(x => new VMArticulo(x)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMArticulo> GetItemsForPagedList(int itemsAmount, int pageNumber)
        {
            try
            {
                return _items.GetItemsForPagedList(itemsAmount, pageNumber).Select(x => new VMArticulo(x)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMArticulo> GetItemsForListWithNameLike(string name)
        {
            try
            {
                return _items.GetItemsForListWithNameLike(name).Select(x => new VMArticulo(x)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool HasCustomsApplicationActive(Articulo item)
        {
            try
            {
                return _items.Find(item.idArticulo).importado;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwCodigosDeArticuloPorProveedor> List(int idArticulo)
        {
            try
            {
                return _itemsCodes.List(idArticulo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VMRAvaluo GetAppraisal(FiltroReporteAvaluo filter, bool onlyWithStock, decimal exchangeRate, DateTime endDate, Clasificacione classification)
        {
            try
            {
                List<VMRDetalleAvaluo> detail = new List<VMRDetalleAvaluo>();
                var d = _appraisals.List(filter, endDate, classification);

                foreach (var item in d.GroupBy(x=>x.idArticulo))
                {
                    VwReporteAvaluo i = item.FirstOrDefault();
                    VMRDetalleAvaluo detalle = new VMRDetalleAvaluo();

                    detalle.Clasificacion = classification.descripcion;
                    detalle.idClasificacion = classification.idClasificacion;
                    detalle.CodigoArticulo = i.codigoArticulo;
                    detalle.DescripcionArticulo = i.descripcionArticulo;
                    detalle.idArticulo = i.idArticulo;
                    detalle.Precio = i.precioUnitario.GetValueOrDefault(0).ToDocumentCurrency(new Moneda(){idMoneda = i.idMoneda},new Moneda() { idMoneda = (int)Monedas.Pesos}, exchangeRate);
                    detalle.Existencia = (decimal)(item.Sum(x => x.entradas) - item.Sum(x => x.salidas.GetValueOrDefault(0)));
                    detalle.Importe = detalle.Existencia * detalle.Precio;

                    detail.Add(detalle);
                }
                //Antes de calcular el porcentaje, se filtran los que no tienen existencia, de aplicar
                if (onlyWithStock)
                {
                    detail = detail.Where(x => x.Existencia > 0).ToList();
                }

                //Ahora se calcula el porcentaje
                //Se obtiene el inventario total
                decimal total = detail.Sum(x => x.Importe);

                foreach (var i in detail)
                {
                    i.PorcentajeTotal = i.Importe / (total / 100);
                }

                VMRAvaluo report = new VMRAvaluo()
                {
                    Clasificacion = classification,
                    Fecha = endDate,
                    Filtro = filter,
                    SoloExistencias = onlyWithStock,
                    TipoDeCambio = exchangeRate,
                    Detalle = detail,
                    Total = total,
                    PorcentajeTotal = 100
                };

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VMEstadoDeLaEmpresa ListItemsForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate)
        {
            try
            {
                var detail = _companyAppraisal.List(startDate, endDate);

                var detailPesos = detail.Where(x => x.idMoneda == (int) Monedas.Pesos).ToList();
                var detailDollars = detail.Where(x => x.idMoneda == (int) Monedas.Dólares).ToList();

                vm.TotalDolaresAvaluo = detailDollars.Sum(x => x.importe.GetValueOrDefault(0m));
                vm.TotalPesosAvaluo = detailPesos.Sum(x => x.importe.GetValueOrDefault(0m));

                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Articulo> FindAllForSupplier(string code, int idSupplier)
        {
            try
            {
                return _items.FindAllForProvider(code, idSupplier);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Articulo> FindAllForCustomer(string code, int idCustomer)
        {
            try
            {
                return _items.FindAllForCustomer(code, idCustomer);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
