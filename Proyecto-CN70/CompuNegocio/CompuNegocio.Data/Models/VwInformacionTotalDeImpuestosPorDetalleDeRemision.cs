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
    
    public partial class VwInformacionTotalDeImpuestosPorDetalleDeRemision
    {
        public int idDetalleDeRemision { get; set; }
        public int idImpuesto { get; set; }
        public string nombre { get; set; }
        public decimal valor { get; set; }
        public int idTipoDeImpuesto { get; set; }
        public int idRemision { get; set; }
        public decimal cantidad { get; set; }
        public Nullable<decimal> precioUnitario { get; set; }
        public int idMoneda { get; set; }
        public decimal tipoDeCambio { get; set; }
    }
}
