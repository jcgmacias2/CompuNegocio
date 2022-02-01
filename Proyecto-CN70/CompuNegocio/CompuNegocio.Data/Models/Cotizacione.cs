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
    
    public partial class Cotizacione
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cotizacione()
        {
            this.DetallesDeCotizacions = new HashSet<DetallesDeCotizacion>();
            this.DatosExtraPorCotizacions = new HashSet<DatosExtraPorCotizacion>();
        }
    
        public int idCotizacion { get; set; }
        public int folio { get; set; }
        public System.DateTime fechaHora { get; set; }
        public decimal tipoDeCambio { get; set; }
        public string numeroDePedido { get; set; }
        public string comentario { get; set; }
        public int idMoneda { get; set; }
        public int idCliente { get; set; }
        public int idUsuarioRegistro { get; set; }
        public int idEstatusDeCotizacion { get; set; }
        public int idEmpresa { get; set; }
        public Nullable<int> idFactura { get; set; }
        public Nullable<int> idRemision { get; set; }
    
        public virtual CancelacionesDeCotizacione CancelacionesDeCotizacione { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual EstatusDeCotizacion EstatusDeCotizacion { get; set; }
        public virtual Moneda Moneda { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetallesDeCotizacion> DetallesDeCotizacions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DatosExtraPorCotizacion> DatosExtraPorCotizacions { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Factura Factura { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Remisione Remisione { get; set; }
    }
}
