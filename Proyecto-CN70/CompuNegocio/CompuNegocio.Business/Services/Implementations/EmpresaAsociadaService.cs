using System;
using System.Collections.Generic;
using System.Linq;
using Aprovi.Data.Models;
using Aprovi.Data.Core;
using Aprovi.Data.Repositories;

namespace Aprovi.Business.Services
{
    public class EmpresaAsociadaService : IEmpresaAsociadaService
    {
        private IUnitOfWork _UOW;
        private IEmpresaService _companies;
        private IEmpresasAsociadasRepository _associatedCompanies;

        public EmpresaAsociadaService(IUnitOfWork unitOfWork, IEmpresaService companies)
        {
            _UOW = unitOfWork;
            _companies = companies;
            _associatedCompanies = _UOW.EmpresasAsociadas;
        }

        public EmpresasAsociada Add(EmpresasAsociada empresaAsociada)
        {
            try
            {
                if (!empresaAsociada.baseDeDatos.isValid())
                    throw new Exception("Deb especificar la base de datos");

                if (!empresaAsociada.nombre.isValid())
                    throw new Exception("Debe especificar el nombre");

                if (!empresaAsociada.usuario.isValid())
                    throw new Exception("Debe especificar el usuario");

                if (!empresaAsociada.contrasena.isValid())
                    throw new Exception("Debe especificar la contraseña");

                if (!empresaAsociada.servidor.isValid())
                    throw new Exception("Debe especificar el servidor");

                var exist = _associatedCompanies.Find(empresaAsociada.nombre);

                if (exist.isValid() && exist.idEmpresaAsociada.isValid())
                    throw new Exception("Ya existe esta empresa asociada");

                //Se verifica que la empresa no tenga otra empresa asociada registrada
                if (empresaAsociada.idEmpresaLocal.HasValue && empresaAsociada.idEmpresaLocal.Value.isValid())
                {
                    var company = _companies.Find(empresaAsociada.idEmpresaLocal.Value);

                    if (!company.EmpresasAsociadas.IsEmpty())
                    {
                        throw new Exception("La empresa seleccionada ya tiene una empresa asociada registrada");
                    }
                }
                else
                {
                    //Si no tiene empresa, se hace que el valor sea nulo
                    empresaAsociada.idEmpresaLocal = null;
                }

                //No se necesitan las propiedades complejas
                empresaAsociada.Empresa = null;

                empresaAsociada.activa = true;

                _associatedCompanies.Add(empresaAsociada);
                _UOW.Save();

                return empresaAsociada;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CanDelete(EmpresasAsociada empresaAsociada)
        {
            try
            {
                return _associatedCompanies.CanDelete(empresaAsociada);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CompanyExistsByDatabaseName(EmpresasAsociada empresaAsociada)
        {
            try
            {
                return _associatedCompanies.ExistsByDatabaseName(empresaAsociada.baseDeDatos);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(int idEmpresaAsociada)
        {
            try
            {
                var local = _associatedCompanies.Find(idEmpresaAsociada);
                _associatedCompanies.Remove(local);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EmpresasAsociada Find(string nombre)
        {
            try
            {
                return _associatedCompanies.Find(nombre);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EmpresasAsociada Find(int idCuenta)
        {
            try
            {
                return _associatedCompanies.Find(idCuenta);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EmpresasAsociada FindByDatabaseName(string databaseName)
        {
            try
            {
                return _associatedCompanies.FindByDatabaseName(databaseName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EmpresasAsociada> List()
        {
            try
            {
                return _associatedCompanies.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EmpresasAsociada> List(string value)
        {
            try
            {
                return _associatedCompanies.Like(value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EmpresasAsociada Update(EmpresasAsociada empresaAsociada)
        {
            try
            {
                var local = _associatedCompanies.Find(empresaAsociada.idEmpresaAsociada);

                local.nombre = empresaAsociada.nombre;
                local.baseDeDatos = empresaAsociada.baseDeDatos;
                local.servidor = empresaAsociada.servidor;
                local.idEmpresaLocal = empresaAsociada.idEmpresaLocal;
                local.usuario = empresaAsociada.usuario;
                local.contrasena = empresaAsociada.contrasena;
                local.activa = true;

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
