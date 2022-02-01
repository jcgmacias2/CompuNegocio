using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Repositories;

namespace Aprovi.Business.ViewModels
{
    public class VMImpuesto : Impuesto
    {
        public VMImpuesto() : base() { }

        public VMImpuesto(Impuesto impuesto)
        {
            this.activo = impuesto.activo;
            this.codigo = impuesto.codigo;
            this.idImpuesto = impuesto.idImpuesto;
            this.idTipoDeImpuesto = impuesto.idTipoDeImpuesto;
            this.idTipoFactor = impuesto.idTipoFactor;
            this.nombre = impuesto.nombre;
            this.valor = impuesto.valor;
            this.TiposDeImpuesto = impuesto.TiposDeImpuesto;
            this.TiposFactor = impuesto.TiposFactor;
            this.MontoGravable = 0.0m;
        }

        public VMImpuesto(Impuesto impuesto, decimal montoGravable)
        {
            this.activo = impuesto.activo;
            this.codigo = impuesto.codigo;
            this.idImpuesto = impuesto.idImpuesto;
            this.idTipoDeImpuesto = impuesto.idTipoDeImpuesto;
            this.idTipoFactor = impuesto.idTipoFactor;
            this.nombre = impuesto.nombre;
            this.valor = impuesto.valor;
            this.TiposDeImpuesto = impuesto.TiposDeImpuesto;
            this.TiposFactor = impuesto.TiposFactor;
            this.MontoGravable = montoGravable;
        }

        /// <summary>
        /// Constructor que generá una instancia dummy para mostrar el total de los impuestos
        /// </summary>
        /// <param name="taxTotal">Importe total de los impuestos</param>
        public VMImpuesto(decimal taxTotal)
        {
            this.activo = true;
            this.idImpuesto = 0;
            this.idTipoDeImpuesto = 0;
            this.idTipoFactor = 0;
            this.nombre = "Impuestos";
            this.valor = 100.0m;
            this.MontoGravable = taxTotal;
        }

        public Impuesto ToImpuesto(IImpuestosRepository impuestosRepository)
        {
            return impuestosRepository.Find(this.idImpuesto);
        }

        /// <summary>
        /// Importe sobre el que se grava el impuesto
        /// </summary>
        public decimal MontoGravable { get; set; }

        /// <summary>
        /// Importe resultante de aplicar la tasa del impuesto sobre el importe gravable
        /// </summary>
        public decimal Importe { get { return (valor / 100.0m) * MontoGravable; } }

        /// <summary>
        /// Descripción a mostrar del impuesto en los detalles de las operaciones para un IVA Trasladado = +IVA, para uno retenido = -IVA
        /// Cuando el id del tipo de impuesto es 0 no lleva ningún signo porque es el totalizado, es un dummy
        /// </summary>
        public string Descripcion { get { return string.Format("{0}{1}", this.idTipoDeImpuesto.Equals(0) ? string.Empty : this.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado) ? "+" : "-", this.nombre); } }
        
        /// <summary>
        /// Se muestra en formato para impresión ej: 002-IVA $2.46
        /// </summary>
        public string Impresion { get { return string.Format("{0}-{1} ${2}", this.codigo, this.nombre, this.Importe.ToDecimalString()); } }
    }
}
