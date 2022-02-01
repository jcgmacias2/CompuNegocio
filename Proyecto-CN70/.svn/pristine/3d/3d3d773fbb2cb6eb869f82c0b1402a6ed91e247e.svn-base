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
    public class CuentaPredialService : ICuentaPredialService
    {
        private IUnitOfWork _UOW;
        private ICuentasPredialesRepository _cuentas;

        public CuentaPredialService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _cuentas = _UOW.CuentasPrediales;
        }

        public CuentasPrediale Add(CuentasPrediale cuenta)
        {
            try
            {
                var exist = _cuentas.Find(cuenta.cuenta);
                if (exist.isValid())
                    throw new Exception("Ya existe un registro con la misma cuenta");

                _cuentas.Add(cuenta);
                _UOW.Save();

                return cuenta;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CuentasPrediale Find(int idCuenta)
        {
            try
            {
                return _cuentas.Find(idCuenta);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CuentasPrediale Find(string cuenta)
        {
            try
            {
                return _cuentas.Find(cuenta);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CuentasPrediale> List()
        {
            try
            {
                return _cuentas.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CuentasPrediale> List(string value)
        {
            try
            {
                return _cuentas.Like(value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Remove(CuentasPrediale cuenta)
        {
            try
            {
                var local = _cuentas.Find(cuenta.idCuentaPredial);
                _cuentas.Remove(local);
                _UOW.Save();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
