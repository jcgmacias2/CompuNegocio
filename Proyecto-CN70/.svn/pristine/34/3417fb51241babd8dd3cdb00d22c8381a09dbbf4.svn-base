using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Business.Services
{
    public interface IFormaPagoService
    {
        /// <summary>
        /// Provee el catálogo de formas de pago actual
        /// </summary>
        /// <returns>Lista de formas de pago</returns>
        List<FormasPago> List();

        /// <summary>
        /// Provee una lista de formas de pago que contienen la descripción que se busca
        /// </summary>
        /// <param name="description">Descripción de la forma de pago que se busca</param>
        /// <returns>Lista de formas de pago en coincidencia</returns>
        List<FormasPago> WithDescriptionLike(string description);

        /// <summary>
        /// Busca una forma de pago a través de su identificador
        /// </summary>
        /// <param name="idPaymentForm">Identificador numérico de la forma de pago</param>
        /// <returns></returns>
        FormasPago Find(int idPaymentForm);

        /// <summary>
        /// Busca un forma de pago a través de su descripción única
        /// </summary>
        /// <param name="description">Descripción exacta</param>
        /// <returns>Forma de pago al que pertenece la descripción</returns>
        FormasPago Find(string description);

        /// <summary>
        /// Inactiva una forma de pago del catálogo actual
        /// </summary>
        /// <param name="paymentForm">Forma de pago a inactivar</param>
        void Delete(FormasPago paymentForm);

        /// <summary>
        /// Reactiva una forma de pago del catálogo actual
        /// </summary>
        /// <param name="paymentForm">Forma de pago a activar</param>
        /// <returns>Forma de pago actualizada</returns>
        FormasPago Update(FormasPago paymentForm);
    }
}
