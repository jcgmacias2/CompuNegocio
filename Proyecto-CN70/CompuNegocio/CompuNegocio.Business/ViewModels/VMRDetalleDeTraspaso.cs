using System;
using System.Collections.Generic;
using System.Linq;
using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;

namespace Aprovi.Business.ViewModels
{
    public class VMRDetalleDeTraspaso
    {
        public decimal CantidadEnviada { get; set; }

        public decimal CantidadNoAceptada
        {
            get
            {
                return !CantidadAceptada.HasValue ? 0m :(CantidadEnviada - CantidadAceptada.Value);
            }
        }

        public decimal? CantidadAceptada { get; set; }
        public string CantidadAceptadaEnLetra { get; set; }
        public string CodigoArticulo { get; set; }
        public string DescripcionUnidad { get; set; }
        public string DescripcionArticulo { get; set; }
        public string MonedaEnLetra { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal Importe { get; set; }
        public string Pedimento { get; set; }
        public int idTraspaso { get; set; }
        public int idDetalleTraspaso { get; set; }
        public int idArticulo { get; set; }
        public int folio { get; set; }
        public Moneda Moneda { get; set; }
        public string FechaRecepcion { get; set; }
        public string Estatus { get; set; }

        public VMRDetalleDeTraspaso(DetallesDeTraspaso detalle)
        {
            this.idDetalleTraspaso = detalle.idDetalleDeTraspaso;
            this.idTraspaso = detalle.idTraspaso;
            this.CodigoArticulo = detalle.Articulo.codigo;
            this.CantidadEnviada = detalle.cantidadEnviada;
            this.CantidadAceptada = detalle.cantidadAceptada;
            this.CantidadAceptadaEnLetra = detalle.cantidadAceptada.HasValue?detalle.cantidadAceptada.Value.ToDecimalString():"";
            this.DescripcionUnidad = detalle.Articulo.UnidadesDeMedida.descripcion;
            this.DescripcionArticulo = detalle.Articulo.descripcion;
            this.CostoUnitario = detalle.costoUnitario;
            this.Importe = detalle.cantidadEnviada * detalle.costoUnitario;
            this.idArticulo = detalle.Articulo.idArticulo;
            this.folio = detalle.Traspaso.isValid() ? detalle.Traspaso.folio : 0;
            this.Moneda = detalle.Moneda;
            this.MonedaEnLetra = detalle.Moneda.isValid() ? detalle.Moneda.descripcion.Substring(0,1) : "";
            this.Estatus = detalle.Traspaso.EstatusDeTraspaso.descripcion;

            this.FechaRecepcion = detalle.Traspaso.EmpresasAsociada.idEmpresaLocal.HasValue ?
                detalle.Traspaso.fechaHora.ToString("dd/MM/yyyy") :
                (detalle.Traspaso.fechaHoraRemoto.HasValue ? detalle.Traspaso.fechaHoraRemoto.Value.ToString(
                    "dd/MM/yyyy") : "");
        }

        public VMRDetalleDeTraspaso(PedimentoPorDetalleDeTraspaso pedimento)
        {
            this.idDetalleTraspaso = pedimento.DetallesDeTraspaso.idDetalleDeTraspaso;
            this.idTraspaso = pedimento.DetallesDeTraspaso.idTraspaso;
            this.CodigoArticulo = pedimento.DetallesDeTraspaso.Articulo.codigo;
            this.CantidadEnviada = pedimento.DetallesDeTraspaso.cantidadEnviada;
            this.CantidadAceptada = pedimento.cantidad;
            this.CantidadAceptadaEnLetra = pedimento.cantidad.ToDecimalString();
            this.DescripcionUnidad = pedimento.DetallesDeTraspaso.Articulo.UnidadesDeMedida.descripcion;
            this.DescripcionArticulo = pedimento.DetallesDeTraspaso.Articulo.descripcion;
            this.CostoUnitario = pedimento.DetallesDeTraspaso.costoUnitario;
            this.Importe = pedimento.DetallesDeTraspaso.cantidadEnviada * pedimento.DetallesDeTraspaso.costoUnitario;
            this.idArticulo = pedimento.DetallesDeTraspaso.Articulo.idArticulo;
            this.folio = pedimento.DetallesDeTraspaso.Traspaso.isValid() ? pedimento.DetallesDeTraspaso.Traspaso.folio : 0;
            this.Moneda = pedimento.DetallesDeTraspaso.Moneda;
            this.MonedaEnLetra = pedimento.DetallesDeTraspaso.Moneda.isValid() ? pedimento.DetallesDeTraspaso.Moneda.descripcion.Substring(0, 1) : "";
            this.Pedimento = pedimento.Pedimento.ToPedimentText();
        }

        public VMRDetalleDeTraspaso()
        {

        }
    }
}