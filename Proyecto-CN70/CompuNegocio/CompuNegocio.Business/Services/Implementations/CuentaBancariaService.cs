using System;
using System.Collections.Generic;
using System.Linq;
using Aprovi.Data.Models;
using Aprovi.Data.Core;
using Aprovi.Data.Repositories;

namespace Aprovi.Business.Services
{
    public class CuentaBancariaService : ICuentaBancariaService
    {
        private IUnitOfWork _UOW;
        private ICuentasBancariasRepository _accounts;

        public CuentaBancariaService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _accounts = _UOW.CuentasBancarias;
        }

        public CuentasBancaria Add(CuentasBancaria cuenta)
        {
            try
            {
                if (!cuenta.Banco.idBanco.isValid())
                    throw new Exception("Debe especificar el banco al que pertenece");

                if (!cuenta.Moneda.idMoneda.isValid())
                    throw new Exception("Deb especificar la moneda de la cuenta");

                if (!cuenta.numeroDeCuenta.isValid())
                    throw new Exception("Debe especificar el número de cuenta");

                var exist = _accounts.Find(cuenta.numeroDeCuenta);

                if (exist.isValid() && exist.idCuentaBancaria.isValid())
                    throw new Exception("Ya existe esta cuenta");

                _accounts.Add(cuenta);
                _UOW.Save();

                return cuenta;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CanDelete(CuentasBancaria cuenta)
        {
            try
            {
                return _accounts.CanDelete(cuenta);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(int idCuenta)
        {
            try
            {
                var local = _accounts.Find(idCuenta);
                _accounts.Remove(local);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CuentasBancaria Find(string numero)
        {
            try
            {
                return _accounts.Find(numero);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CuentasBancaria Find(int idCuenta)
        {
            try
            {
                return _accounts.Find(idCuenta);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CuentasBancaria> List()
        {
            try
            {
                return _accounts.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CuentasBancaria> List(string value)
        {
            try
            {
                return _accounts.Like(value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CuentasBancaria Update(CuentasBancaria cuenta)
        {
            try
            {
                var local = _accounts.Find(cuenta.idCuentaBancaria);

                if (!local.idMonedas.Equals(cuenta.idMonedas))
                    throw new Exception("No es posible cambiar la moneda de una cuenta ya utilizada");

                local.Banco = null;
                local.Moneda = null;
                local.idBanco = cuenta.idBanco;
                local.idMonedas = cuenta.idMonedas;
                local.numeroDeCuenta = cuenta.numeroDeCuenta;
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
