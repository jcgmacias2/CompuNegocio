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
    public class ClassificationsPresenter
    {
        private readonly IClassificationsView _view;
        private IClasificacionService _classifications;

        public ClassificationsPresenter(IClassificationsView view, IClasificacionService classificationsService)
        {
            _view = view;
            _classifications = classificationsService;

            _view.Find += Find;
            _view.New += New;
            _view.Delete += Delete;
            _view.Save += Save;
            _view.Update += Update;
            _view.OpenList += OpenList;
            _view.Quit += Quit;
        }

        private void Quit()
        {
            try
            {
                _view.CloseWindow();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OpenList()
        {
            try
            {
                IClassificationsListView view;
                ClassificationsListPresenter presenter;

                view = new ClassificationsListView();
                presenter = new ClassificationsListPresenter(view, _classifications);

                view.ShowWindow();

                if (view.Classification.isValid() && view.Classification.idClasificacion.isValid())
                    _view.Show(view.Classification);

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Update()
        {
            if (!_view.Classification.descripcion.isValid())
            {
                _view.ShowError("La clasificación no es válida");
                return;
            }

            try
            {
                _classifications.Update(_view.Classification);
                _view.ShowMessage(string.Format("Clasificación {0} actualizada exitosamente", _view.Classification.descripcion));
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Save()
        {
            if(!_view.Classification.descripcion.isValid())
            {
                _view.ShowError("La clasificación no es válida");
                return;
            }

            try
            {
                _classifications.Add(_view.Classification);
                _view.ShowMessage(string.Format("Clasificación {0} agregada exitosamente", _view.Classification.descripcion));
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
                _view.ShowError("No existe clasificación seleccionada para eliminar");
                return;
            }

            if (!_classifications.CanDelete(_view.Classification))
            {
                _view.ShowError("Para eliminar esta clasificación debe eliminarla de sus artículos primero");
                return;
            }

            try
            {
                _classifications.Delete(_view.Classification);

                _view.ShowMessage(string.Format("Clasificación {0} removida exitosamente", _view.Classification.descripcion));
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

        private void Find()
        {
            if (!_view.Classification.isValid())
                return;

            if (!_view.Classification.descripcion.isValid())
                return;

            try
            {
                var classification = _classifications.Find(_view.Classification.descripcion);

                if (!classification.isValid())
                    classification = new Clasificacione() { descripcion = _view.Classification.descripcion };

                _view.Show(classification);

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
