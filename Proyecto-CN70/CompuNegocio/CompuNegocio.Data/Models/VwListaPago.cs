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
    
    public partial class VwListaPago
    {
        public int idPago { get; set; }
        public string serie { get; set; }
        public int folio { get; set; }
        public int idCliente { get; set; }
        public string codigo { get; set; }
        public string nombreComercial { get; set; }
        public string razonSocial { get; set; }
        public System.DateTime fechaHora { get; set; }
        public int idEstatusDePago { get; set; }
        public string estatus { get; set; }
        public decimal tipoDeCambio { get; set; }
        public decimal totalPesos { get; set; }
        public decimal totalDolares { get; set; }
        public string fechaCancelacion { get; set; }
    }
}
