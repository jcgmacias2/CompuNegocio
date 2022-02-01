using System.Collections.Generic;
using System.Linq;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMTraspaso : Traspaso
    {
        public SolicitudesDeTraspaso TransferRequest { get; set; }

        public decimal Total { get; set; }
        public List<DetallesDeTraspaso> Detalle { get; set; }

        public VMTraspaso()
        {
            
        }

        public VMTraspaso(Traspaso transfer)
        {
            this.DetallesDeTraspasoes = transfer.DetallesDeTraspasoes;
            this.Detalle = transfer.DetallesDeTraspasoes.Select(x => x).ToList();
            this.EmpresasAsociada = transfer.EmpresasAsociada;
            this.EmpresasAsociada1 = transfer.EmpresasAsociada1;
            this.EstatusDeTraspaso = transfer.EstatusDeTraspaso;
            this.Usuario = transfer.Usuario;

            this.descripcion = transfer.descripcion;
            this.fechaHora = transfer.fechaHora;
            this.fechaHoraRemoto = transfer.fechaHoraRemoto;
            this.folio = transfer.folio;
            this.tipoDeCambio = transfer.tipoDeCambio;
            this.idUsuarioRegistro = transfer.idUsuarioRegistro;
            this.idTraspaso = transfer.idTraspaso;
            this.idEstatusDeTraspaso = transfer.idEstatusDeTraspaso;
            this.folioRemoto = transfer.folioRemoto;
            this.idEmpresaAsociadaDestino = transfer.idEmpresaAsociadaDestino;
            this.idEmpresaAsociadaOrigen = transfer.idEmpresaAsociadaOrigen;
        }

        public Traspaso ToTraspaso()
        {
            Traspaso transfer = new Traspaso();

             transfer.DetallesDeTraspasoes = Detalle;
             transfer.EmpresasAsociada = this.EmpresasAsociada;
             transfer.EmpresasAsociada1 = this.EmpresasAsociada1;
             transfer.EstatusDeTraspaso = this.EstatusDeTraspaso;
             transfer.Usuario = this.Usuario;

             transfer.descripcion = this.descripcion;
             transfer.fechaHora = this.fechaHora;
             transfer.fechaHoraRemoto = this.fechaHoraRemoto;
             transfer.folio = this.folio;
             transfer.idUsuarioRegistro = this.idUsuarioRegistro;
             transfer.idTraspaso = this.idTraspaso;
             transfer.idEstatusDeTraspaso = this.idEstatusDeTraspaso;
             transfer.folioRemoto = this.folioRemoto;
             transfer.idEmpresaAsociadaDestino = this.idEmpresaAsociadaDestino;
             transfer.idEmpresaAsociadaOrigen = this.idEmpresaAsociadaOrigen;
            transfer.tipoDeCambio = this.tipoDeCambio;

            return transfer;
        }
    }
}