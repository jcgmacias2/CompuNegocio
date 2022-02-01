using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Aprovi.Business.ViewModels;

namespace Aprovi.Business.Services
{
    public abstract class ImpuestoService : IImpuestoService
    {
        private IImpuestosRepository _taxes;
        private IUnitOfWork _UOW;

        public ImpuestoService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _taxes = _UOW.Impuestos;
        }

        public Impuesto Add(Impuesto tax)
        {
            try
            {
                tax.TiposDeImpuesto = null;
                tax.TiposFactor = null;
                _taxes.Add(tax);
                _UOW.Save();
                return tax;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public List<Impuesto> List(bool includeInactive)
        {
            try
            {
                if (includeInactive)
                    return _taxes.List().ToList();
                else
                    return _taxes.List().Where(t => t.activo).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Impuesto> WithNameLike(string name)
        {
            try
            {
                return _taxes.WithNameLike(name).Where(i => i.activo).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Impuesto Find(int id)
        {
            try
            {
                return _taxes.Find(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Impuesto Find(string code, decimal rate, TiposDeImpuesto type)
        {
            try
            {
                return _taxes.Find(code, rate, type);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Impuesto Update(Impuesto tax)
        {
            try
            {
                var local = _taxes.Find(tax.idImpuesto);
                local.nombre = tax.nombre;
                local.idTipoDeImpuesto = tax.idTipoDeImpuesto;
                local.activo = tax.activo;

                _UOW.Save();
                return local;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(Impuesto tax)
        {
            try
            {
                if (!_taxes.CanDelete(tax.idImpuesto))
                    throw new Exception("El impuesto ya se encuentra relacionado, por lo que no puede eliminarse");

                _taxes.Remove(tax.idImpuesto);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CanDelete(Impuesto tax)
        {
            try
            {
                return _taxes.CanDelete(tax.idImpuesto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VMRImpuestosPorPeriodo ListTaxesPerPeriod(DateTime startDate, DateTime endDate)
        {
            try
            {
                //Se obtiene la lista de impuestos
                var taxes = _taxes.List().ToList();
                var incomeDetail = new List<VMRDetalleImpuestosPorPeriodo>();
                var outcomeDetail = new List<VMRDetalleImpuestosPorPeriodo>();
                var detail = _taxes.List(startDate, endDate);

                //Ingresos (facturas)
                foreach (var t in taxes)
                {
                    //Se buscan las transacciones relacionadas al impuesto
                    var transactions = detail.Where(x => x.idImpuesto == t.idImpuesto && x.idFactura != null).ToList();

                    incomeDetail.Add(new VMRDetalleImpuestosPorPeriodo()
                    {
                        ValorImpuesto = t.valor,
                        DescripcionImpuesto = t.nombre,
                        DescripcionTipoDeImpuesto = t.TiposDeImpuesto.descripcion,
                        DescripcionTipoFactor = t.TiposFactor.codigo,
                        IdImpuesto = t.idImpuesto,
                        IdTipoDeImpuesto = t.idTipoDeImpuesto,
                        IdTipoFactor = t.idTipoFactor,
                        Importe = transactions.Sum(x => x.importe.GetValueOrDefault(0).ToDocumentCurrency(new Moneda() { idMoneda = x.idMoneda }, new Moneda() { idMoneda = (int)Monedas.Pesos }, x.tipoDeCambio)),
                        BaseGravable = transactions.Sum(x => x.baseGravable.GetValueOrDefault(0m).ToDocumentCurrency(new Moneda() { idMoneda = x.idMoneda }, new Moneda() { idMoneda = (int)Monedas.Pesos }, x.tipoDeCambio))
                    });
                }

                //Egresos (notas de credito)
                foreach (var t in taxes)
                {
                    //Se buscan las transacciones relacionadas al impuesto
                    var transactions = detail.Where(x => x.idImpuesto == t.idImpuesto && x.idNotaDeCredito != null).ToList();

                    outcomeDetail.Add(new VMRDetalleImpuestosPorPeriodo()
                    {
                        ValorImpuesto = t.valor,
                        DescripcionImpuesto = t.nombre,
                        DescripcionTipoDeImpuesto = t.TiposDeImpuesto.descripcion,
                        DescripcionTipoFactor = t.TiposFactor.codigo,
                        IdImpuesto = t.idImpuesto,
                        IdTipoDeImpuesto = t.idTipoDeImpuesto,
                        IdTipoFactor = t.idTipoFactor,
                        Importe = transactions.Sum(x => x.importe.GetValueOrDefault(0).ToDocumentCurrency(new Moneda() { idMoneda = x.idMoneda }, new Moneda() { idMoneda = (int)Monedas.Pesos }, x.tipoDeCambio)),
                        BaseGravable = transactions.Sum(x => x.baseGravable.GetValueOrDefault(0m).ToDocumentCurrency(new Moneda() { idMoneda = x.idMoneda }, new Moneda() { idMoneda = (int)Monedas.Pesos }, x.tipoDeCambio))
                    });
                }

                return new VMRImpuestosPorPeriodo(taxes, incomeDetail, outcomeDetail, startDate, endDate);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
