using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IAbonoDeRemisionService
    {
        /// <summary>
        /// Registra un nuevo abono a la remisión
        /// </summary>
        /// <param name="payment">Abono a registrar</param>
        /// <returns>Abono registrado</returns>
        AbonosDeRemision Add(AbonosDeRemision payment);


        /// <summary>
        /// Busca un abono a partir de su identificador numérico
        /// </summary>
        /// <param name="idPayment">Identificador numérico del abono</param>
        /// <returns>Abono asociado al identificador</returns>
        AbonosDeRemision Find(int idPayment);

        /// <summary>
        /// Busca un abono a partir de su folio
        /// </summary>
        /// <param name="folio">Folio asociado al abono</param>
        /// <returns>Abono al que corresponde el folio</returns>
        AbonosDeRemision Find(string folio);


        /// <summary>
        /// Provee una lista de los abonos de una remisión determinada
        /// </summary>
        /// <param name="idBillOfLanding">Identificador numérico de la remisión</param>
        /// <returns>Lista de abonos de la remisión</returns>
        List<AbonosDeRemision> List(int idBillOfLanding);

        /// <summary>
        /// Provee una lista de abonos en la que coincida el folio o la razon social con el valor que se pasa
        /// </summary>
        /// <param name="value">Valor a buscar en coincidencia</param>
        /// <returns>Lista de parcialidades que coinciden</returns>
        List<AbonosDeRemision> ListAbonosLike(string value);

        /// <summary>
        /// Provee el siguiente folio único
        /// </summary>
        /// <returns>Siguiente folio consecutivo</returns>
        string GetNextFolio();

        /// <summary>
        /// Cancela un abono registrado
        /// </summary>
        /// <param name="idPayment">Identificador del abono de la remisión a cancelar</param>
        /// <returns>Abono cancelado</returns>
        AbonosDeRemision Cancel(int idPayment);
    }
}
