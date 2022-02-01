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
    
    public partial class DetallesDeRemision
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DetallesDeRemision()
        {
            this.PedimentoPorDetalleDeRemisions = new HashSet<PedimentoPorDetalleDeRemision>();
            this.Impuestos = new HashSet<Impuesto>();
        }
    
        public int idDetalleDeRemision { get; set; }
        public int idArticulo { get; set; }
        public decimal cantidad { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal descuento { get; set; }
        public int idRemision { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PedimentoPorDetalleDeRemision> PedimentoPorDetalleDeRemisions { get; set; }
        public virtual ComentariosPorDetalleDeRemision ComentariosPorDetalleDeRemision { get; set; }
        public virtual Articulo Articulo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Impuesto> Impuestos { get; set; }
        public virtual Remisione Remisione { get; set; }
    }
}
