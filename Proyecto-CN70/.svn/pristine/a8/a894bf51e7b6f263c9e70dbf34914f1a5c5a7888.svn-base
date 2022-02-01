using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Views;
using System;

namespace Aprovi.Presenters
{
    public class ScaleItemsTransferPresenter
    {
        private IScaleItemsTransferView _view;
        private IBasculaService _scale;
        private IClasificacionService _classifications;
        private IArticuloService _items;

        public ScaleItemsTransferPresenter(IScaleItemsTransferView view, IBasculaService scale, IClasificacionService classifications, IArticuloService items)
        {
            _view = view;
            _scale = scale;
            _classifications = classifications;
            _items = items;

            _view.Quit += Quit;
            _view.Transfer += Transfer;

            _view.FillCombo(_classifications.List());
        }

        private void Transfer()
        {
            if(!_view.Classification.idClasificacion.isValid())
            {
                _view.ShowMessage("Debe seleccionar la clasificación por la que se filtran los artículos a transferir");
                return;
            }

            if(!Session.Station.isValid())
            {
                _view.ShowError("Este equipo no se encuentra asociado a ninguna estación");
                return;
            }

            //if(!Session.Station.Bascula.isValid() || !Session.Station.Bascula.puerto.isValid())
            //{
            //    _view.ShowError("Este equipo no cuenta con una báscula configurada");
            //    return;
            //}

            try
            {
                var items = _items.List(_view.Classification);

                if(items.Count.Equals(0))   
                {
                    _view.ShowError("No existen artículos con la clasificación especificada");
                    return;
                }

                //if(!_scale.IsReady(Session.Station.Bascula))
                //{
                //    _view.ShowError("No fue posible establecer comunicación con la báscula, por favor revise la configuración");
                //    return;
                //}

                var file = _scale.WritePLUs(Session.Station.Bascula, items);

                _view.ShowMessage("{0} artículos transferidos", items.Count);
                _view.ShowMessage("Archivo creado en {0}", file);

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
    }
}
