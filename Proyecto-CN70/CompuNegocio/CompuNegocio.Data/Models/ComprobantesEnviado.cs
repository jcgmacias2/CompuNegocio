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
    
    public partial class ComprobantesEnviado
    {
        public int idComprobanteEnviado { get; set; }
        public Nullable<System.DateTime> fechaHora { get; set; }
        public bool pdf { get; set; }
        public bool xml { get; set; }
        public int idTipoDeComprobante { get; set; }
        public string serie { get; set; }
        public string folio { get; set; }
    
        public virtual TiposDeComprobante TiposDeComprobante { get; set; }
    }
}
