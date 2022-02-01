using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IListaDePrecioService
    {
        /// <summary>
        /// Agrega una nueva lista de precios al catálogo actual
        /// </summary>
        /// <param name="pricesList">Lista de precios que se desea agregar</param>
        /// <returns>Lista de precios agregada</returns>
        ListasDePrecio Add(ListasDePrecio pricesList);

        /// <summary>
        /// Enlista las listas de precios existentes
        /// </summary>
        /// <returns>Listas de precios</returns>
        List<ListasDePrecio> List();

        /// <summary>
        /// Enlista las listas de precios que contienen en su código el código que se busca
        /// </summary>
        /// <param name="code">Código o parte del código que se busca</param>
        /// <returns>Listas de precio que coinciden o contienen el código buscado</returns>
        List<ListasDePrecio> WithCodeLike(string code);

        /// <summary>
        /// Enlista las listas de precios que contienen precios para articulos que en su código o descripción coinciden con el artículo buscado
        /// </summary>
        /// <param name="item">Código o descripción total o parcial del artículo que se busca</param>
        /// <returns>Listas de precio que contienen precios de artículos en coincidencia</returns>
        List<ListasDePrecio> WithItemLike(string item);

        /// <summary>
        /// Enlista las listas de precios que contienen clientes que en su código o razón social coinciden con el cliente buscado
        /// </summary>
        /// <param name="client">Código o razón social total o parcial del cliente que se busca</param>
        /// <returns>Listas de precios que contienen clientes en coincidencia</returns>
        List<ListasDePrecio> WithClientLike(string client);

        /// <summary>
        /// Obtiene la lista de precios que coincida con el código
        /// </summary>
        /// <param name="code">Código de la lista de precios que se busca</param>
        /// <returns>Lista de precios relacionada al código</returns>
        ListasDePrecio Find(string code);

        /// <summary>
        /// Obtiene una lista de precios que coincida con el identificador numérico
        /// </summary>
        /// <param name="idPricesList">Identificador numérico de la lista</param>
        /// <returns>Lista de precios relacionada al identificador</returns>
        ListasDePrecio Find(int idPricesList);

        /// <summary>
        /// Revisa si existe un precio especial para el articulo y cliente que se le pasa
        /// </summary>
        /// <param name="idItem">Identificador del Artículo a buscar</param>
        /// <param name="idClient">Identificador del Cliente</param>
        /// <returns>Precio en caso de que exista</returns>
        Precio Find(int idItem, int idClient);

        /// <summary>
        /// Actualiza la información de una lista de precios
        /// </summary>
        /// <param name="pricesList">Lista de precios actualizada</param>
        /// <returns>Lista de precios con los cambios generados</returns>
        ListasDePrecio Update(ListasDePrecio pricesList);

        /// <summary>
        /// Elimina una lista de precios
        /// </summary>
        /// <param name="pricesList">Lista de precios a eliminar</param>
        void Delete(ListasDePrecio pricesList);

        /// <summary>
        /// Agrega un precio a la lista de precios que trae el Precio
        /// </summary>
        /// <param name="price">Precio con lista de precios asignada en idListaDePrecio</param>
        /// <returns>Precio agregado</returns>
        Precio Add(Precio price);

        /// <summary>
        /// Elimina un precio de la lista de precios que trae el Precio
        /// </summary>
        /// <param name="price">Precio con idListaDePrecio de donde se eliminará</param>
        void Remove(Precio price);

        /// <summary>
        /// Actualiza la utilidad de un precio ya existente
        /// </summary>
        /// <param name="price">Precio con la nueva utilidad</param>
        /// <returns>Precio con el cambio realizado</returns>
        Precio Update(Precio price);

        /// <summary>
        /// Agrega un cliente a la lista de precios que trae el Cliente
        /// </summary>
        /// <param name="client">Cliente con lista de precios asignada en idListaDePrecio</param>
        /// <returns>Cliente con los datos actualizados</returns>
        Cliente Add(Cliente client);

        /// <summary>
        /// Elimina un cliente de la lista de precios que trae el Cliente
        /// </summary>
        /// <param name="client">Cliente con idListaDePrecio de donde se eliminará</param>
        void Remove(Cliente client);
    }
}
