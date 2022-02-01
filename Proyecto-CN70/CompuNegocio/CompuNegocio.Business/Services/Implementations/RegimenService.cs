using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;

namespace Aprovi.Business.Services
{
    public abstract class RegimenService : IRegimenService
    {
        private IUnitOfWork _UOW;
        private IRegimenesRepository _regimes;

        public RegimenService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _regimes = _UOW.Regimenes;
        }

        public Regimene Add(Regimene regime)
        {
            try
            {
                var exists = _regimes.Find(regime.codigo);
                //Si ya existía solo lo reactivo
                if (exists.isValid())
                    exists.activo = true;
                else
                {
                    regime.activo = true;
                    _regimes.Add(regime);
                }

                _UOW.Save();
                return regime;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Regimene Find(int idRegime)
        {
            try
            {
                return _regimes.Find(idRegime);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(Regimene regime)
        {
            try
            {
                var local = _regimes.Find(regime.idRegimen);
                //Si tiene facturas relacionadas, no puedo eliminarlo
                if (local.Facturas.Count > 0)
                    local.activo = false;
                else
                     _regimes.Remove(regime.idRegimen);

                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
