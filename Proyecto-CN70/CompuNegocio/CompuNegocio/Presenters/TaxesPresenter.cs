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
    public class TaxesPresenter
    {
        private readonly ITaxesView _view;
        private IImpuestoService _taxes;
        private ICatalogosEstaticosService _catalogs;

        public TaxesPresenter(ITaxesView view, IImpuestoService taxesService, ICatalogosEstaticosService catalogs)
        {
            _view = view;
            _taxes = taxesService;
            _catalogs = catalogs;

            _view.New += New;
            _view.Delete += Delete;
            _view.Save += Save;
            _view.Update += Update;
            _view.OpenList += OpenList;
            _view.Quit += Quit;

            _view.FillCombos(_catalogs.ListTiposDeImpuesto(), Enum.GetValues(typeof(Impuestos)).Cast<Impuestos>().ToList(), _catalogs.ListTiposFactor());
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

        private void Delete()
        {
            if(!_view.IsDirty)
            {
                _view.ShowError("No existe un impuesto seleccionado para eliminar");
                return;
            }
            try
            {
                if (_taxes.CanDelete(_view.Tax))
                {
                    _taxes.Delete(_view.Tax);
                }
                else
                {
                    var local = _view.Tax;
                    local.activo = false;
                    _taxes.Update(local);
                }

                _view.ShowMessage(string.Format("Impuesto {0} removido exitosamente", _view.Tax.nombre));
                _view.Clear();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Save()
        {
            string error;

            if (!IsTaxValid(_view.Tax, out error))
            {
                _view.ShowError(error);
                return;
            }

            //Verifico que no exista un impuesto de ese tipo y con esta tasa
            if(_taxes.Find(_view.Tax.codigo, _view.Tax.valor, _view.Tax.TiposDeImpuesto) != null)
            {
                _view.ShowMessage(string.Format("Ya existe un impuesto {0} con tasa del {1} %, por lo que no es posible realizar este registro", _view.Tax.TiposDeImpuesto.descripcion, _view.Tax.valor.ToDecimalString()));
                return;
            }

            try
            {
                var local = _view.Tax;
                local.activo = true;

                _taxes.Add(local);
                _view.ShowMessage(string.Format("Impuesto {0} agregado exitosamente", local.nombre));
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Update()
        {
            string error;

            if (!IsTaxValid(_view.Tax, out error))
            {
                _view.ShowError(error);
                return;
            }

            //Busco algun impuesto con la misma tasa y del mismo tipo
            var taxValidation = _taxes.Find(_view.Tax.codigo, _view.Tax.valor, _view.Tax.TiposDeImpuesto);

            //Si existe alguno y no es el mismo que se esta editando, le mando el aviso
            if (taxValidation != null && !_view.Tax.idImpuesto.Equals(taxValidation.idImpuesto))
            {
                _view.ShowMessage(string.Format("Ya existe un impuesto {0} con tasa del {1} %, por lo que no es posible realizar este registro", _view.Tax.TiposDeImpuesto.descripcion, _view.Tax.valor.ToDecimalString()));
                return; 
            }

            try
            {
                var local = _view.Tax;
                local.activo = true;

                _taxes.Update(local);
                _view.ShowMessage(string.Format("Impuesto {0} actualizado exitosamente", local.nombre));
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenList()
        {
            try
            {
                ITaxesListView view;
                TaxesListPresenter presenter;

                view = new TaxesListView();
                presenter = new TaxesListPresenter(view, _taxes, true);

                view.ShowWindow();

                if (view.Tax.isValid() && view.Tax.idImpuesto.isValid())
                {
                    if(!view.Tax.activo)
                    {
                        _view.ShowMessage("Este impuesto esta marcado como inactivo, para reactivarlo solo de click en Guardar");
                    }
                    _view.Show(view.Tax);
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private bool IsTaxValid(Impuesto tax, out string error)
        {
            if(!tax.isValid())
            {
                error = "Impuesto inválido";
                return false;
            }

            if(!tax.nombre.isValid())
            {
                error = "Nombre de impuesto inválido";
                return false;
            }

            if(tax.valor < 0.0m)
            {
                error = "Tasa de impuesto inválida";
                return false;
            }

            if(!tax.idTipoDeImpuesto.isValid())
            {
                error = "Tipo de impuesto inválido";
                return false;
            }

            if(!tax.idTipoFactor.isValid())
            {
                error = "Tipo de factor inválido";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }
}
