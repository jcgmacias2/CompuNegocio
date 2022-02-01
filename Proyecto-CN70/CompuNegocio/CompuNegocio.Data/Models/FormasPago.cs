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
    
    public partial class FormasPago
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FormasPago()
        {
            this.AbonosDeCompras = new HashSet<AbonosDeCompra>();
            this.AbonosDeFacturas = new HashSet<AbonosDeFactura>();
            this.AbonosDeRemisions = new HashSet<AbonosDeRemision>();
            this.NotasDeCreditoes = new HashSet<NotasDeCredito>();
        }
    
        public int idFormaPago { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public bool bancarizado { get; set; }
        public string patron { get; set; }
        public bool activa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AbonosDeCompra> AbonosDeCompras { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AbonosDeFactura> AbonosDeFacturas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AbonosDeRemision> AbonosDeRemisions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NotasDeCredito> NotasDeCreditoes { get; set; }
    }
}
