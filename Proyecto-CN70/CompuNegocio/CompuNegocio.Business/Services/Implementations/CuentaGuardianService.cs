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
    public class CuentaGuardianService : ICuentaGuardianService
    {
        private IUnitOfWork _UOW;
        private ICuentasGuardianRepository _accounts;

        public CuentaGuardianService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _accounts = _UOW.CuentasGuardian;
        }

        public CuentasGuardian Add(CuentasGuardian account)
        {
            try
            {
                if (!account.idConfiguracion.isValid())
                    throw new Exception("La cuenta debe estar asociada a una configuración");

                _accounts.Add(account);
                _UOW.Save();

                return account;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CuentasGuardian Find(int idConfiguration, string accountAddress)
        {
            try
            {
                return _accounts.Find(idConfiguration, accountAddress);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CuentasGuardian> List(int idConfiguration)
        {
            try
            {
                return _accounts.List(idConfiguration);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Remove(CuentasGuardian account)
        {
            try
            {
                if (!account.idCuentaGuardian.isValid())
                    throw new Exception("La cuenta no tiene identificador");

                var local = _accounts.Find(account.idCuentaGuardian);
                _accounts.Remove(local);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
