using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface INotaDeDescuentoService
    {
        /// <summary>
        /// Proporciona el siguiente folio de nota de descuento
        /// </summary>
        /// <returns>Folio asignado</returns>
        int Next();

        /// <summary>
        /// Proporciona el último folio utilizado en notas de descuento
        /// </summary>
        /// <returns>Folio utilizado</returns>
        int Last();

        /// <summary>
        /// Actualiza una nota de descuento
        /// </summary>
        /// <param name="discountNote">Nota de descuento a modificar</param>
        /// <returns>Nota de descuento modificada</returns>
        NotasDeDescuento Update(NotasDeDescuento discountNote);

        /// <summary>
        /// Agrega una nota de descuento a la colección de notas de descuento existentes
        /// </summary>
        /// <param name="disccountNote">Nota de descuento</param>
        /// <returns>Nota de descuento registrada</returns>
        NotasDeDescuento Add(NotasDeDescuento disccountNote);

        /// <summary>
        /// Busca una nota de descuento a partir de su identificador numérico
        /// </summary>
        /// <param name="idDisccountNote">Identificador numérico de la nota de descuento</param>
        /// <returns>Nota de descuento que corresponde al identificador</returns>
        NotasDeDescuento Find(int idDisccountNote);

        /// <summary>
        /// Busca una nota de descuento a partir de su folio
        /// </summary>
        /// <param name="folio">Folio de la nota de debito que se busca</param>
        /// <returns>Nota de debito que corresponda con el folio</returns>
        NotasDeDescuento FindByFolio(int folio);

        /// <summary>
        /// Cancela una nota de descuento
        /// </summary>
        /// <param name="idDisccountNote">Nota de descuento a cancelar</param>
        /// <param name="reason">Motivo por el que se cancela la nota de descuento</param>
        /// <returns>Nota de descuento cancelada</returns>
        NotasDeDescuento Cancel(int idDisccountNote,string reason);

        /// <summary>
        /// Proporciona las notas de credito con el filtro especificado
        /// </summary>
        /// <param name="value">valor para el filtro</param>
        /// <returns></returns>
        List<NotasDeDescuento> WithClientOrFolioLike(string value);

        /// <summary>
        /// Proporciona todas las notas de credito
        /// </summary>
        /// <returns></returns>
        List<NotasDeDescuento> List();

        /// <summary>
        /// Obtiene las notas de descuento para el reporte de estado de la empresa
        /// </summary>
        /// <param name="vm">objeto a llenar con los totales</param>
        /// <param name="startDate">Fecha de inicio para el reporte</param>
        /// <param name="endDate">Fecha de fin para el reporte</param>
        /// <returns></returns>
        VMEstadoDeLaEmpresa ListDiscountNotesForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Obtiene el reporte de notas de descuento
        /// </summary>
        /// <param name="customer">Cliente para el filtro</param>
        /// <param name="startDate">Fecha de inicio para el filtro</param>
        /// <param name="endDate">Fecha de fin para el filtro</param>
        /// <param name="includeOnlyPending">true si se deben reportear solo notas de descuento sin aplicar</param>
        /// <param name="includeOnlyApplied">true si se deben reportear solo notas de descuento aplicadas</param>
        /// <returns></returns>
        VMReporteNotasDeDescuento ListDisccountNotesForReport(Cliente customer, DateTime startDate, DateTime endDate, bool includeOnlyPending, bool includeOnlyApplied);
    }
}
