using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class UnidadDeMedidaService : IUnidadDeMedidaService
    {
        private IUnidadesDeMedidaRepository _units;
        private IEquivalenciasRepository _equivalencies;
        private IArticulosRepository _items;
        private IUnitOfWork _UOW;

        public UnidadDeMedidaService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _units = _UOW.UnidadesDeMedida;
            _equivalencies = _UOW.Equivalencias;
            _items = _UOW.Articulos;
        }

        public UnidadesDeMedida Add(UnidadesDeMedida unitOfMeasure)
        {
            try
            {
                unitOfMeasure.codigo.Trim();
                unitOfMeasure.descripcion.Trim();
                _units.Add(unitOfMeasure);
                _UOW.Save();

                return unitOfMeasure;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UnidadesDeMedida> List()
        {
            try
            {
                return _units.List().Where(u => u.activo).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UnidadesDeMedida> Like(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                    return _units.List().Where(u => u.activo).ToList();
                else
                    return _units.Like(code).Where(u => u.activo).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UnidadesDeMedida> List(int idItem)
        {
            try
            {
                List<UnidadesDeMedida> units;

                units = new List<UnidadesDeMedida>();

                //La unidad de medidad mínima /default
                units.Add(_items.Find(idItem).UnidadesDeMedida);

                //Obtengo las medidas equivalentes
                var equivalencies = _equivalencies.List(idItem);

                //Si hay equivalencies obtengo la unidad de medida de cada una de ellas
                if (equivalencies.Count > 0)
                    equivalencies.ForEach(e => units.Add(e.UnidadesDeMedida));

                return units;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UnidadesDeMedida Find(int id)
        {
            try
            {
                return _units.Find(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UnidadesDeMedida Find(string code)
        {
            try
            {
                //La funcion search regresa un resultado sin cache, no es util para establecer propiedades de navegacion
                //return _units.Search(u => u.codigo.Equals(code, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                return _units.Find(code);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Exists(string code)
        {
            try
            {
                return _units.Search(u => u.codigo.Equals(code, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault() != null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UnidadesDeMedida Update(UnidadesDeMedida unitOfMeasure)
        {
            try
            {
                var local = _units.Find(unitOfMeasure.idUnidadDeMedida);
                local.descripcion = unitOfMeasure.descripcion.Trim();
                local.activo = unitOfMeasure.activo;

                _units.Update(local);
                _UOW.Save();

                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(UnidadesDeMedida unitOfMeasure)
        {
            try
            {
                _units.Remove(unitOfMeasure.idUnidadDeMedida);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CanDelete(UnidadesDeMedida unitOfMeasure)
        {
            try
            {
                return _units.CanDelete(unitOfMeasure.idUnidadDeMedida);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
