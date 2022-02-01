using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class ClasificacionService: IClasificacionService
    {
        private IUnitOfWork _UOW;
        private IClasificacionesRepository _clasification;

        public ClasificacionService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _clasification = _UOW.Clasificaciones;
        }

        public Clasificacione Add(Clasificacione clasification)
        {
            try
            {
                _clasification.Add(clasification);
                _UOW.Save();
                return clasification;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Clasificacione> List()
        {
            try
            {
                return _clasification.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Clasificacione> WithNameLike(string code)
        {
            try
            {
                return _clasification.WithNameLike(code);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Clasificacione Find(int idClasification)
        {
            try
            {
                return _clasification.Find(idClasification);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Clasificacione Find(string code)
        {
            try
            {
                return _clasification.Find(code);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Clasificacione Update(Clasificacione clasification)
        {
            try
            {
                var local = _clasification.Find(clasification.idClasificacion);
                local.descripcion = clasification.descripcion;
                _clasification.Update(local);
                _UOW.Save();
                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(Clasificacione clasification)
        {
            try
            {
                _clasification.Remove(clasification.idClasificacion);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CanDelete(Clasificacione clasification)
        {
            try
            {
                return _clasification.CanDelete(clasification.idClasificacion);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
