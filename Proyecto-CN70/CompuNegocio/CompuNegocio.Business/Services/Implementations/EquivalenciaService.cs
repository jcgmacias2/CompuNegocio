using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public abstract class EquivalenciaService : IEquivalenciaService
    {
        private IUnitOfWork _UOW;
        private IEquivalenciasRepository _equivalencies;

        public EquivalenciaService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _equivalencies = _UOW.Equivalencias;
        }

        public Equivalencia Add(Equivalencia equivalency)
        {
            try
            {
                equivalency.Articulo = null;
                equivalency.activa = true;
                _equivalencies.Add(equivalency);
                _UOW.Save();
                return equivalency;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Equivalencia Find(int idEquivalence)
        {
            try
            {
                return _equivalencies.Find(idEquivalence);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Equivalencia Find(int idItem, int idUnitOfMeasure)
        {
            try
            {
                return _equivalencies.Find(idItem, idUnitOfMeasure);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Equivalencia> List()
        {
            try
            {
                return _equivalencies.List().Where(e => e.activa).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Equivalencia> List(int idItem)
        {
            try
            {
                return _equivalencies.List(idItem).Where(e => e.activa).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Equivalencia Update(Equivalencia equivalence)
        {
            try
            {
                var local = _equivalencies.Find(equivalence.idEquivalencia);
                local.activa = equivalence.activa;
                local.unidades = equivalence.unidades;
                _equivalencies.Update(local);
                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(Equivalencia equivalence)
        {
            try
            {
                _equivalencies.Remove(equivalence.idEquivalencia);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool HasOperations(Equivalencia equivalence)
        {
            try
            {
                return _equivalencies.HasOperations(equivalence.idEquivalencia);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
