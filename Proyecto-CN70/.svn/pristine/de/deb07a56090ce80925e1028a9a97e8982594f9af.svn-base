using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;

namespace Aprovi.Business.Services
{
    public abstract class PrivilegioService : IPrivilegioService
    {
        private IUnitOfWork _UOW;
        private IPrivilegiosRepository _privileges;

        public PrivilegioService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _privileges = _UOW.Privilegios;
        }

        public Privilegio Add(Privilegio privilege)
        {
            try
            {
                _privileges.Add(privilege);
                _UOW.Save();
                return privilege;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Privilegio> List(int idUser)
        {
            try
            {
                return _privileges.List(idUser);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Privilegio Find(int idUser, int idView)
        {
            try
            {
                return _privileges.Find(idUser, idView);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(Privilegio privilege)
        {
            try
            {
                _privileges.Remove(privilege);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
