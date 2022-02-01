using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IOrdenDeCompraService
    {
        /// <summary>
        /// Proporciona el siguiente folio de orden de compra
        /// </summary>
        /// <returns>Folio asignado</returns>
        int Next();

        /// <summary>
        /// Proporciona el último folio utilizado en ordenes de compra
        /// </summary>
        /// <returns>Folio utilizado</returns>
        int Last();

        /// <summary>
        /// Agrega una orden de compra a la colección de ordenes de compra existentes
        /// </summary>
        /// <param name="order">Orden de compra a registrar</param>
        /// <returns>Orden de compra registrada</returns>
        OrdenesDeCompra Add(VMOrdenDeCompra order);

        /// <summary>
        /// Busca una orden de compra a partir de su identificador numérico y proveedor
        /// </summary>
        /// <param name="idOrder">Identificador numérico de la orden de compra</param>
        /// <param name="idProveedor">Identificador numérico del proveedor</param>
        /// <returns>Orden de compra que corresponde al identificador y proveedor</returns>
        OrdenesDeCompra Find(int idOrder, int idProveedor);

        /// <summary>
        /// Busca una orden de compra a partir de su identificador numérico
        /// </summary>
        /// <param name="idOrder">Identificador numérico de la orden de compra</param>
        /// <returns>Orden de compra que corresponde al identificador</returns>
        OrdenesDeCompra Find(int idOrder);

        /// <summary>
        /// Busca una orden de compra a partir de su  folio
        /// </summary>
        /// <param name="folio">Folio de la orden de compra que se busca</param>
        /// <returns>Orden de compra que corresponda con él folio</returns>
        OrdenesDeCompra FindByFolio(int folio);

        /// <summary>
        /// Enlista todas las ordenes de compra existentes
        /// </summary>
        /// <returns>Lista de ordenes de compra</returns>
        List<OrdenesDeCompra> List();

        /// <summary>
        /// Enlista todas las ordenes de compra que coinciden total o parcialmente en su folio, o razón social del proveedor, con el valor que se especifíca
        /// </summary>
        /// <param name="value">Valor a buscar en coincidencia</param>
        /// <returns>Lista de ordenes de compra que coincidan con la búsqueda</returns>
        List<OrdenesDeCompra> WithFolioOrProviderLike(string value);

        /// <summary>
        /// Cancela una orden de compra
        /// </summary>
        /// <param name="idOrder">Identificador de la orden de compra a cancelar</param>
        /// <param name="reason">Motivo por el que se cancela la orden de compra</param>
        /// <returns>Orden de compra cancelada</returns>
        OrdenesDeCompra Cancel(int idOrder, string reason);

        /// <summary>
        /// Actualiza una orden de compra
        /// </summary>
        /// <param name="order">Orden de compra a actualizar</param>
        /// <returns>Orden de compra actualizada</returns>
        OrdenesDeCompra Update(OrdenesDeCompra order);

        /// <summary>
        /// Actualiza el detalle de una orden de compra
        /// </summary>
        /// <param name="order">Orden de compra a actualizar</param>
        /// <param name="detail">Detalle a actualizar</param>
        /// <returns>Orden de compra actualizada</returns>
        OrdenesDeCompra Update(OrdenesDeCompra order, List<VMDetalleDeOrdenDeCompra> detail);

        /// <summary>
        /// Proporciona la cantidad de articulos pendientes para la orden de compra proporcionada
        /// </summary>
        /// <param name="idOrdenDeCompra">Orden de compra en la que se verificaran los articulos pendientes</param>
        /// <param name="idArticulo">articulo a verificar</param>
        /// <returns></returns>
        decimal PendingUnitsForOrderItem(int idOrdenDeCompra, int idArticulo);
    }
}
