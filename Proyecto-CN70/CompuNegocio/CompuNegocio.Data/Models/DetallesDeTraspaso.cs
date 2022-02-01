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
    
    public partial class DetallesDeTraspaso
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DetallesDeTraspaso()
        {
            this.PedimentoPorDetalleDeTraspasoes = new HashSet<PedimentoPorDetalleDeTraspaso>();
        }
    
        public int idDetalleDeTraspaso { get; set; }
        public int idTraspaso { get; set; }
        public int idArticulo { get; set; }
        public decimal cantidadEnviada { get; set; }
        public Nullable<decimal> cantidadAceptada { get; set; }
        public decimal costoUnitario { get; set; }
        public int idMoneda { get; set; }
    
        public virtual Articulo Articulo { get; set; }
        public virtual Moneda Moneda { get; set; }
        public virtual Traspaso Traspaso { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PedimentoPorDetalleDeTraspaso> PedimentoPorDetalleDeTraspasoes { get; set; }
    }
}
