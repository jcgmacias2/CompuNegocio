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
    public class BancoService : IBancoService
    {
        private IUnitOfWork _UOW;
        private IBancosRepository _banks;

        public BancoService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _banks = _UOW.Bancos;
        }

        public Banco Add(Banco banco)
        {
            try
            {
                if (!banco.nombre.isValid())
                    throw new Exception("Debe especificar el nombre del banco");

                var exist = _banks.Find(banco.nombre);

                if (exist.isValid() && exist.idBanco.isValid())
                    throw new Exception("Este banco ya esta registrado");
                else
                    exist = new Banco() { nombre = banco.nombre };

                _banks.Add(exist);
                _UOW.Save();

                return exist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CanDelete(int idBanco)
        {
            try
            {
                return _banks.CanDelete(idBanco);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(Banco banco)
        {
            try
            {
                var local = _banks.Find(banco.idBanco);
                _banks.Remove(local);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Banco Find(string nombre)
        {
            try
            {
                return _banks.Find(nombre);                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Banco Find(int idBanco)
        {
            try
            {
                return _banks.Find(idBanco);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Banco> List()
        {
            try
            {
                return _banks.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Banco> List(string value)
        {
            try
            {
                return _banks.Like(value);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
