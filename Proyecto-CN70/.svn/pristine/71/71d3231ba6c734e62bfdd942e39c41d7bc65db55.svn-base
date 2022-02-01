using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;

namespace Aprovi.Business.Services
{
    public abstract class CodigoDeArticuloPorProveedorService : ICodigoDeArticuloPorProveedorService
    {
        private IUnitOfWork _UOW;
        private ICodigosDeArticuloPorProveedorRepository _codes;

        public CodigoDeArticuloPorProveedorService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _codes = _UOW.CodigosDeArticuloPorProveedor;
        }

        public CodigosDeArticuloPorProveedor Add(CodigosDeArticuloPorProveedor code)
        {
            try
            {
                _codes.Add(code);
                _UOW.Save();

                return code;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Remove(CodigosDeArticuloPorProveedor code)
        {
            try
            {
                if (!code.idCodigoDeArticuloPorProveedor.isValid())
                    throw new Exception("No es un código válido");

                _codes.Remove(code.idCodigoDeArticuloPorProveedor);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CodigosDeArticuloPorProveedor Update(CodigosDeArticuloPorProveedor code)
        {
            try
            {
                if (!code.idCodigoDeArticuloPorProveedor.isValid())
                    throw new Exception("No es un código válido");

                var local = _codes.Find(code.idCodigoDeArticuloPorProveedor);
                local.codigo = code.codigo;
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
