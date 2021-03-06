using Aprovi.Business.ViewModels;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class AjusteService : IAjusteService
    {
        private IUnitOfWork _UOW;
        private IAjustesRepository _adjustments;
        private IArticulosRepository _items;
        private IUsuariosRepository _users;
        private IViewReporteEstatusDeLaEmpresaAjustesEntradaRepository _entranceAdjustments;
        private IViewReporteEstatusDeLaEmpresaAjustesSalidaRepository _exitAdjustments;

        public AjusteService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _adjustments = _UOW.Ajustes;
            _items = _UOW.Articulos;
            _users = _UOW.Usuarios;
            _entranceAdjustments = _UOW.EstatusDeLaEmpresaAjustesEntrada;
            _exitAdjustments = _UOW.EstatusDeLaEmpresaAjustesSalida;
        }

        public Ajuste Add(Ajuste adjustment)
        {
            try
            {
                // validaciones
                if (adjustment.DetallesDeAjustes.Count <= 0)
                    throw new Exception("No es posible agregar un ajuste sin artículos");

                if (!adjustment.descripcion.isValid())
                    throw new Exception("Debe capturar una descripción o motivo de ajuste");

                if (!adjustment.idTipoDeAjuste.isValid())
                    throw new Exception("Debe especificar el tipo de ajuste");

                //Debo asignarle el costo para cada articulo
                foreach (var item in adjustment.DetallesDeAjustes)
                {
                    var art = _items.Find(item.idArticulo);
                    item.costoUnitario = art.costoUnitario;
                    item.idMoneda = art.idMoneda;
                    item.Moneda = null;
                    item.Articulo = null;
                }

                //Me aseguro que tiene el ultimo folio antes de agregarlo
                adjustment.folio = Next();
                adjustment.Usuario = null;
                adjustment.TiposDeAjuste = null;
                adjustment.fechaHora = DateTime.Now;
                adjustment.idEstatusDeAjuste = (int)StatusDeAjuste.Registrado;

                _UOW.Reload();
                _adjustments.Add(adjustment);
                _UOW.Save();

                return adjustment;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Ajuste Find(int idAdjustment)
        {
            try
            {
                return _adjustments.Find(idAdjustment);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Ajuste Find(string folio)
        {
            try
            {
                return _adjustments.Find(folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Ajuste> List()
        {
            try
            {
                return _adjustments.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Ajuste> List(TiposDeAjuste type)
        {
            try
            {
                return _adjustments.List(type);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Ajuste> Like(string value)
        {
            try
            {
                return _adjustments.WithFolioLike(value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Next()
        {
            try
            {
                return _adjustments.Next();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Ajuste> List(DateTime start, DateTime end, TiposDeAjuste type)
        {
            try
            {
                return _adjustments.ListByPeriodAndType(start, end, type);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Ajuste MigrateStock(List<VMArticulo> items)
        {
            try
            {
                var initialStock = new Ajuste();
                initialStock.folio = _adjustments.Next();
                initialStock.idTipoDeAjuste = (int)TipoDeAjuste.Entrada;
                initialStock.idEstatusDeAjuste = (int)StatusDeAjuste.Registrado;
                initialStock.descripcion = "Inventario inicial por migración";
                initialStock.fechaHora = DateTime.Now;
                initialStock.idUsuarioRegistro = _users.Find("Aprovi").idUsuario;
                initialStock.DetallesDeAjustes = new List<DetallesDeAjuste>();
                foreach (var item in items)
                {
                    if (item.Existencia > 0.0m)
                        initialStock.DetallesDeAjustes.Add(new DetallesDeAjuste() { idArticulo = item.idArticulo, idMoneda = item.idMoneda, costoUnitario = item.costoUnitario, cantidad = item.Existencia });
                }

                _adjustments.Add(initialStock);

                _UOW.Save();

                return initialStock;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Ajuste GenerateExit(VMArticulo item)
        {
            try
            {
                var salida = new Ajuste();
                salida.folio = _adjustments.Next();
                salida.fechaHora = DateTime.Now;
                salida.descripcion = string.Format("Salida por inicio de uso de pedimentos");
                salida.idTipoDeAjuste = (int)TipoDeAjuste.Salida;
                salida.DetallesDeAjustes.Add(new DetallesDeAjuste()
                {
                    idArticulo = item.idArticulo,
                    cantidad = item.Existencia,
                    costoUnitario = item.costoUnitario,
                    idMoneda = item.idMoneda

                });

                return salida;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Ajuste GenerateEntrance(VMArticulo item, VMPedimento customApplication)
        {
            try
            {
                var entrada = new Ajuste();
                entrada.folio = _adjustments.Next();
                entrada.fechaHora = DateTime.Now;
                entrada.descripcion = string.Format("Entrada por inicio de uso de pedimentos");
                entrada.idTipoDeAjuste = (int)TipoDeAjuste.Entrada;
                entrada.DetallesDeAjustes.Add(new DetallesDeAjuste()
                {
                    idArticulo = item.idArticulo,
                    cantidad = customApplication.Unidades,
                    costoUnitario = item.costoUnitario,
                    idMoneda = item.idMoneda
                });
                entrada.Pedimentos.Add(new Pedimento()
                {
                    añoOperacion = customApplication.añoOperacion,
                    aduana = customApplication.aduana,
                    patente = customApplication.patente,
                    añoEnCurso = customApplication.añoEnCurso,
                    progresivo = customApplication.progresivo,
                    fecha= customApplication.fecha
                });

                return entrada;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VMEstadoDeLaEmpresa ListEntranceAdjustmentsForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate)
        {
            try
            {
                var detail = _entranceAdjustments.List(startDate, endDate);

                var detailPesos = detail.Where(x => x.idMoneda == (int) Monedas.Pesos).ToList();
                var detailDollars = detail.Where(x => x.idMoneda == (int) Monedas.Dólares).ToList();

                vm.TotalDolaresAjusteEntrada = detailDollars.Sum(x => x.importe.GetValueOrDefault(0m));
                vm.TotalPesosAjusteEntrada = detailPesos.Sum(x => x.importe.GetValueOrDefault(0m));

                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VMEstadoDeLaEmpresa ListExitAdjustmentsForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate)
        {
            try
            {
                var detail = _exitAdjustments.List(startDate, endDate);

                var detailPesos = detail.Where(x => x.idMoneda == (int) Monedas.Pesos).ToList();
                var detailDollars = detail.Where(x => x.idMoneda == (int) Monedas.Dólares).ToList();

                vm.TotalDolaresAjusteSalida = detailDollars.Sum(x => x.importe.GetValueOrDefault(0m));
                vm.TotalPesosAjusteSalida = detailPesos.Sum(x => x.importe.GetValueOrDefault(0m));

                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
