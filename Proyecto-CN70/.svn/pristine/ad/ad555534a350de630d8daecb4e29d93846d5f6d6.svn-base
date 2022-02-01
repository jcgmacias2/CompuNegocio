using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class SuppliersPresenter
    {
        private readonly ISuppliersView _view;
        private IProveedorService _suppliers;
        private ICatalogosEstaticosService _catalogs;

        public SuppliersPresenter(ISuppliersView view, IProveedorService suppliersService, ICatalogosEstaticosService catalogs)
        {
            _view = view;
            _suppliers = suppliersService;
            _catalogs = catalogs;

            _view.Quit += Quit;
            _view.New += New;
            _view.Delete += Delete;
            _view.Save += Save;
            _view.Find += Find;
            _view.Update += Update;
            _view.OpenList += OpenList;

            _view.FillCombo(_catalogs.ListPaises());
        }

        private void OpenList()
        {
            try
            {
                ISuppliersListView view;
                SuppliersListPresenter presenter;

                view = new SuppliersListView();
                presenter = new SuppliersListPresenter(view, _suppliers);

                view.ShowWindow();

                //Si seleccionó alguno lo muestro
                if (view.Supplier.idProveedor.isValid())
                    _view.Show(view.Supplier);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Update()
        {
            string error;

            if(!IsSupplierValid(_view.Supplier,out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                var supplier = _view.Supplier;
                supplier.activo = true;
                _suppliers.Update(supplier);

                _view.ShowMessage(string.Format("Proveedor {0} modificado exitosamente", supplier.nombreComercial));
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            if (!_view.Supplier.codigo.isValid())
                return;

            try
            {
                var supplier = _suppliers.Find(_view.Supplier.codigo);

                if (supplier.isValid() && !supplier.activo)
                    _view.ShowMessage("El proveedor ya existe pero esta marcado como inactivo, para reactivarlo solo de click en Guardar");

                if (supplier == null)
                    supplier = new Proveedore() { codigo = _view.Supplier.codigo, Domicilio = new Domicilio() };

                _view.Show(supplier);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Save()
        {
            string error;
            if(!IsSupplierValid(_view.Supplier,out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                var supplier = _view.Supplier;
                supplier.activo = true;
                _suppliers.Add(supplier);

                _view.ShowMessage(string.Format("El proveedor {0} ha sido agregado exitosamente", supplier.nombreComercial));
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Delete()
        {
            if(!_view.IsDirty)
            {
                _view.ShowError("No hay ningún proveedor seleccionado para eliminar");
                return;
            }

            try
            {
                if(_suppliers.CanDelete(_view.Supplier))
                {
                    _suppliers.Delete(_view.Supplier);
                }
                else
                {
                    var supplier = _view.Supplier;
                    supplier.activo = false;
                    _suppliers.Update(supplier);
                }

                _view.ShowMessage(string.Format("Proveedor {0} removido exitosamente", _view.Supplier.nombreComercial));
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void New()
        {
            try
            {
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Quit()
        {
            try
            {
                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private bool IsSupplierValid(Proveedore supplier, out string error)
        {
            if (!supplier.codigo.isValid())
            {
                error = "Código del proveedor inválido";
                return false;
            }

            if (!supplier.nombreComercial.isValid())
            {
                error = "Nombre comercial no es válido";
                return false;
            }

            if (!supplier.razonSocial.isValid())
            {
                error = "Razón social no es válida";
                return false;
            }

            if (!supplier.Domicilio.calle.isValid())
            {
                error = "La calle no es válida";
                return false;
            }

            if (!supplier.Domicilio.numeroExterior.isValid())
            {
                error = "Número exterior no es válido";
                return false;
            }

            if (!supplier.Domicilio.colonia.isValid())
            {
                error = "Colonia no es válida";
                return false;
            }

            if (!supplier.Domicilio.ciudad.isValid())
            {
                error = "Ciudad no es válida";
                return false;
            }

            if (!supplier.Domicilio.estado.isValid())
            {
                error = "Estado no es válida";
                return false;
            }

            if (!supplier.Domicilio.idPais.isValid())
            {
                error = "País no es válido";
                return false;
            }

            if (!supplier.Domicilio.codigoPostal.isValid())
            {
                error = "Código postal no es válido";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }
}
