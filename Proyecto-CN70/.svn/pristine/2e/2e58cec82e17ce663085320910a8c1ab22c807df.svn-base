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
    public class CuentaDeCorreoService : ICuentaDeCorreoService
    {
        private IUnitOfWork _UOW;
        private ICuentasDeCorreoRepository _accounts;

        public CuentaDeCorreoService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _accounts = _UOW.CuentasDeCorreo;
        }

        public CuentasDeCorreo Add(CuentasDeCorreo account)
        {
            try
            {
                if (!account.idCliente.isValid())
                    throw new Exception("Debe especificar el cliente al que pertenece");

                if (!account.cuenta.isValid())
                    throw new Exception("Cuenta de correo inválida");

                _accounts.Add(account);
                _UOW.Save();
                return account;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CuentasDeCorreo> List(int idCliente)
        {
            try
            {
                return _accounts.List(idCliente);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Remove(CuentasDeCorreo account)
        {
            try
            {
                var local = _accounts.Find(account.idCuentaDeCorreo);
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
