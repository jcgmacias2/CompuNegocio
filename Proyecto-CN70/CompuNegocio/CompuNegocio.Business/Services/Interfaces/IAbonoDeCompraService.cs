using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IAbonoDeCompraService
    {
        /// <summary>
        /// Registra un nuevo abono a la compra
        /// </summary>
        /// <param name="payment">Abono a registrar</param>
        /// <returns>Abono registrado</returns>
        AbonosDeCompra Add(AbonosDeCompra payment);

        /// <summary>
        /// Busca un abono a partir de su identificador numérico
        /// </summary>
        /// <param name="idPayment">Identificador numérico del abono</param>
        /// <returns>Abono asociado al identificador</returns>
        AbonosDeCompra Find(int idPayment);

        /// <summary>
        /// Busca un abono a partir de su folio
        /// </summary>
        /// <param name="folio">Folio asociado al abono</param>
        /// <returns>Abono al que corresponde el folio</returns>
        AbonosDeCompra Find(string folio);

        /// <summary>
        /// Provee una lista de los abonos de una compra determinada
        /// </summary>
        /// <param name="idPurchase">Identificador numérico de la compra</param>
        /// <returns>Lista de abonos de la compra</returns>
        List<AbonosDeCompra> List(int idPurchase);

        /// <summary>
        /// Provee el siguiente folio único
        /// </summary>
        /// <returns>Siguiente folio consecutivo</returns>
        string GetNextFolio();

        /// <summary>
        /// Cancela un abono registrado
        /// </summary>
        /// <param name="payment">Abono a cancelar (con el id es suficiente)</param>
        /// <returns>Abono cancelado</returns>
        AbonosDeCompra Cancel(AbonosDeCompra payment);
    }
}
