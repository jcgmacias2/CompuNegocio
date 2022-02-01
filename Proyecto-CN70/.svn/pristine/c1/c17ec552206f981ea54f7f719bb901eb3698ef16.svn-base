using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class EmpresaService : IEmpresaService
    {
        private IUnitOfWork _UOW;
        private IEmpresasRepository _businesses;
        private IBasculasRepository _scales;

        public EmpresaService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _businesses = _UOW.Empresas;
            _scales = _UOW.Basculas;
        }

        public Empresa Add(Empresa business)
        {
            try
            {
                business.activa = true;
                _businesses.Add(business);
                _UOW.Save();
                return business;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Empresa Find(int idBusiness)
        {
            try
            {
                return _businesses.Find(idBusiness);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Empresa Find(string description)
        {
            try
            {
                return _businesses.Find(description);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Empresa Update(Empresa business)
        {
            try
            {
                var local = _businesses.Find(business.idEmpresa);
                local.activa = business.activa;
                local.descripcion = business.descripcion;
                local.licencia = business.licencia;
                _businesses.Update(local);
                _UOW.Save();
                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CanDelete(Empresa business)
        {
            try
            {
                return _businesses.CanDelete(business.idEmpresa);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(Empresa business)
        {
            try
            {
                _businesses.Remove(business.idEmpresa);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Empresa> List()
        {
            try
            {
                return _businesses.List().Where(r => r.activa).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Empresa> WithDescriptionLike(string description)
        {
            try
            {
                return _businesses.WithDescriptionLike(description).Where(c => c.activa).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
