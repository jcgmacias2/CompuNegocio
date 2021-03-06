using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMPagoMultiple
    {
        public VMPagoMultiple() { }

        /// <summary>
        /// Inicializa un Pago múltiple con los valores base y default
        /// </summary>
        /// <param name="cliente">Cliente al que corresponde el pago</param>
        /// <param name="saldos">Saldos al dia de documentos del cliente</param>
        /// <param name="serie">Serie asignada al pago</param>
        /// <param name="folio">Folio de la serie asignado al pago</param>
        /// <param name="tipo">Tipo de cambio al dia</param>
        public VMPagoMultiple(Cliente cliente, List<VMFacturaConSaldo> saldos, string serie, int folio, decimal tipo)
        {
            IdPago = -1;
            FechaHora = DateTime.Now;
            Serie = serie;
            Folio = folio;
            TipoDeCambio = tipo;
            Cliente = cliente;
            IdCliente = -1;
            IdMetodoDePago = -1;
            IdEstatusDePago = (int)StatusDePago.Nuevo;
            AbonosDeFacturas = new List<AbonosDeFactura>();
            FacturasConSaldo = saldos;
            TotalAbonadoPesos = 0.0m;
            TotalAbonadoDolares = 0.0m;
        }

        /// <summary>
        /// Inicializa un Pago múltiple que incluye la información del pago y los comprobantes abonados
        /// </summary>
        /// <param name="pago">Transacción del pago</param>
        /// <param name="saldos">Saldos actualizados al dia sobre los documentos que fueron abonados</param>
        public VMPagoMultiple(Pago pago, List<VMFacturaConSaldo> saldos)
        {
            IdPago = pago.idPago;
            FechaHora = pago.fechaHora;
            Serie = pago.serie;
            Folio = pago.folio;
            TipoDeCambio = pago.tipoDeCambio;
            IdCliente = pago.idCliente;
            Cliente = pago.Cliente;
            IdUsuario = pago.idUsuario;
            Usuario = pago.Usuario;
            IdMetodoDePago = pago.idMetodoDePago;
            MetodosPago = pago.MetodosPago;
            IdEstatusDePago = pago.idEstatusDePago;
            EstatusDePago = pago.EstatusDePago;
            IdEmpresa = pago.idEmpresa;
            Empresa = pago.Empresa;
            IdRegimen = pago.idRegimen;
            Regimene = pago.Regimene;
            IdUsoCFDI = pago.idUsoCFDI;
            UsosCFDI = pago.UsosCFDI;
            cadenaOriginal = pago.cadenaOriginal;
            TimbresDePago = pago.TimbresDePago;

            //Si es un pago existente debo mostrar las facturas con saldo registradas
            FacturasConSaldo = saldos;

            this.UpdateAccount();
        }

        public Pago ToPago()
        {
            var pago = new Pago();
            pago.idPago = this.IdPago;
            pago.fechaHora = this.FechaHora;
            pago.serie = this.Serie;
            pago.folio = this.Folio;
            pago.tipoDeCambio = this.TipoDeCambio;
            pago.idCliente = this.IdCliente;
            pago.idUsuario = this.IdUsuario;
            pago.idMetodoDePago = this.IdMetodoDePago;
            pago.idEstatusDePago = this.IdEstatusDePago;
            pago.idEmpresa = this.IdEmpresa;
            pago.idRegimen = this.IdRegimen;
            pago.idUsoCFDI = this.IdUsoCFDI;
            pago.cadenaOriginal = this.cadenaOriginal;
            pago.Cliente = this.Cliente;
            pago.Empresa = this.Empresa;
            pago.EstatusDePago = this.EstatusDePago;
            pago.MetodosPago = this.MetodosPago;
            pago.Regimene = this.Regimene;
            pago.UsosCFDI = this.UsosCFDI;
            pago.Usuario = this.Usuario;
            pago.TimbresDePago = this.TimbresDePago;

            //Si es un pago ya registrado, ya tiene los abonos de factura
            if(this.IdPago.isValid())
            {
                pago.AbonosDeFacturas = this.AbonosDeFacturas;
            }
            else //De lo contrario es agregarselos
            {
                pago.AbonosDeFacturas = new List<AbonosDeFactura>();
                foreach (var a in FacturasConSaldo.Where(f => f.Abono.isValid() && f.Abono.monto.isValid()))
                {
                    pago.AbonosDeFacturas.Add(a.Abono);
                }
            }

            return pago;
        }

        public int IdPago { get; set; }
        public DateTime FechaHora { get; set; }
        public string Serie { get; set; }
        public int Folio { get; set; }
        public decimal TipoDeCambio { get; set; }
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public int IdMetodoDePago { get; set; }
        public int IdEstatusDePago { get; set; }
        public int IdEmpresa { get; set; }
        public int IdRegimen { get; set; }
        public int IdUsoCFDI { get; set; }
        public string cadenaOriginal { get; set; }

        public List<AbonosDeFactura> AbonosDeFacturas { get; set; }
        public Cliente Cliente { get; set; }
        public Empresa Empresa { get; set; }
        public EstatusDePago EstatusDePago { get; set; }
        public MetodosPago MetodosPago { get; set; }
        public Regimene Regimene { get; set; }
        public UsosCFDI UsosCFDI { get; set; }
        public Usuario Usuario { get; set; }
        public TimbresDePago TimbresDePago { get; set; }

        public List<VMFacturaConSaldo> FacturasConSaldo { get; set; }
        public decimal TotalAbonadoPesos { get; set; }
        public decimal TotalAbonadoDolares { get; set; }

    }
}
