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
    
    public partial class PedimentoPorDetalleDeRemision
    {
        public int idPedimento { get; set; }
        public int idDetalleDeRemision { get; set; }
        public decimal cantidad { get; set; }
    
        public virtual DetallesDeRemision DetallesDeRemision { get; set; }
        public virtual Pedimento Pedimento { get; set; }
    }
}
