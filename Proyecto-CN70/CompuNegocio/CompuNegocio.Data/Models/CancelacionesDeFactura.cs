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
    
    public partial class CancelacionesDeFactura
    {
        public int idFactura { get; set; }
        public System.DateTime fechaHora { get; set; }
        public string motivo { get; set; }
    
        public virtual Factura Factura { get; set; }
    }
}
