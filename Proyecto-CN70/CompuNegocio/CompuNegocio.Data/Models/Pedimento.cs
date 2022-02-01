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
    
    public partial class Pedimento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pedimento()
        {
            this.PedimentoPorDetalleDeAjustes = new HashSet<PedimentoPorDetalleDeAjuste>();
            this.PedimentoPorDetalleDeFacturas = new HashSet<PedimentoPorDetalleDeFactura>();
            this.PedimentoPorDetalleDeRemisions = new HashSet<PedimentoPorDetalleDeRemision>();
            this.Ajustes = new HashSet<Ajuste>();
            this.PedimentoPorDetalleDeTraspasoes = new HashSet<PedimentoPorDetalleDeTraspaso>();
            this.Compras = new HashSet<Compra>();
            this.PedimentoPorDetalleDeNotaDeCreditoes = new HashSet<PedimentoPorDetalleDeNotaDeCredito>();
        }
    
        public int idPedimento { get; set; }
        public string añoOperacion { get; set; }
        public string aduana { get; set; }
        public string patente { get; set; }
        public string añoEnCurso { get; set; }
        public string progresivo { get; set; }
        public System.DateTime fecha { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PedimentoPorDetalleDeAjuste> PedimentoPorDetalleDeAjustes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PedimentoPorDetalleDeFactura> PedimentoPorDetalleDeFacturas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PedimentoPorDetalleDeRemision> PedimentoPorDetalleDeRemisions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ajuste> Ajustes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PedimentoPorDetalleDeTraspaso> PedimentoPorDetalleDeTraspasoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Compra> Compras { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PedimentoPorDetalleDeNotaDeCredito> PedimentoPorDetalleDeNotaDeCreditoes { get; set; }
    }
}
