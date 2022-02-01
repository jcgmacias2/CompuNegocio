using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IPagoService
    {
        /// <summary>
        /// Proporciona el siguiente folio de pago según la serie
        /// </summary>
        /// <param name="serie">Serie sobre la que se desea obtener folio</param>
        /// <returns>Folio asignado</returns>
        int Next(string serie);

        /// <summary>
        /// Proporciona el último folio utilizado en pagos según la serie
        /// </summary>
        /// <param name="serie">Serie sobre la que se desea obtener folio</param>
        /// <returns>Folio utilizado</returns>
        int Last(string serie);

        /// <summary>
        /// Agrega un pago a la colección de pagos existentes
        /// </summary>
        /// <param name="payment">Pago a registrar</param>
        /// <returns>Pago registrado</returns>
        Pago Add(Pago payment);

        /// <summary>
        /// Timbra un pago, si es exitoso genera el codigo cbb
        /// </summary>
        /// <param name="payment">Pago a timbrar</param>
        /// <returns>Pago timbrado</returns>
        Pago Stamp(Pago payment);

        /// <summary>
        /// Busca un pago a partir de su identificador numérico
        /// </summary>
        /// <param name="idPayment">Identificador numérico del pago</param>
        /// <returns>Pago que corresponde al identificador</returns>
        Pago Find(int idPayment);

        /// <summary>
        /// Busca un pago a partir de su serie y folio
        /// </summary>
        /// <param name="serie">Serie del pago que se busca</param>
        /// <param name="folio">Folio del pago que se busca</param>
        /// <returns>Pago que corresponda con la serie y folio</returns>
        Pago Find(string serie, string folio);

        /// <summary>
        /// Enlista todos los pagos existentes
        /// </summary>
        /// <returns>Lista de pagos</returns>
        List<VwListaPago> List();

        /// <summary>
        /// Enlista todos los pagos que coinciden total o parcialmente en su folio, o razón social del cliente, con el valor que se especifíca
        /// </summary>
        /// <param name="value">Valor a buscar en coincidencia</param>
        /// <returns>Lista de pagos que coincidan con la búsqueda</returns>
        List<VwListaPago> WithClientOrFolioLike(string value);

        /// <summary>
        /// Cancela un pago y todos su abonos registrados
        /// </summary>
        /// <param name="idPayment">Identificador del pago a cancelar</param>
        /// <param name="reason">Motivo por el que se cancela el pago</param>
        /// <returns>Pago cancelado</returns>
        Pago Cancel(int idPayment, string reason);
    }
}
