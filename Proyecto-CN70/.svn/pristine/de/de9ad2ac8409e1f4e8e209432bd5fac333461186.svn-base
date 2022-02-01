using System;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMOperacion
    {
        public string Tipo { get; set; }
        public string Folio { get; set; }
        public DateTime FechaHora { get; set; }

        public VMOperacion()
        {
            
        }

        public VMOperacion(Factura invoice)
        {
            Tipo = "Factura";
            Folio = string.Format("{0}{1}", invoice.serie, invoice.folio);
            FechaHora = invoice.fechaHora;
        }

        public VMOperacion(Remisione billOfSale)
        {
            Tipo = "Remision";
            Folio = billOfSale.folio.ToString();
            FechaHora = billOfSale.fechaHora;
        }

        public VMOperacion(Compra purchase)
        {
            Tipo = "Compra";
            Folio = purchase.folio.ToString();
            FechaHora = purchase.fechaHora;
        }
    }
}