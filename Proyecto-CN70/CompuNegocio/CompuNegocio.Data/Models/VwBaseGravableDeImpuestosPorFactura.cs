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
    
    public partial class VwBaseGravableDeImpuestosPorFactura
    {
        public int idFactura { get; set; }
        public int folio { get; set; }
        public System.DateTime fechaHora { get; set; }
        public int idCliente { get; set; }
        public int idMoneda { get; set; }
        public int idEstatusDeFactura { get; set; }
        public decimal tipoDeCambio { get; set; }
        public int idImpuesto { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public decimal valor { get; set; }
        public bool activo { get; set; }
        public int idTipoDeImpuesto { get; set; }
        public int idTipoFactor { get; set; }
        public Nullable<decimal> baseGravable { get; set; }
    }
}
