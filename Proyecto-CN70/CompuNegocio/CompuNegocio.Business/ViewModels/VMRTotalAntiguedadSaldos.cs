using System;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMRTotalAntiguedadSaldos
    {
        public int idCliente { get; set; }
        public string CodigoCliente { get; set; }
        public string RazonSocial { get; set; }
        public string Moneda { get; set; }
        public decimal Importe { get; set; }
        public decimal Abonado { get; set; }
        public decimal Saldo { get; set; }
        public int Tiempo { get; set; }
        public DateTime Fecha { get; set; }

        public VMRTotalAntiguedadSaldos(VwReporteAntiguedadSaldosFactura detail, DateTime to)
        {
            idCliente = detail.idCliente;
            RazonSocial = detail.razonSocial;
            CodigoCliente = detail.codigo;
            Moneda = detail.moneda;
            //Cuando una factura ha sido sustituida el comprobante original debe "saldarse" con el importe abonado
            if (detail.FacturaSustituta.HasValue)
                Importe = detail.abonado.Value;
            else
                Importe = detail.subtotal.Value + detail.impuestos.Value;

            Abonado = detail.abonado.GetValueOrDefault(0);
            Saldo = Importe - Abonado;
            Fecha = detail.fechaHora;
            Tiempo = (to - detail.fechaHora).Days;
        }

        public VMRTotalAntiguedadSaldos(VwReporteAntiguedadSaldosRemisione detail, DateTime to)
        {
            idCliente = detail.idCliente;
            RazonSocial = detail.razonSocial;
            CodigoCliente = detail.codigo;
            Importe = (detail.subtotal + detail.impuestos).GetValueOrDefault(0m);
            Moneda = detail.moneda;
            Saldo = (detail.subtotal + detail.impuestos - detail.abonado).GetValueOrDefault(0m);
            Abonado = detail.abonado.GetValueOrDefault(0);
            Fecha = detail.fechaHora;
            Tiempo = (to - detail.fechaHora).Days;
        }
    }
}