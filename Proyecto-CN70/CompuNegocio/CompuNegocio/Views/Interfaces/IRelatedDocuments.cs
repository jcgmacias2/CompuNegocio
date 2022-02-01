using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IRelatedDocuments
    {
        event Action FindInvoice;
        event Action OpenInvoicesList;

        Factura RelatedInvoice { get; }

        void Show(Factura related);
    }
}
