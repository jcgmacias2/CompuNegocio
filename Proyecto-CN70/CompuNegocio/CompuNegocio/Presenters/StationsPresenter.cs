using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Aprovi.Presenters
{
    public class StationsPresenter
    {
        private IStationsView _view;
        private IEstacionService _stations;
        private IEmpresaService _businesses;
        private ICatalogosEstaticosService _catalogs;
        private IConfiguracionService _config;
        private IDispositivoService _devices;

        public StationsPresenter(IStationsView view, IEstacionService stationsService, IEmpresaService businessesService, ICatalogosEstaticosService catalogsService, IConfiguracionService configService, IDispositivoService devicesService)
        {
            _view = view;
            _stations = stationsService;
            _businesses = businessesService;
            _catalogs = catalogsService;
            _config = configService;
            _devices = devicesService;

            _view.Find += Find;
            _view.OpenList += OpenList;
            _view.Quit += Quit;
            _view.New += New;
            _view.Delete += Delete;
            _view.Save += Save;
            _view.Update += Update;
            _view.AssociateStation += AssociateStation;
            _view.DissociateStation += DissociateStation;
            _view.ListPorts += ListPorts;

            _view.FillCombos(_catalogs.ListImpresoras(), _businesses.List(), _devices.ListStopBits(), _devices.ListParities());
        }

        private void ListPorts()
        {
            try
            {
                var ports = _devices.ListPorts();
                ports.Insert(0, "TCP");

                _view.FillPorts(ports);
                _view.ShowMessage("Seleccione el puerto a utilizar");
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void DissociateStation()
        {
            try
            {
                _stations.DissociateStation(_view.Station);

                _view.ShowMessage("La relación con el equipo ha sido eliminada");

                //Refrescar la info
                Session.Configuration = _config.GetDefault();

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void AssociateStation()
        {
            try
            {
                if(_view.IsStationSet)
                {
                    if(!_view.Station.idEstacion.isValid())
                    {
                        _view.ShowError("No existe ninguna estación seleccionada para relacionar");
                        return;
                    }
                }

                //Reviso que la estación a la que me quiero enlazar, no este relacionada a otro equipo
                if(_view.Station.equipo.isValid())
                {
                    _view.ShowError("Esta estación ya se encuentra relacionada a un equipo, por lo que no puede ejecutarse esta acción");
                    return;
                }

                //Reviso si este equipo ya estaba enlazado a alguna
                if(Session.Station.isValid() && Session.Station.idEstacion.isValid())
                {
                    //En este caso le mando un mensaje al usuario 
                    if (!_view.ShowMessageWithOptions(string.Format("Este equipo ya se encuentra relacionado a la estación {0}, seguro que desea relacionarlo a la estación {1}", Session.Station.descripcion, _view.Station.descripcion)).Equals(MessageBoxResult.Yes))
                        return;
                }

                //Si ya confirmo el cambio, hay que hacerlo
                var station = _stations.AssociateStation(_view.Station);

                //Actualizo la configuración en sesión
                Session.Configuration = _config.GetDefault();

                //Envio mensaje al usuario
                _view.ShowMessage("Este equipo ahora se encuentra relacionado con la estación {0}", _view.Station.descripcion);

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
                var station = _view.Station;

                //Si no tiene estación válida 
                var fullChange = Session.Station.isValid();

                //Hago el cambio total o parcial dependiendo de si tiene o no ventas pendientes de facturar
                _stations.Update(station, fullChange);

                //Si los cambios son en la estación asociada hago la actualización de la configuración también
                if (_view.IsStationSet)
                {
                    Session.Configuration.Escaner = _view.Configuration.Escaner;
                    Session.Configuration.CodigoEscaner = _view.Configuration.CodigoEscaner;
                    Session.Configuration = _config.Update(Session.Configuration);
                }

                _view.ShowMessage("La estación fué actualizada exitosamente");

                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Save()
        {
            var station = _view.Station;

            if (!station.descripcion.isValid())
            {
                _view.ShowError("Debe capturar la descripción de la empresa");
                return;
            }

            if (!station.idEmpresa.isValid())
            {
                _view.ShowError("Debe especificar a que empresa pertenece esta estación");
                return;
            }

            if(station.ImpresorasPorEstacions.Any(i => !i.impresora.isValid()))
            {
                _view.ShowError("Debe especificar las impresoras que utilizará");
                return;
            }

            try
            {
                _stations.Add(station);

                _view.ShowMessage("La estación {0} ha sido agregada exitosamente", station.descripcion);

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
                _view.ShowError("No hay ningúna estación seleccionada para eliminar");
                return;
            }

            try
            {
                //Si tiene ventas pendientes de facturar no puede eliminarse
                //Llamo el registro original, porque los datos de la vista pueden estar modificador
                var station = _stations.Find(_view.Station.idEstacion);


                //Si la estación esta relacionada con un equipo distinto al que intenta realizar la eliminación, no puede eliminarse
                var currentStation = _stations.GetAssociatedStation();
                if(currentStation.isValid() && station.equipo.isValid() && !station.idEstacion.Equals(currentStation.idEstacion))
                {
                    _view.ShowError("La estación que desea eliminar esta asociada con otro equipo, no puede eliminarse en este momento");
                    return;
                }

                //Si la estación a eliminar esta relacionada con este equipo, primero elimino la asociación
                if(currentStation.isValid() && currentStation.idEstacion.Equals(station.idEstacion))
                {
                    _stations.DissociateStation(station);
                }

                //Ahora si elimino la estación
                _stations.Remove(station);


                //Mensaje al usuario
                _view.ShowMessage("Estación removida exitosamente");

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

        private void OpenList()
        {
            try
            {
                IStationsListView view;
                StationsListPresenter presenter;

                view = new StationsListView();
                presenter = new StationsListPresenter(view, _stations);

                view.ShowWindow();

                if (view.Station.idEstacion.isValid())
                {
                    var configuration = Session.Station.isValid() && view.Station.idEstacion.Equals(Session.Station.idEstacion) ? Session.Configuration : new Configuracion();
                    _view.Show(view.Station, configuration);
                }
                    
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            if (!_view.Station.descripcion.isValid())
                return;

            try
            {
                var station = _stations.Find(_view.Station.descripcion);

                if (station == null)
                    station = new Estacione() { descripcion = _view.Station.descripcion, Bascula = new Bascula() };

                var configuration = station.idEstacion.isValid() && Session.Station.isValid() && station.idEstacion.Equals(Session.Station.idEstacion) ? Session.Configuration : new Configuracion();

                _view.Show(station, configuration);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
