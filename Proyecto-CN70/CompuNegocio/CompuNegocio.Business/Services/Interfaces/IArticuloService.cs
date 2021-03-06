using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IArticuloService
    {
        /// <summary>
        /// Agrega un nuevo artículo a la lista existente
        /// </summary>
        /// <param name="item">Artículo a agregar</param>
        /// <returns>Artículo con identificador númerico generado</returns>
        Articulo Add(Articulo item);

        /// <summary>
        /// Enlista los artículos existentes y activos
        /// </summary>
        /// <returns>Lista de artículos</returns>
        List<Articulo> List();

        /// <summary>
        /// Enlista los artículos existentes y activos que contienen la clasificación especificada
        /// </summary>
        /// <param name="classification">Clasificación sobre la que se desea obtener la lista</param>
        /// <returns>Lista de artículos que contienen tal clasificación</returns>
        List<Articulo> List(Clasificacione classification);

        /// <summary>
        /// Enlista los artículos que tienen alguna coincidencia ya sea en código o nombre con el nombre que se pasa
        /// </summary>
        /// <param name="name">Nombre que se busca coincida</param>
        /// <returns>Lista de artículos en coincidencia</returns>
        List<Articulo> WithNameLike(string name);

        /// <summary>
        /// Busca un artículo con el identificador especificado
        /// </summary>
        /// <param name="idItem">Identificador númerico del artículo</param>
        /// <returns>Artículo que coincide con el identificador buscado</returns>
        Articulo Find(int idItem);

        /// <summary>
        /// Busca un artículo por su código, no es case sensitive
        /// </summary>
        /// <param name="code">código a buscar</param>
        /// <returns>Artículo al que corresponde el código</returns>
        Articulo Find(string code);

        /// <summary>
        /// Busca un artículo por código en comparación con el código principal y el alterno para el proveedor especificado
        /// </summary>
        /// <param name="code">Código a buscar</param>
        /// <param name="idSupplier">Proveedor al que corresponde</param>
        /// <returns>Lista de artículos que corresponden al código</returns>
        List<Articulo> FindAllForSupplier(string code, int idSupplier);

        /// <summary>
        /// Busca un artículo por código en comparación con el código principal y el alterno para el cliente especificado
        /// </summary>
        /// <param name="code">Código a buscar</param>
        /// <param name="idSupplier">Proveedor al que corresponde</param>
        /// <returns>Lista de artículos que corresponden al código</returns>
        List<Articulo> FindAllForCustomer(string code, int idCustomer);

        /// <summary>
        /// Actualiza la información de un artículo
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Articulo Update(Articulo item);

        /// <summary>
        /// Elimina un artículo del catálogo de artículos existente
        /// </summary>
        /// <param name="item">Artículo que se desea eliminar</param>
        void Delete(Articulo item);

        /// <summary>
        /// Verifica si es posible eliminar un artículo basado en si tiene relaciones o no
        /// </summary>
        /// <param name="item">Artículo que se desea verificar si es posible eliminar</param>
        /// <returns>True si es posible eliminarlo</returns>
        bool CanDelete(Articulo item);

        /// <summary>
        /// Consulta el inventario actual de un artículo
        /// </summary>
        /// <param name="idItem"></param>
        /// <returns></returns>
        decimal Stock(int idItem);

        /// <summary>
        /// Busca la lista de articulos en la base de datos
        /// </summary>
        /// <param name="items">Lista de articulos a buscar</param>
        /// <returns>Articulos que no se encontraron en la base de datos</returns>
        List<Articulo> GetMissingItems(List<Articulo> items);

        /// <summary>
        /// Provee todo el inventario actual con objetos VMInventario
        /// </summary>
        /// <returns>Lista de VMInventario's con la existencia actual</returns>
        List<VMInventario> Stock(List<Clasificacione> classifications);

        /// <summary>
        /// Provee información concentrada del flujo del inventario en el período solicitado
        /// </summary>
        /// <param name="start">Inicio del período del que se desea obtener la información</param>
        /// <param name="end">Fin del período del que se desea obtener la información</param>
        /// <returns>Lista de VMFlujoPorArticulo con la información solicitada</returns>
        List<VMFlujoPorArticulo> StockFlow(DateTime start, DateTime end);

        /// <summary>
        /// Provee información concentrada del flujo de inventario en el período solicitado respecto al artículo especificado
        /// </summary>
        /// <param name="start">Inicio del período del que se desea obtener la información</param>
        /// <param name="end">Fin del período del que se desea obtener la información</param>
        /// <param name="item">Artículo del que se desea obtener el reporte</param>
        /// <returns>VMFlujoPorArticulo con la información solicitada</returns>
        VMFlujoPorArticulo StockFlow(DateTime start, DateTime end, Articulo item);

        /// <summary>
        /// Provee información concentrada del flujo de inventario en el período solicitado respecto a los artículos de la clasificación especificada
        /// </summary>
        /// <param name="start">Inicio del período del que se desea obtener la información</param>
        /// <param name="end">Fin del período del que se desea obtener la información</param>
        /// <param name="classification"></param>
        /// <returns>Lista de VMFlujoPorArticulo con la información solicitada</returns>
        List<VMFlujoPorArticulo> StockFlow(DateTime start, DateTime end, Clasificacione classification);

        /// <summary>
        /// Provee información detallada del flujo del inventario en el período solicitado
        /// </summary>
        /// <param name="item">Artículo del cual se desea obtener el flujo detallado</param>
        /// <param name="start">Inicio del período del que se desea obtener la información</param>
        /// <param name="end">Fin del período del que se desea obtener la información</param>
        /// <returns>Lista de VMDetalleKardex que muestra la información solicitada</returns>
        List<VMDetalleKardex> StockFlow(Articulo item, DateTime start, DateTime end);

        /// <summary>
        /// Importa los artículos del contenedor que se le pasa hacia la base de datos local
        /// </summary>
        /// <param name="items">Información de artículos de VFP</param>
        /// <param name="classifications">Equivalencias de clasificaciones</param>
        /// <param name="units">Equivalencias de unidades de medida</param>
        /// <param name="vat">Impuesto a servir como IVA</param>
        /// <returns>Artículos integrados</returns>
        List<Articulo> Import(List<Articulo> items, List<VMEquivalenciaUnidades> units, List<VMEquivalenciaClasificacion> classifications, Impuesto vat);

        /// <summary>
        /// Obtiene las unidades de medida del articulo
        /// </summary>
        /// <param name="item">Articulo del que se obtendran las unidades de medida</param>
        /// <returns>Unidades de medida del articulo</returns>
        List<UnidadesDeMedida> GetItemUnits(Articulo item);

        /// <summary>
        /// Obtiene los articulos vendidos a un cliente
        /// </summary>
        /// <param name="customer">Cliente al que pertenecen los articulos</param>
        /// <param name="filter">Valor a buscar en el codigo, descripcion o fecha</param>
        /// <returns>Lista de articulos vendidos al cliente</returns>
        List<VwArticulosVendido> GetSoldItems(Cliente customer, string filter);

        /// <summary>
        /// Verifica si un articulo tiene transacciones relacionadas
        /// </summary>
        /// <param name="item">Articulo a verificar</param>
        /// <returns>Resultado de la verificacion</returns>
        bool HasTransactions(Articulo item);

        /// <summary>
        /// Obtiene los articulos para la lista de articulos
        /// </summary>
        /// <returns></returns>
        List<VMArticulo> GetItemsForList();

        /// <summary>
        /// Obtiene los articulos para la lista de articulos con paginado
        /// </summary>
        /// <returns></returns>
        List<VMArticulo> GetItemsForPagedList(int itemsAmount, int pageNumber);

        /// <summary>
        /// Obtiene los articulos para la lista de articulos
        /// </summary>
        /// <param name="name">Valor para el filtro</param>
        /// <returns></returns>
        List<VMArticulo> GetItemsForListWithNameLike(string name);

        /// <summary>
        /// Verifica si un articulo tiene activado la parte de pedimentos
        /// </summary>
        /// <param name="item">Artículo a verificar</param>
        /// <returns>Resultado de la verificación</returns>
        bool HasCustomsApplicationActive(Articulo item);

        /// <summary>
        /// Obtiene la lista de códigos para un artículo determinado
        /// </summary>
        /// <param name="idArticulo">Identificador del artículo</param>
        /// <returns>Lista de codigos relacionados</returns>
        List<VwCodigosDeArticuloPorProveedor> List(int idArticulo);

        /// <summary>
        /// Regresa una lista de articulos para un reporte de avaluo
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="onlyWithStock"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        VMRAvaluo GetAppraisal(FiltroReporteAvaluo filter, bool onlyWithStock, decimal exchangeRate, DateTime endDate, Clasificacione classification);

        /// <summary>
        /// Obtiene el avaluo para el reporte de estado de la empresa
        /// </summary>
        /// <param name="vm">objeto a llenar con los totales</param>
        /// <param name="startDate">Fecha de inicio para el reporte</param>
        /// <param name="endDate">Fecha de fin para el reporte</param>
        /// <returns></returns>
        VMEstadoDeLaEmpresa ListItemsForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate);
    }
}
