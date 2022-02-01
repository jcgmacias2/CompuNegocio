using Aprovi.Business.ViewModels;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Aprovi.Business.Helpers;

namespace Aprovi.Business.Services
{
    public abstract class NotaDeDescuentoService : INotaDeDescuentoService
    {
        private IUnitOfWork _UOW;
        private INotasDeDescuentoRepository _discountNotes;
        private IConfiguracionService _config;
        private IClienteService _clients;
        private IArticuloService _items;
        private IViewReporteEstatusDeLaEmpresaNotasDeDescuentoRepository _companyStatusDiscountNotes;
        private IViewReporteNotasDeDescuentoRepository _discountNotesReport;

        public NotaDeDescuentoService(IUnitOfWork unitOfWork, IConfiguracionService config, IClienteService clients, IArticuloService items)
        {
            _UOW = unitOfWork;
            _config = config;
            _clients = clients;
            _items = items;
            _discountNotes = _UOW.NotasDeDescuento;
            _discountNotesReport = _UOW.ReporteNotasDeDescuento;
            _companyStatusDiscountNotes = _UOW.EstatusDeLaEmpresaNotasDeDescuento;
        }

        public int Next()
        {
            try
            {
                return _discountNotes.Next();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Last()
        {
            try
            {
                return _discountNotes.Last();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotasDeDescuento Update(NotasDeDescuento discountNote)
        {
            try
            {
                var dbDiscountNote = _discountNotes.Find(discountNote.idNotaDeDescuento);

                if (dbDiscountNote.idEstatusDeNotaDeDescuento != (int) StatusDeNotaDeDescuento.Registrada)
                {
                    throw new Exception("No se puede modificar una nota de descuento cancelada o aplicada");
                }

                dbDiscountNote.monto = discountNote.monto;
                dbDiscountNote.descripcion = discountNote.descripcion;
                dbDiscountNote.idMoneda = discountNote.idMoneda;
                dbDiscountNote.tipoDeCambio = discountNote.tipoDeCambio;

                _discountNotes.Update(dbDiscountNote);
                _UOW.Save();

                return dbDiscountNote;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotasDeDescuento Add(NotasDeDescuento discountNote)
        {
            try
            {

                //Obtengo una instancia de la configuración
                var config = _config.GetDefault();

                discountNote.idEmpresa = config.Estacion.idEmpresa;

                //La hora en que se esta registrando
                discountNote.fechaHora = DateTime.Now;

                //Antes de registrarla obtengo nuevamente el folio, por si acaso ya se utilizo
                discountNote.folio = Next();

                //Le agrego estado
                discountNote.idEstatusDeNotaDeDescuento = (int)StatusDeNotaDeDescuento.Registrada;

                //Solo requiere la referencia
                discountNote.Factura = null;
                discountNote.Usuario = null;
                discountNote.NotasDeCredito = null;
                discountNote.Cliente = null;

                //Guardo la nota de credito
                _UOW.Reload();
                var local = _discountNotes.Add(discountNote);

                _UOW.Save();

                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotasDeDescuento Find(int idDisccountNote)
        {
            try
            {
                return _discountNotes.Find(idDisccountNote);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotasDeDescuento FindByFolio(int folio)
        {
            try
            {
                return _discountNotes.FindByFolio(folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotasDeDescuento Cancel(int idDiscountNote, string reason)
        {
            try
            {
                var discountNote = _discountNotes.Find(idDiscountNote);

                //Si ya esta cancelada, tiro excepción
                if (discountNote.idEstatusDeNotaDeDescuento.Equals((int)StatusDeNotaDeDescuento.Cancelada))
                    throw new Exception("Esta nota de descuento ya se encuentra cancelada");

                //Si ya esta aplicada, no se puede cancelar
                if (discountNote.idEstatusDeNotaDeDescuento == (int) StatusDeNotaDeDescuento.Aplicada)
                {
                    throw new Exception("No se pueden cancelar notas de descuento aplicadas");
                }

                //Se agrega registro en la tabla de cancelaciones
                discountNote.CancelacionesDeNotaDeDescuento = new CancelacionesDeNotaDeDescuento() { fechaHora = DateTime.Now, motivo = reason };

                discountNote.idEstatusDeNotaDeDescuento = (int)StatusDeNotaDeDescuento.Cancelada;
                discountNote.EstatusDeNotaDeDescuento = null;

                _UOW.Save();

                return discountNote;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<NotasDeDescuento> WithClientOrFolioLike(string value)
        {
            try
            {
                return _discountNotes.WithFolioOrClientLike(value, null).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<NotasDeDescuento> List()
        {
            try
            {
                return _discountNotes.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VMEstadoDeLaEmpresa ListDiscountNotesForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate)
        {
            try
            {
                var detail = _companyStatusDiscountNotes.List(startDate, endDate);

                var detailPesos = detail.Where(x => x.idMoneda == (int)Monedas.Pesos).ToList();
                var detailDollars = detail.Where(x => x.idMoneda == (int)Monedas.Dólares).ToList();

                vm.TotalDolaresNotasDeDescuento = detailDollars.Sum(x => x.importe);
                vm.TotalPesosNotasDeDescuento = detailPesos.Sum(x => x.importe);

                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VMReporteNotasDeDescuento ListDisccountNotesForReport(Cliente customer, DateTime startDate, DateTime endDate,
            bool includeOnlyPending, bool includeOnlyApplied)
        {
            try
            {
                var detail = _discountNotesReport.List(customer, startDate, endDate, includeOnlyPending, includeOnlyApplied).Select(x=>new VMRDetalleDeNotaDeDescuento(x)).ToList();

                return new VMReporteNotasDeDescuento(){Detail = detail, Customer = customer, StartDate = startDate, EndDate = endDate, IncludeOnlyApplied = includeOnlyApplied, IncludeOnlyPending = includeOnlyPending };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
