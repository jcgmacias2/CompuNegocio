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
    
    public partial class ImpresorasPorEstacion
    {
        public int idEstacion { get; set; }
        public int idTipoDeImpresora { get; set; }
        public string impresora { get; set; }
    
        public virtual Estacione Estacione { get; set; }
        public virtual TiposDeImpresora TiposDeImpresora { get; set; }
    }
}
