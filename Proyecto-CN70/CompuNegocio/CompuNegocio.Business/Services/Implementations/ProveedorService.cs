using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class ProveedorService : IProveedorService
    {
        private IUnitOfWork _UOW;
        private IProveedoresRepository _suppliers;
        private IDomiciliosRepository _addresses;

        public ProveedorService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _suppliers = _UOW.Proveedores;
            _addresses = _UOW.Domicilios;
        }

        public Proveedore Add(Proveedore supplier)
        {
            try
            {
                _suppliers.Add(supplier);
                _UOW.Save();
                return supplier;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Proveedore> List()
        {
            try
            {
                return _suppliers.List().Where(s => s.activo).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Proveedore> WithCodeLike(string code)
        {
            try
            {
                return _suppliers.WithCodeLike(code).Where(s => s.activo).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Proveedore Find(int idSupplier)
        {
            try
            {
                return _suppliers.Find(idSupplier);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Proveedore Find(string code)
        {
            try
            {
                return _suppliers.Find(code);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Proveedore Update(Proveedore supplier)
        {
            try
            {
                var local = _suppliers.Find(supplier.idProveedor);
                local.activo = supplier.activo;
                local.nombreComercial = supplier.nombreComercial;
                local.razonSocial = supplier.razonSocial;
                local.rfc = supplier.rfc;
                local.telefono = supplier.telefono;
                local.Domicilio.calle = supplier.Domicilio.calle;
                local.Domicilio.numeroExterior = supplier.Domicilio.numeroExterior;
                local.Domicilio.numeroInterior = supplier.Domicilio.numeroInterior;
                local.Domicilio.colonia = supplier.Domicilio.colonia;
                local.Domicilio.ciudad = supplier.Domicilio.ciudad;
                local.Domicilio.estado = supplier.Domicilio.estado;
                local.Domicilio.idPais = supplier.Domicilio.idPais;
                local.Domicilio.codigoPostal = supplier.Domicilio.codigoPostal;
                _suppliers.Update(local);
                _UOW.Save();
                return local;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CanDelete(Proveedore supplier)
        {
            try
            {
                return _suppliers.CanDelete(supplier.idProveedor);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(Proveedore supplier)
        {
            try
            {
                if (!supplier.Domicilio.idDomicilio.isValid())
                    supplier = _suppliers.Find(supplier.idProveedor);

                _addresses.Remove(supplier.Domicilio.idDomicilio);
                _suppliers.Remove(supplier.idProveedor);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Exist(string rfc)
        {
            try
            {
                return _suppliers.Exist(rfc);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Import(string dbcPath)
        {
            try
            {
                //Obtengo la lista de clientes de VFP
                var catalog = _suppliers.List(dbcPath);

                //Por cada artículos en el catálogo voy a procesarlo para agregarlo a la base de datos
                foreach (var supplier in catalog)
                {
                    var newSupplier = new Proveedore();
                    //Si ya existe me lo salto
                    newSupplier = _suppliers.SearchAll(supplier.codigo);
                    if (newSupplier.isValid() || !supplier.rfc.isValid() || _suppliers.Exist(supplier.rfc))
                        continue;

                    //Si llegó aqui es nuevo
                    newSupplier = new Proveedore();
                    newSupplier.activo = true;
                    newSupplier.codigo = supplier.codigo;
                    newSupplier.nombreComercial = supplier.nombreComercial;
                    newSupplier.razonSocial = supplier.razonSocial;
                    newSupplier.rfc = supplier.rfc;
                    newSupplier.Domicilio = supplier.Domicilio;
                    newSupplier.Domicilio.numeroInterior = newSupplier.Domicilio.numeroInterior.ToTrimmedString(10);
                    newSupplier.Domicilio.numeroExterior = newSupplier.Domicilio.numeroExterior.ToTrimmedString(10);

                    //Agrego el nuevo artículo
                    _suppliers.Add(newSupplier);
                }

                //Una vez que he procesado todos, entonces hago persistentes los cambios
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
