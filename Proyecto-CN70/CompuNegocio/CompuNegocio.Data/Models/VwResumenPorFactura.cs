//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Aprovi.Data.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class VwResumenPorFactura
    {
        public int idFactura { get; set; }
        public string serie { get; set; }
        public int folio { get; set; }
        public int idCliente { get; set; }
        public Nullable<int> idTipoRelacion { get; set; }
        public Nullable<int> idComprobanteOriginal { get; set; }
        public Nullable<int> FacturaSustituta { get; set; }
        public string codigo { get; set; }
        public string nombreComercial { get; set; }
        public string razonSocial { get; set; }
        public int idMoneda { get; set; }
        public string moneda { get; set; }
        public System.DateTime fechaHora { get; set; }
        public int idEstatusDeFactura { get; set; }
        public string estatus { get; set; }
        public decimal tipoDeCambio { get; set; }
        public Nullable<decimal> subtotal { get; set; }
        public Nullable<decimal> impuestos { get; set; }
        public Nullable<decimal> abonado { get; set; }
        public int idEstatusCrediticio { get; set; }
    }
}
