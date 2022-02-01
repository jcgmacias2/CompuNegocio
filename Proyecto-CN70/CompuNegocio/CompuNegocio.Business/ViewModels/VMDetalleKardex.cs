using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMDetalleKardex
    {
        public VMDetalleKardex(VwEntradasPorCompra movimiento)
        {
            fechaHora = movimiento.fechaHora;
            Movimiento = string.Format("Compra - {0}", movimiento.razonSocial);
            Folio = movimiento.folio;
            Entrada = movimiento.Unidades.GetValueOrDefault();
            Salida = 0.0m;
        }

        public VMDetalleKardex(VwSalidasPorComprasCancelada movimiento)
        {
            fechaHora = movimiento.fechaCancelacion.GetValueOrDefault();
            Movimiento = string.Format("Cancelación de compra - {0}", movimiento.razonSocial);
            Folio = movimiento.folio;
            Entrada = 0.0m;
            Salida = movimiento.Unidades.GetValueOrDefault();
        }

        public VMDetalleKardex(VwEntradasPorAjuste movimiento)
        {
            fechaHora = movimiento.fechaHora;
            Movimiento = string.Format("Ajuste de entrada - {0}", movimiento.descripcion);
            Folio = movimiento.folio;
            Entrada = movimiento.Unidades;
            Salida = 0.0m;
        }

        public VMDetalleKardex(VwSalidasPorAjustesCancelado movimiento)
        {
            fechaHora = movimiento.fechaCancelacion;
            Movimiento = string.Format("Cancelación de ajuste de entrada - {0}", movimiento.descripcion);
            Folio = movimiento.folio;
            Entrada = 0.0m;
            Salida = movimiento.Unidades.GetValueOrDefault();
        }

        public VMDetalleKardex(VwSalidasPorAjuste movimiento)
        {
            fechaHora = movimiento.fechaHora;
            Movimiento = string.Format("Ajuste de salida - {0}", movimiento.descripcion);
            Folio = movimiento.folio;
            Entrada = 0.0m;
            Salida = movimiento.Unidades;
        }

        public VMDetalleKardex(VwEntradasPorAjustesCancelado movimiento)
        {
            fechaHora = movimiento.fechaCancelacion.GetValueOrDefault();
            Movimiento = string.Format("Cancelación de ajuste de salida - {0}", movimiento.descripcion);
            Folio = movimiento.folio;
            Entrada = movimiento.Unidades;
            Salida = 0.0m;
        }

        public VMDetalleKardex(VwSalidasPorFactura movimiento)
        {
            fechaHora = movimiento.fechaHora;
            Movimiento = string.Format("Factura - {0}", movimiento.razonSocial);
            Folio = movimiento.folio.ToUpper();
            Entrada = 0.0m;
            Salida = movimiento.Unidades;
        }

        public VMDetalleKardex(VwEntradasPorFacturasCancelada movimiento)
        {
            if (!movimiento.fechaCancelacion.HasValue)
            {
                Movimiento = string.Format("Anulación de factura - {0}", movimiento.razonSocial);
                fechaHora = movimiento.fechaHora;
            }
            else
            {
                Movimiento = string.Format("Cancelación de factura - {0}", movimiento.razonSocial);
                fechaHora = movimiento.fechaCancelacion.GetValueOrDefault();
            }
            Folio = movimiento.folio.ToUpper();
            Entrada = movimiento.Unidades;
            Salida = 0.0m;
        }

        public VMDetalleKardex(VwSalidasPorRemisione movimiento)
        {
            fechaHora = movimiento.fechaHora;
            Movimiento = string.Format("Remisión - {0}", movimiento.razonSocial);
            Folio = movimiento.folio.ToString();
            Entrada = 0.0m;
            Salida = movimiento.Unidades;
        }

        public VMDetalleKardex(VwEntradasPorRemisionesCancelada movimiento)
        {
            fechaHora = movimiento.fechaCancelacion.GetValueOrDefault();
            Movimiento = string.Format("Cancelación de remisión - {0}", movimiento.razonSocial);
            Folio = movimiento.folio.ToString();
            Entrada = movimiento.Unidades;
            Salida = 0.0m;
        }

        public VMDetalleKardex(VwSalidasPorTraspaso movimiento)
        {
            fechaHora = movimiento.fechaHora;
            Movimiento = string.Format("Traspaso de Salida - {0}", movimiento.descripcion);
            Folio = movimiento.folio.ToString();
            Entrada = 0.0m;
            Salida = movimiento.Unidades;
        }

        public VMDetalleKardex(VwEntradasPorTraspaso movimiento)
        {
            fechaHora = movimiento.fechaHora;
            Movimiento = string.Format("Traspaso de Entrada - {0}", movimiento.descripcion);
            Folio = movimiento.folio.ToString();
            Salida = 0.0m;
            Entrada = movimiento.Unidades;
        }

        public VMDetalleKardex(VwSalidasPorNotasDeCreditoCancelada movimiento)
        {
            fechaHora = movimiento.fechaHora;
            Movimiento = string.Format("Cancelación de nota de crédito - {0}", movimiento.razonSocial);
            Folio = movimiento.folio.ToString();
            Entrada = 0.0m;
            Salida = movimiento.Unidades;
        }

        public VMDetalleKardex(VwEntradasPorNotasDeCredito movimiento)
        {
            fechaHora = movimiento.fechaHora;
            Movimiento = string.Format("Nota de crédito - {0}", movimiento.razonSocial);
            Folio = movimiento.folio.ToString();
            Entrada = movimiento.Unidades;
            Salida = 0.0m;
        }

        public DateTime fechaHora { get; set; }
        public string Movimiento { get; set; }
        public string Folio { get; set; }
        public decimal Entrada { get; set; }
        public decimal Salida { get; set; }
    }
}
