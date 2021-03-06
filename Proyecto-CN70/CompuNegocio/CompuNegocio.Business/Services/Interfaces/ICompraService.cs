using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Aprovi.Business.Services
{
    public interface ICompraService
    {
        /// <summary>
        /// Agrega una nueva compra a la lista de compras
        /// </summary>
        /// <param name="purchase">Compra a agregar</param>
        /// <returns>Compra registrada</returns>
        Compra Add(Compra purchase);

        /// <summary>
        /// Busca una compra activa a partir del id del proveedor y el folio de la compra
        /// </summary>
        /// <param name="idSupplier">Identificador del proveedor al que se le realizó la compra</param>
        /// <param name="folio">Folio del proveedor asignado a la compra</param>
        /// <returns>Compra registrada</returns>
        Compra Find(int idSupplier, string folio);

        /// <summary>
        /// Enlista todas las compras donde el folio o el código del proveedor tengan similitud con el parametro de busqueda
        /// </summary>
        /// <param name="search">Parametro de busqueda entre el folio y el código del proveedor</param>
        /// <returns>Lista de Compras que cumplen con el criterio</returns>
        List<Compra> WithFolioOrSupplierLike(string search);

        /// <summary>
        /// Enlista todas las compras registradas 
        /// </summary>
        /// <returns>Lista de Compras</returns>
        List<Compra> List();

        /// <summary>
        /// Enlista todas las compras registradas en el período indicado
        /// </summary>
        /// <param name="start">Inicio del período</param>
        /// <param name="end">Fin del período</param>
        /// <returns>Lista de compras</returns>
        List<VMCompra> ByPeriod(DateTime start, DateTime end);

        /// <summary>
        /// Enlista todas las compras registradas al proveedor especificado en el período señalado
        /// </summary>
        /// <param name="start">Inicio del período</param>
        /// <param name="end">Fin del período</param>
        /// <param name="supplier">Proveedor</param>
        /// <returns>Lista de compras</returns>
        List<VMCompra> ByPeriodAndSupplier(DateTime start, DateTime end, Proveedore supplier);

        /// <summary>
        /// Cancela una compra, incluidos sus abonos
        /// </summary>
        /// <param name="purchase">Compra a cancelar</param>
        /// <param name="reason">Motivo de la cancelacion</param>
        /// <returns>Compra cancelada</returns>
        Compra Cancel(Compra purchase, string reason);

        /// <summary>
        /// Provee la ultima compra registrada al proveedor especificado
        /// </summary>
        /// <param name="supplier">Proveedor sobre el cual filtrar las compras</param>
        /// <returns>Ultima compra registrada</returns>
        Compra Last(Proveedore supplier);

        /// <summary>
        /// Provee la compra que corresponde con el identificador numerico unico
        /// </summary>
        /// <param name="idPurchase">Identificador numerico unico</param>
        /// <returns>Compra correspondiente</returns>
        Compra Find(int idPurchase);

        /// <summary>
        /// Provee una lista de detalles de compra para la orden de compra especificada
        /// </summary>
        /// <param name="purchaseOrder">orden de compra a buscar</param>
        /// <returns>lista de detalles de compra relacionados con la orden de compra</returns>
        List<DetallesDeCompra> Find(OrdenesDeCompra purchaseOrder);

        /// <summary>
        /// Genera una compra en base al CFDI Proporcionado
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        Compra Import(Compra compra, XmlDocument doc);

        /// <summary>
        /// Obtiene las compras para el reporte de estado de la empresa
        /// </summary>
        /// <param name="vm">objeto a llenar con los totales</param>
        /// <param name="startDate">Fecha de inicio para el reporte</param>
        /// <param name="endDate">Fecha de fin para el reporte</param>
        /// <returns></returns>
        VMEstadoDeLaEmpresa ListPurchasesForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Obtiene las compras por pagar para el reporte de estado de la empresa
        /// </summary>
        /// <param name="vm">objeto a llenar con los totales</param>
        /// <param name="startDate">Fecha de inicio para el reporte</param>
        /// <param name="endDate">Fecha de fin para el reporte</param>
        /// <returns></returns>
        VMEstadoDeLaEmpresa ListPayableBalancesForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate);
    }
}
