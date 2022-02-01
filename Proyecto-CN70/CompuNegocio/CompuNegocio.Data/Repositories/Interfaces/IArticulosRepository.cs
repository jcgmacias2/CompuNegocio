using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Data.Repositories
{
    public interface IArticulosRepository : IBaseRepository<Articulo>
    {
        bool CanDelete(int idItem);

        Articulo Find(string code);

        List<Articulo> WithNameLike(string name);

        List<Articulo> List(int idClassification);

        List<Articulo> WithFlow(DateTime start, DateTime end);

        Articulo SearchAll(string code);

        List<VwListaArticulo> GetItemsForPagedList(int itemsAmount, int pageNumber);

        /// <summary>
        /// Obtiene los articulos para la lista de articulos
        /// </summary>
        /// <returns></returns>
        List<VwListaArticulo> GetItemsForList();

        /// <summary>
        /// Obtiene los articulos para la lista de articulos
        /// </summary>
        /// <param name="name">Valor para el filtro</param>
        /// <returns></returns>
        List<VwListaArticulo> GetItemsForListWithNameLike(string name);

        List<Articulo> FindAllForProvider(string code, int idSupplier);

        List<Articulo> FindAllForCustomer(string code, int idCustomer);
    }
}
