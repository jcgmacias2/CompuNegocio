using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IAbonoDeFacturaService
    {
        /// <summary>
        /// Registra un nuevo abono a la venta
        /// </summary>
        /// <param name="payment">Abono a registrar</param>
        /// <returns>Abono registrado</returns>
        AbonosDeFactura Add(AbonosDeFactura payment);

        /// <summary>
        /// Timbra abono existente como Comprobante de pago
        /// </summary>
        /// <param name="invoice">Factura a la cual pertenece el abono</param>
        /// <param name="payment">Abono a timbrar</param>
        /// <returns>Abono registrado</returns>
        AbonosDeFactura Stamp(VMFactura invoice, AbonosDeFactura payment);

        ///// <summary>
        ///// Timbra abonos existentes como Comprobante de pago
        ///// </summary>
        ///// <param name="invoice">Facturas a la cuales pertenecen los abonos</param>
        ///// <param name="payment">Abonos a timbrar</param>
        ///// <returns>Abonos registrados</returns>
        //List<AbonosDeFactura> Stamp(List<VMFactura> invoices, List<AbonosDeFactura> payments);

        /// <summary>
        /// Busca un abono a partir de su identificador numérico
        /// </summary>
        /// <param name="idPayment">Identificador numérico del abono</param>
        /// <returns>Abono asociado al identificador</returns>
        AbonosDeFactura Find(int idPayment);

        /// <summary>
        /// Busca un abono a partir de su folio
        /// </summary>
        /// <param name="folio">Folio asociado al abono</param>
        /// <returns>Abono al que corresponde el folio</returns>
        AbonosDeFactura Find(string folio);

        /// <summary>
        /// Busca un abono/parcialidad a partir de su serie y folio
        /// </summary>
        /// <param name="serie">Serie de la parcialidad a buscar</param>
        /// <param name="folio">Folio de la parcialidad a buscar</param>
        /// <returns>Parcialidad</returns>
        AbonosDeFactura Find(string serie, int folio);

        /// <summary>
        /// Provee una lista de los abonos de una compra determinada
        /// </summary>
        /// <param name="idInvoice">Identificador numérico de la factura</param>
        /// <returns>Lista de abonos de la factura</returns>
        List<AbonosDeFactura> List(int idInvoice);

        /// <summary>
        /// Provee una lista de abonos que sean parcialidades fiscales
        /// </summary>
        /// <returns>Lista de parcialidades</returns>
        List<VwListaParcialidade> ListParcialidades();

        /// <summary>
        /// Provee una lista de abonos en la que coincida el folio y la serie o la razon social con el valor que se pasa
        /// </summary>
        /// <param name="value">Valor a buscar en coincidencia</param>
        /// <returns>Lista de parcialidades que coinciden</returns>
        List<VwListaParcialidade> ListParcialidadesLike(string value);

        AbonosDeFactura LastParcialidad(string serie);

        /// <summary>
        /// Provee el siguiente folio único
        /// </summary>
        /// <returns>Siguiente folio consecutivo</returns>
        string GetNextFolio();

        /// <summary>
        /// Cancela un abono registrado
        /// </summary>
        /// <param name="idPayment">Identificador del abono de la factura a cancelar</param>
        /// <returns>Abono cancelado</returns>
        AbonosDeFactura Cancel(int idPayment);

        List<VMRDetalleAbonosFacturas> GetPaymentsForReport(List<AbonosDeFactura> payments);

        VwListaParcialidade FindVMBySerieAndFolio(string serie, int folio);
    }
}
