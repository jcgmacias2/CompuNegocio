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
    public abstract class CodigoDeArticuloPorClienteService : ICodigoDeArticuloPorClienteService
    {
        private IUnitOfWork _UOW;
        private ICodigosDeArticuloPorClienteRepository _codes;

        public CodigoDeArticuloPorClienteService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _codes = _UOW.CodigosDeArticuloPorCliente;
        }

        public CodigosDeArticuloPorCliente Add(CodigosDeArticuloPorCliente code)
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

        public void Remove(CodigosDeArticuloPorCliente code)
        {
            try
            {
                if (!code.idCodigoDeArticuloPorCliente.isValid())
                    throw new Exception("No es un código válido");

                _codes.Remove(code.idCodigoDeArticuloPorCliente);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CodigosDeArticuloPorCliente Update(CodigosDeArticuloPorCliente code)
        {
            try
            {
                if (!code.idCodigoDeArticuloPorCliente.isValid())
                    throw new Exception("No es un código válido");

                var local = _codes.Find(code.idCodigoDeArticuloPorCliente);
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
