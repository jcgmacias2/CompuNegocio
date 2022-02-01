using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;

namespace Aprovi.Business.Services
{
    public abstract class AbonoDeRemisionService : IAbonoDeRemisionService
    {
        private IUnitOfWork _UOW;
        private IAbonosDeRemisionRepository _payments;
        private IViewFoliosPorAbonosRepository _folios;

        public AbonoDeRemisionService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _payments = _UOW.AbonosDeRemision;
            _folios = _UOW.FoliosPorAbono;
        }

        public AbonosDeRemision Add(AbonosDeRemision payment)
        {
            try
            {
                //Agrego los defaults
                payment.fechaHora = DateTime.Now;
                payment.folio = GetNextFolio();
                payment.idEstatusDeAbono = (int)StatusDeAbono.Registrado;
                payment.FormasPago = null;
                payment.Moneda = null;

                _payments.Add(payment);
                _UOW.Save();

                return payment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AbonosDeRemision Find(int idPayment)
        {
            try
            {
                return _payments.Find(idPayment);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AbonosDeRemision Find(string folio)
        {
            try
            {
                return _payments.Find(folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AbonosDeRemision> List(int idBillOfLanding)
        {
            try
            {
                return _payments.List(idBillOfLanding);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AbonosDeRemision> ListAbonosLike(string value)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetNextFolio()
        {
            try
            {
                return _folios.Next();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AbonosDeRemision Cancel(int idPayment)
        {
            try
            {
                var local = _payments.Find(idPayment);
                //Si ya esta cancelada, tiro excepción
                if (local.idEstatusDeAbono.Equals((int)StatusDeAbono.Cancelado))
                    throw new Exception("Este abono ya se encuentra cancelado");

                local.idEstatusDeAbono = (int)StatusDeAbono.Cancelado;
                local.EstatusDeAbono = null;

                _payments.Update(local);
                _UOW.Save();
                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
