using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;
using Aprovi.Data.Core;
using Aprovi.Data.Repositories;

namespace Aprovi.Business.Services
{
    public class UsosCFDIService : IUsosCFDIService
    {
        private IUnitOfWork _UOW;
        private IUsosCFDIRepository _usos;

        public UsosCFDIService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _usos = _UOW.UsosCFDI;
        }

        public UsosCFDI Deactivate(UsosCFDI uso)
        {
            try
            {
                var local = _usos.Find(uso.idUsoCFDI);
                local.activo = false;
                _UOW.Save();
                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UsosCFDI Find(string code)
        {
            try
            {
                return _usos.Find(code);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UsosCFDI Find(int id)
        {
            try
            {
                return _usos.Find(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UsosCFDI> List()
        {
            try
            {
                return _usos.List();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UsosCFDI> List(string value)
        {
            try
            {
                return _usos.Like(value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UsosCFDI Reactivate(UsosCFDI uso)
        {
            try
            {
                var local = _usos.Find(uso.idUsoCFDI);
                local.activo = true;
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
