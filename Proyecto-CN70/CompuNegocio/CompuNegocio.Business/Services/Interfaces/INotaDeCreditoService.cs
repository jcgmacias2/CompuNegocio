using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface INotaDeCreditoService
    {
        /// <summary>
        /// Proporciona el siguiente folio de nota de credito según la serie
        /// </summary>
        /// <param name="serie">Serie sobre la que se desea obtener folio</param>
        /// <returns>Folio asignado</returns>
        int Next(string serie);

        /// <summary>
        /// Proporciona el último folio utilizado en notas de credito según la serie
        /// </summary>
        /// <param name="serie">Serie sobre la que se desea obtener folio</param>
        /// <returns>Folio utilizado</returns>
        int Last(string serie);

        /// <summary>
        /// Agrega una nota de credito a la colección de notas de credito existentes
        /// </summary>
        /// <param name="creditNote">Nota de credito</param>
        /// <returns>Nota de credito registrada</returns>
        NotasDeCredito Add(VMNotaDeCredito creditNote);

        /// <summary>
        /// Timbra una nota de credito, si es exitoso genera el codigo cbb
        /// </summary>
        /// <param name="creditNote">Nota de credito a timbrar</param>
        /// <returns>Nota de credito timbrada</returns>
        NotasDeCredito Stamp(VMNotaDeCredito creditNote);

        /// <summary>
        /// Busca una nota de credito a partir de su identificador numérico
        /// </summary>
        /// <param name="idCreditNote">Identificador numérico de la nota de credito</param>
        /// <returns>Nota de credito que corresponde al identificador</returns>
        NotasDeCredito Find(int idCreditNote);

        /// <summary>
        /// Busca una nota de credito a partir de su serie y folio
        /// </summary>
        /// <param name="serie">Serie de la nota de credito que se busca</param>
        /// <param name="folio">Folio de la nota de credito que se busca</param>
        /// <returns>Nota de credito que corresponda con la serie y folio</returns>
        NotasDeCredito Find(string serie, string folio);

        /// <summary>
        /// Cancela una nota de credito
        /// </summary>
        /// <param name="idCreditNote">Nota de credito a cancelar</param>
        /// <param name="reason">Motivo por el que se cancela la nota de credito</param>
        /// <returns>Nota de credito cancelada</returns>
        NotasDeCredito Cancel(int idCreditNote,string reason);

        /// <summary>
        /// Proporciona las notas de credito con el filtro especificado
        /// </summary>
        /// <param name="value">valor para el filtro</param>
        /// <returns></returns>
        List<VMNotaDeCredito> WithClientOrFolioLike(string value);

        /// <summary>
        /// Proporciona todas las notas de credito
        /// </summary>
        /// <returns></returns>
        List<VMNotaDeCredito> List();

        /// <summary>
        /// Enlista todas las notas de crédito registradas en el período indicado
        /// </summary>
        /// <param name="start">Inicio del período</param>
        /// <param name="end">Fin del período</param>
        /// <returns>Lista de notas de credito</returns>
        List<VMNotaDeCredito> ByPeriod(DateTime start, DateTime end);

        /// <summary>
        /// Enlista todas las notas de credito registradas al cliente especificado en el período señalado
        /// </summary>
        /// <param name="start">Inicio del período</param>
        /// <param name="end">Fin del período</param>
        /// <param name="customers">Cliente</param>
        /// <returns>Lista de notas de credito</returns>
        List<VMNotaDeCredito> ByPeriodAndCustomer(DateTime start, DateTime end, Cliente customer);

        /// <summary>
        /// Agrega una nota de credito para la factura proporcionada con los descuentos proporcionados
        /// </summary>
        /// <param name="invoice">Factura a la que se agregara la nota de crédito</param>
        /// <param name="discountNotes">Notas de descuento de las que se creara la nota de crédito</param>
        /// <returns>Nota de credito creada</returns>
        NotasDeCredito Add(Factura invoice, List<NotasDeDescuento> discountNotes);

        /// <summary>
        /// Obtiene las notas de credito para el reporte de estado de la empresa
        /// </summary>
        /// <param name="vm">objeto a llenar con los totales</param>
        /// <param name="startDate">Fecha de inicio para el reporte</param>
        /// <param name="endDate">Fecha de fin para el reporte</param>
        /// <returns></returns>
        VMEstadoDeLaEmpresa ListCreditNotesForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Valida si se puede crear una nota de crédito para la factura proporcionada
        /// </summary>
        /// <param name="invoice">Factura</param>
        /// <param name="creditNote">Nota de Credito</param>
        /// <returns></returns>
        bool CanCreateForInvoice(VMNotaDeCredito creditNote, Factura invoice);
    }
}
