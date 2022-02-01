using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMRTraspaso : VMTraspaso
    {
        public VMRTraspaso() : base() { }

        public VMRTraspaso(VMTraspaso transfer): base()
        {
            //Generales
            this.FolioTraspaso = transfer.folio.ToString();
            this.FechaTraspaso = transfer.fechaHora.ToUTCFormat();
            this.fechaHora = transfer.fechaHora;
            this.Total = transfer.Total;
            this.Descripcion = transfer.descripcion;
            this.Usuario = transfer.Usuario.isValid() ? transfer.Usuario.nombreDeUsuario : "";
            
            //Detalles
            this.DetallesDeTraspasoes = transfer.DetallesDeTraspasoes;

            this.EmpresaAsociadaDestino = transfer.EmpresasAsociada.nombre;
            this.EmpresaAsociadaOrigen = transfer.EmpresasAsociada1.nombre;
        }

        #region Generales

        public string FolioTraspaso { get; set; }
        public string FechaTraspaso { get; set; }
        public string Estatus { get; set; }
        public string Descripcion { get; set; }
        public string Usuario { get; set; }
        public string EmpresaAsociadaOrigen { get; set; }
        public string EmpresaAsociadaDestino { get; set; }

        #endregion

        #region Cuenta
        public decimal Total { get; set; }

        #endregion
    }
}
