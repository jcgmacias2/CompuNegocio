using System;
using System.Collections.Generic;
using System.Linq;


namespace Aprovi.Data.Models
{
    public class Configuracion : Configuracione
    {
        public Configuracion() : base() { }

        public Configuracion(Configuracione config) : base()
        {
            this.Certificados = config.Certificados;
            this.Domicilio = config.Domicilio;
            this.idConfiguracion = config.idConfiguracion;
            this.idDomicilio = config.idDomicilio;
            this.razonSocial = config.razonSocial;
            this.Regimenes = config.Regimenes;
            this.rfc = config.rfc;
            this.Series = config.Series;
            this.tipoDeCambio = config.tipoDeCambio;
            this.Estacion = new Estacione();
            this.usuarioPAC = config.usuarioPAC;
            this.contraseñaPAC = config.contraseñaPAC;
            this.Sistema = Customization.Default;
            this.Modulos = new List<Modulos>();
            this.CuentasGuardians = config.CuentasGuardians;
            this.telefono = config.telefono;
            this.idOpcionCostoDisminuye = config.idOpcionCostoDisminuye;
            this.idOpcionCostoAumenta = config.idOpcionCostoAumenta;
            this.FormatosPorConfiguracions = config.FormatosPorConfiguracions;
            this.idPeriodicidad = config.Periodicidad.idPeriodicidad;
            this.Periodicidad = config.Periodicidad;
        }

        public Configuracion(Configuracione config, Estacione station, bool useScanner, string scannerCode, bool useDrawer) : base()
        {
            this.Certificados = config.Certificados;
            this.Domicilio = config.Domicilio;
            this.idConfiguracion = config.idConfiguracion;
            this.idDomicilio = config.idDomicilio;
            this.razonSocial = config.razonSocial;
            this.Regimenes = config.Regimenes;
            this.rfc = config.rfc;
            this.Series = config.Series;
            this.tipoDeCambio = config.tipoDeCambio;
            this.Estacion = station;
            this.usuarioPAC = config.usuarioPAC;
            this.contraseñaPAC = config.contraseñaPAC;
            this.Escaner = useScanner;
            this.CodigoEscaner = scannerCode;
            this.CajonDeEfectivo = useDrawer;
            this.Sistema = Customization.Default;
            this.Modulos = new List<Modulos>();
            this.CuentasGuardians = config.CuentasGuardians;
            this.telefono = config.telefono;
            this.idOpcionCostoDisminuye = config.idOpcionCostoDisminuye;
            this.idOpcionCostoAumenta = config.idOpcionCostoAumenta;
            this.FormatosPorConfiguracions = config.FormatosPorConfiguracions;
            this.idPeriodicidad = config.Periodicidad.idPeriodicidad;
            this.Periodicidad = config.Periodicidad;
        }

        public Estacione Estacion { get; set; }

        public string CarpetaReportes { get; set; }

        public Uri Logo { get; set; }

        public string CarpetaXml { get; set; }

        public string CarpetaPdf { get; set; }

        public string CarpetaCbb { get; set; }

        public Ambiente Mode { get; set; }

        public string Regimen { get { return this.Regimenes.First().descripcion; } }

        public bool Escaner { get; set; }

        public string CodigoEscaner { get; set; }

        public bool CajonDeEfectivo { get; set; }

        public List<Modulos> Modulos { get; set; }

        public Customization Sistema { get; set; }
    }
}
