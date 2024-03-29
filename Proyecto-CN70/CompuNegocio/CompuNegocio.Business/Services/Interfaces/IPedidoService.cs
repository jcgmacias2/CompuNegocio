﻿using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IPedidoService
    {
        /// <summary>
        /// Proporciona el siguiente folio de pedido
        /// </summary>
        /// <returns>Folio asignado</returns>
        int Next();

        /// <summary>
        /// Proporciona el último folio utilizado en pedidos
        /// </summary>
        /// <returns>Folio utilizado</returns>
        int Last();

        /// <summary>
        /// Agrega un pedido a la colección de pedidos existentes
        /// </summary>
        /// <param name="order">Pedido a registrar</param>
        /// <returns>Pedido registrado</returns>
        Pedido Add(VMPedido order);

        /// <summary>
        /// Agrega un pedido a la coleccion de pedidos existentes (App)
        /// </summary>
        /// <param name="order">Pedido a registrar</param>
        /// <param name="station">Estacion desde la que se registra el pedido</param>
        /// <returns></returns>
        Pedido Add(Pedido order, Estacione station);

        /// <summary>
        /// Busca un pedido a partir de su identificador numérico
        /// </summary>
        /// <param name="order">Identificador numérico del pedido</param>
        /// <returns>Pedido que corresponde al identificador</returns>
        Pedido Find(int idOrder);

        /// <summary>
        /// Busca un pedido a partir de su  folio
        /// </summary>
        /// <param name="folio">Folio del pedido que se busca</param>
        /// <returns>Pedido que corresponda con él folio</returns>
        Pedido FindByFolio(int folio);

        /// <summary>
        /// Enlista todos los pedidos existentes
        /// </summary>
        /// <returns>Lista de pedidos</returns>
        List<Pedido> List();

        /// <summary>
        /// Enlista todos los pedidos que coinciden total o parcialmente en su folio, o razón social del cliente, con el valor que se especifíca
        /// </summary>
        /// <param name="value">Valor a buscar en coincidencia</param>
        /// <returns>Lista de pedidos que coincidan con la búsqueda</returns>
        List<Pedido> WithFolioOrClientLike(string value);

        /// <summary>
        /// Cancela un pedido
        /// </summary>
        /// <param name="idOrder">Identificador del pedido a cancelar</param>
        /// <param name="reason">Motivo por el que se cancela el pedido</param>
        /// <returns>Pedido cancelado</returns>
        Pedido Cancel(int idOrder, string reason);

        /// <summary>
        /// Actualiza un pedido
        /// </summary>
        /// <param name="pedido">Pedido a actualizar</param>
        /// <returns>Pedido actualizado</returns>
        Pedido Update(VMPedido order);

        List<DetallesDePedido> PendingOrdersReport();

        List<DetallesDePedido> CustomerOrders(Cliente customer);

        /// <summary>
        /// Obtiene los pedidos para el reporte de estado de la empresa
        /// </summary>
        /// <param name="vm">objeto a llenar con los totales</param>
        /// <param name="startDate">Fecha de inicio para el reporte</param>
        /// <param name="endDate">Fecha de fin para el reporte</param>
        /// <returns></returns>
        VMEstadoDeLaEmpresa ListOrdersForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate);
    }
}
