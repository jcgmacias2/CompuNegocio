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
    
    public partial class ComisionesPorUsuario
    {
        public int idUsuario { get; set; }
        public int idTipoDeComision { get; set; }
        public decimal valor { get; set; }
    
        public virtual TiposDeComision TiposDeComision { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
