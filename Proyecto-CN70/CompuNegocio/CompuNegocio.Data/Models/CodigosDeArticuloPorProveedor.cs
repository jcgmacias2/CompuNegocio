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
    
    public partial class CodigosDeArticuloPorProveedor
    {
        public int idCodigoDeArticuloPorProveedor { get; set; }
        public int idArticulo { get; set; }
        public Nullable<int> idProveedor { get; set; }
        public string codigo { get; set; }
    
        public virtual Articulo Articulo { get; set; }
        public virtual Proveedore Proveedore { get; set; }
    }
}
