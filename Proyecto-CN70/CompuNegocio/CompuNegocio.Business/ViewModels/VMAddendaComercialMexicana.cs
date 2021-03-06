using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    [XmlRoot(ElementName = "Orden")]
    public class VMAddendaComercialMexicana
    {
        //Nota: El formato de la fecha es AAAAMMDD segun el documento de AMC
        public string FechaEntrega { get; set; }
        //Serie+Folio
        public string NumeroFactura { get; set; }

        public string NumeroOrden { get; set; }
        public string Seccion { get; set; }

        public string GlnDirectorio { get; set; }
        public string NombreDirectorio { get; set; }
        public string DireccionDirectorio { get; set; }
        public string CiudadDirectorio { get; set; }
        public string CodigoPostalDirectorio { get; set; }

        public List<VMComercialMexicanaItem> Items { get; set; }

        public string Total { get; set; }
        public string SubTotal { get; set; }

        public string TasaIVA { get; set; }
        public string TotalIVA { get; set; }

        public string TasaIEPS { get; set; }
        public string TotalIEPS { get; set; }

        public string TotalAPagar { get; set; }

        public VMAddendaComercialMexicana()
        {

        }

        public VMAddendaComercialMexicana(VMFactura invoice, List<DatosExtraPorFactura> datos, Directorio directorio,Seccione seccion)
        {
            //Se obtienen los datos extras solicitados al crear la factura
            DatosExtraPorFactura datoFecha = datos.FindDatoOrDefault(DatoExtra.FechaDeEntrega);

            //Se convierte la fecha al formato solicitado por AMECE
            DateTime date;
            string addendaDate = "";

            if (DateTime.TryParseExact(datoFecha.valor, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out date))
            {
                FechaEntrega = date.ToString("yyyyMMdd");
            }
            else
            {
                FechaEntrega = "";
            }

            NumeroFactura = string.Format("{0}{1}", invoice.serie, invoice.folio);
            NumeroOrden = invoice.ordenDeCompra;

            if (directorio.isValid())
            {
                GlnDirectorio = directorio.gln;
                NombreDirectorio = directorio.nombre;
                DireccionDirectorio = directorio.direccion;
                CiudadDirectorio = directorio.ciudad;
                CodigoPostalDirectorio = directorio.codigoPostal;
            }

            Total = invoice.Total.ToDecimalString();
            SubTotal = invoice.Subtotal.ToDecimalString();

            //Se agrega el detalle de la factura
            Items = new List<VMComercialMexicanaItem>();
            for (int i = 1; i <= invoice.DetalleDeFactura.Count; i++)
            {
                VMComercialMexicanaItem item = new VMComercialMexicanaItem(new VMDetalle(invoice.DetalleDeFactura.ElementAt(i - 1)), i);
                Items.Add(item);
            }

            //Se obtienen los impuestos que aplican a la factura
            VMImpuesto iva = invoice.Impuestos.FirstOrDefault(x => x.codigo == ((int)Impuestos.IVA).ToString("000") && x.idTipoDeImpuesto == (int)TipoDeImpuesto.Trasladado);
            VMImpuesto ieps = invoice.Impuestos.FirstOrDefault(x => x.codigo == ((int)Impuestos.IEPS).ToString("000") && x.idTipoDeImpuesto == (int)TipoDeImpuesto.Trasladado);

            TasaIVA = iva.isValid() ? iva.valor.ToDecimalString() : "0.00";
            TotalIVA = iva.isValid() ? iva.Importe.ToDecimalString() : "0.00";

            TasaIEPS = ieps.isValid() ? ieps.valor.ToDecimalString() : "0.00";
            TotalIEPS = ieps.isValid() ? ieps.Importe.ToDecimalString() : "0.00";

            TotalAPagar = invoice.Saldo.ToDecimalString();

            if (seccion.isValid())
            {
                Seccion = seccion.nombre;
            }
        }
    }

    [XmlRoot(ElementName = "Item")]
    [XmlType("Item")]
    public class VMComercialMexicanaItem
    {
        public string NumeroLinea { get; set; }
        public string Gtin { get; set; }
        public string Descripcion { get; set; }

        //Nota: Ver EDIFACT Anexo A AMECE
        public string UnidadMedida { get; set; }

        public string Cantidad { get; set; }
        //Precio del producto
        public string PrecioBruto { get; set; }
        //Precio del producto mas impuestos
        public string PrecioNeto { get; set; }
        //Cantidad * Precio
        public string ImporteNeto { get; set; }
        //(Cantidad * Precio)+Impuestos
        public string ImporteBruto { get; set; }

        public VMComercialMexicanaItem()
        {

        }

        public VMComercialMexicanaItem(VMDetalle detalle, int numeroLinea)
        {
            NumeroLinea = numeroLinea.ToString();
            Gtin = detalle.CodigoArticulo;
            Descripcion = detalle.DescripcionArticulo;

            UnidadMedida = detalle.ClaveUnidad;

            Cantidad = detalle.Cantidad.ToDecimalString();

            PrecioBruto = detalle.PrecioUnitario.ToDecimalString();

            //Se calcula el IVA e IEPS de 1 unidad
            VMImpuesto iva = detalle.Impuestos.FirstOrDefault(x => x.codigo == ((int)Impuestos.IVA).ToString("000") && x.idTipoDeImpuesto == (int)TipoDeImpuesto.Trasladado);
            VMImpuesto ieps = detalle.Impuestos.FirstOrDefault(x => x.codigo == ((int)Impuestos.IEPS).ToString("000") && x.idTipoDeImpuesto == (int)TipoDeImpuesto.Trasladado);

            decimal tasaIva = iva.isValid() ? iva.valor : 0m;
            decimal tasaIeps = ieps.isValid() ? ieps.valor : 0m;
            PrecioNeto = (detalle.PrecioUnitario + ((detalle.PrecioUnitario / 100) * tasaIva) + ((detalle.PrecioUnitario / 100) * tasaIeps)).ToDecimalString();

            //Se calcula el IVA e IEPS de 1 fila
            ImporteBruto = detalle.Importe.ToDecimalString();
            ImporteNeto = (detalle.Importe + (iva.isValid() ? iva.Importe : 0m) + (ieps.isValid() ? ieps.Importe : 0m)).ToDecimalString();
        }

    }
}