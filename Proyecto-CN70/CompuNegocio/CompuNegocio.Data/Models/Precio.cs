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
    
    public partial class Precio
    {
        public int idListaDePrecio { get; set; }
        public int idArticulo { get; set; }
        public decimal utilidad { get; set; }
    
        public virtual ListasDePrecio ListasDePrecio { get; set; }
        public virtual Articulo Articulo { get; set; }
    }
}
