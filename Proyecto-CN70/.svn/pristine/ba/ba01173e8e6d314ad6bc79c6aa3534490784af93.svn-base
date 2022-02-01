using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;

namespace Aprovi.Presenters
{
    public class AssociatedCompaniesPresenter
    {
        private IAssociatedCompaniesView _view;
        private IEmpresaAsociadaService _associatedCompanies;
        private IEmpresaService _companies;

        public AssociatedCompaniesPresenter(IAssociatedCompaniesView view, IEmpresaAsociadaService associatedCompanies, IEmpresaService companies)
        {
            _view = view;
            _associatedCompanies = associatedCompanies;
            _companies = companies;

            _view.Find += Find;
            _view.FindCompany += FindCompany;
            _view.New += New;
            _view.OpenList += OpenList;
            _view.OpenCompaniesList += OpenCompaniesList;
            _view.Delete += Delete;
            _view.Update += Update;
            _view.Save += Save;
            _view.Quit += Quit;
        }

        private void FindCompany()
        {
            try
            {
                var vm = _view.AssociatedCompany;

                if (!_view.AssociatedCompany.Empresa.descripcion.isValid())
                {
                    //Si no se limpia la empresa, se quedaría guardadada la empresa anterior anterior
                    vm.Empresa = new Empresa();
                    _view.Show(vm);

                    return;
                }

                var company = _companies.Find(_view.AssociatedCompany.Empresa.descripcion);

                if (company.isValid())
                    vm.Empresa = company;
                else
                {
                    vm.Empresa = new Empresa();
                    _view.ShowError("No se encontró una empresa con la descripción proporcionada");
                }

                _view.Show(vm);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenCompaniesList()
        {
            try
            {
                IBusinessesListView view;
                BusinessesListPresenter presenter;

                view = new BusinessesListView();
                presenter = new BusinessesListPresenter(view, _companies);

                view.ShowWindow();

                if (view.Business.isValid() && view.Business.idEmpresa.isValid())
                {
                    var vm = _view.AssociatedCompany;
                    vm.Empresa = view.Business;

                    _view.Show(vm);
                }
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

        private void Save()
        {
            try
            {
                //Se debe verificar que la empresa no tenga otra empresa asociada
                var vm = _view.AssociatedCompany;

                if (vm.idEmpresaLocal.isValid())
                {
                    var company = _companies.Find(vm.idEmpresaLocal.Value);

                    if (company.EmpresasAsociadas.Count > 0)
                    {
                        _view.ShowError("La empresa seleccionada ya está asignada a otra empresa asociada");
                        return;
                    }
                }

                _associatedCompanies.Add(vm);

                _view.ShowMessage("Empresa asociada {0} registrada exitosamente", _view.AssociatedCompany.nombre);

                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Update()
        {
            try
            {
                //Se debe verificar que la empresa no tenga otra empresa asociada
                var vm = _view.AssociatedCompany;

                if (vm.idEmpresaLocal.isValid())
                {
                    var company = _companies.Find(vm.idEmpresaLocal.Value);

                    if (company.EmpresasAsociadas.Count > 0)
                    {
                        _view.ShowError("La empresa seleccionada ya está asignada a otra empresa asociada");
                        return;
                    }
                }

                _associatedCompanies.Update(vm);

                _view.ShowMessage("Empresa asociada {0} actualizada exitosamente", _view.AssociatedCompany.nombre);
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Delete()
        {
            try
            {
                if (!_associatedCompanies.CanDelete(_view.AssociatedCompany))
                    throw new Exception("Esta empresa asociada esta relacionada, por lo que no puede ser eliminada");

                _associatedCompanies.Delete(_view.AssociatedCompany.idEmpresaAsociada);

                _view.ShowMessage("Empresa asociada eliminada exitosamente");
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
                IAssociatedCompaniesListView view;
                AssociatedCompaniesListPresenter presenter;

                view = new AssociatedCompaniesListView();
                presenter = new AssociatedCompaniesListPresenter(view, _associatedCompanies);

                view.ShowWindow();

                if (view.AssociatedCompany.isValid() && view.AssociatedCompany.idEmpresaAsociada.isValid())
                {
                    _view.Clear();
                    _view.Show(view.AssociatedCompany);
                }
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

        private void Find()
        {
            try
            {
                var company = _associatedCompanies.Find(_view.AssociatedCompany.nombre);

                if (company.isValid())
                {
                    _view.Clear();
                    _view.Show(company);
                }
                else
                    _view.Show(new EmpresasAsociada() { nombre = _view.AssociatedCompany.nombre, Empresa = new Empresa()});
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
