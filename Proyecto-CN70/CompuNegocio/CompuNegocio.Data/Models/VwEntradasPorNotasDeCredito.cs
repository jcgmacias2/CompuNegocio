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
    
    public partial class VwEntradasPorNotasDeCredito
    {
        public int idArticulo { get; set; }
        public int idDetalleDeNotaDeCredito { get; set; }
        public decimal Unidades { get; set; }
        public System.DateTime fechaHora { get; set; }
        public int idNotaDeCredito { get; set; }
        public string folio { get; set; }
        public string razonSocial { get; set; }
        public string factura { get; set; }
    }
}
