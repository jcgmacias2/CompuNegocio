using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class EstacionService : IEstacionService
    {
        private IUnitOfWork _UOW;
        private IEstacionesRepository _stations;
        private ILicenciaService _licenses;
        private IAplicacionesRepository _app;

        public EstacionService(IUnitOfWork unitOfWork, ILicenciaService licenses)
        {
            _UOW = unitOfWork;
            _stations = _UOW.Estaciones;
            _licenses = licenses; // new LicenciaService(unitOfWork);
            _app = _UOW.Aplicaciones;
        }

        public Estacione Add(Estacione station)
        {
            try
            {
                //Valido que la licencia permite agregar esta estacion
                var maxStations = _licenses.IncludedStations(station.Empresa.licencia);
                var currentStations = _stations.List(station.idEmpresa).Count;

                if (currentStations >= maxStations)
                    throw new Exception("Ha llegado al número máximo de estaciones incluidas en la licencia");

                //Si llega aquí entonces la agrego
                if (!station.Bascula.isValid() || !station.Bascula.puerto.isValid())
                    station.Bascula = new Bascula() { puerto = "N/A", finDeLinea = "." };
                _stations.Add(station);

                _UOW.Save();

                return station;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Estacione Find(string description)
        {
            try
            {
                return _stations.Find(description);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Estacione Find(int idStation)
        {
            try
            {
                return _stations.Find(idStation);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Estacione> List()
        {
            try
            {
                return _stations.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Estacione> List(int idRegister)
        {
            try
            {
                return _stations.List(idRegister);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Estacione Update(Estacione station, bool canChangeBusiness)
        {
            try
            {
                var local = _stations.Find(station.idEstacion);
                if(canChangeBusiness)
                    local.idEmpresa = station.idEmpresa;

                //Actualizo la información de la báscula
                if (!local.Bascula.isValid())
                    local.Bascula = new Bascula() { idBasculaEstacion = station.idEstacion };
                local.Bascula.puerto = station.Bascula.puerto;
                local.Bascula.velocidad = station.Bascula.velocidad;
                local.Bascula.bitsDeDatos = station.Bascula.bitsDeDatos;
                local.Bascula.bitsDeParada = station.Bascula.bitsDeParada;
                local.Bascula.finDeLinea = station.Bascula.finDeLinea;
                local.Bascula.paridad = station.Bascula.paridad;
                local.Bascula.tiempoDeEscritura = station.Bascula.tiempoDeEscritura;
                local.Bascula.tiempoDeLectura = station.Bascula.tiempoDeLectura;


                //Agrego o actualizo las impresoras
                foreach (var p in station.ImpresorasPorEstacions)
                {
                    var printer = local.ImpresorasPorEstacions.FirstOrDefault(i => i.idTipoDeImpresora.Equals(p.idTipoDeImpresora));

                    if (printer == null)
                        local.ImpresorasPorEstacions.Add(p);
                    else
                        printer.impresora = p.impresora;
                }

                _UOW.Save();

                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Estacione AssociateStation(Estacione station)
        {
            try
            {
                var computerCode = _app.GetComputerCode();
                //Debo buscar si el equipo esta relacionado con alguna otra estación
                var previousStation = _stations.HasStation(computerCode);

                //Si esta relacionado eliminar esa relación
                if (previousStation.isValid())
                    previousStation.equipo = null;

                //Establecer relación con esta nueva estación
                var newStation = _stations.Find(station.idEstacion);
                newStation.equipo = computerCode;

                //Actualizo en el archivo de aplicación local la nueva estación a la cual estará relacionado el equipo
                _app.UpdateSetting("Estacion", station.idEstacion.ToString());

                //Guardo los cambios
                _UOW.Save();

                return newStation;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Estacione DissociateStation(Estacione station)
        {
            try
            {
                var local = _stations.Find(station.idEstacion);
                local.equipo = null;
                _stations.Update(local);

                //Actualizo en el archivo de aplicación local la nueva estación a la cual estará relacionado el equipo
                _app.UpdateSetting("Estacion", "0");

                //Guardo los cambios
                _UOW.Save();

                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Estacione GetAssociatedStation()
        {
            try
            {
                //Obtengo el codigo de la computadora
                var computerCode = _app.GetComputerCode();
                //Debo buscar si el equipo esta relacionado con alguna estación
                return _stations.HasStation(computerCode);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Estacione> WithDescriptionLike(string description)
        {
            try
            {
                return _stations.WithDescriptionLike(description);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Remove(Estacione station)
        {
            try
            {
                _stations.Remove(station.idEstacion);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
