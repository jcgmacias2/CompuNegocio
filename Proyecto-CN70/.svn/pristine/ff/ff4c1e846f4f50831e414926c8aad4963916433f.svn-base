using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewReporteVentasPorArticuloRepository : BaseRepository<VwReporteVentasPorArticulo>, IViewReporteVentasPorArticuloRepository
    {
        public ViewReporteVentasPorArticuloRepository(CNEntities context) : base(context) { }

        public List<VwReporteVentasPorArticulo> ListDetailed(Articulo item, Clasificacione classification, DateTime startDate, DateTime endDate, bool includeInvoices, bool includeBillsOfSale, bool includeCancelled)
        {
            try
            {
                //Si se filtra por clasificacion, se utiliza la consulta sql
                if (classification.isValid() && classification.idClasificacion.isValid())
                {
                    string query = string.Format("SELECT RVPA.* FROM dbo.VwReporteVentasPorArticulo AS RVPA JOIN dbo.ClasificacionesPorArticulo AS CPA ON RVPA.idArticulo = CPA.idArticulo AND CPA.idClasificacion = {0} WHERE RVPA.fecha >= '{1}' AND RVPA.fecha <= '{2}'", classification.idClasificacion, startDate.Date.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

                    //Se agregan las condiciones
                    if (!includeInvoices || !includeBillsOfSale || !includeCancelled)
                    {
                        query += " AND ";
                    }

                    if (!includeInvoices)
                    {
                        query += string.Format("RVPA.idEstatusDeFactura IS NULL{0}",(!includeBillsOfSale || !includeCancelled) ? " AND " : "");
                    }

                    if (!includeBillsOfSale)
                    {
                        query += string.Format("RVPA.idEstatusDeRemision IS NULL{0}", (!includeCancelled) ? " AND " : "");
                    }

                    if (!includeCancelled)
                    {
                        query += string.Format(
                            "((RVPA.idEstatusDeFactura != {0} AND RVPA.idEstatusDeFactura != {1}) OR RVPA.idEstatusDeFactura IS NULL) AND (RVPA.idEstatusDeRemision != {2} OR RVPA.idEstatusDeRemision IS NULL)",
                            (int) StatusDeFactura.Cancelada, (int) StatusDeFactura.Anulada,
                            (int) StatusDeRemision.Cancelada);
                    }

                    return _dbSet.SqlQuery(query).AsNoTracking().ToList();
                }

                //De lo contrario se efectua la consulta usual
                var data = _dbSet.Where(x => DbFunctions.TruncateTime(x.fecha) >= DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(x.fecha) <= DbFunctions.TruncateTime(endDate) && x.idEstatusDeRemision.Value != (int)StatusDeRemision.Facturada).AsQueryable();

                if (item.isValid() && item.idArticulo.isValid())
                {
                    data = data.Where(x => x.idArticulo == item.idArticulo);
                }

                if (!includeInvoices)
                {
                    data = data.Where(x => x.idEstatusDeFactura == null);
                }

                if (!includeBillsOfSale)
                {
                    data = data.Where(x => x.idEstatusDeRemision == null);
                }

                if (!includeCancelled)
                {
                    data = data.Where(x =>
                        x.idEstatusDeFactura != (int) StatusDeFactura.Cancelada &&
                        x.idEstatusDeFactura != (int) StatusDeFactura.Anulada &&
                        x.idEstatusDeRemision != (int) StatusDeRemision.Cancelada);
                }

                return data.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwReporteVentasPorArticulo> ListTotals(Articulo item, Clasificacione classification, DateTime startDate, DateTime endDate,
            bool includeInvoices, bool includeBillsOfSale, bool includeCancelled)
        {
            try
            {
                string query;

                //Si se filtra por clasificacion, se utiliza la consulta sql
                if (classification.isValid() && classification.idClasificacion.isValid())
                {
                    query = string.Format("SELECT RVPA.codigoArticulo, RVPA.idArticulo, RVPA.articulo, RVPA.idMonedaArticulo, SUM(RVPA.cantidad) AS cantidad, SUM(RVPA.importe) as importe, AVG(RVPA.precio) AS precio, RVPA.idMonedaTransaccion, RVPA.descripcionMoneda, 1.0 AS tipoDeCambio, 0.0 AS costoUnitario, '' AS nota, '' as folioTexto, cast('1900-01-01' AS datetime) AS fecha, '' AS cliente, '' AS codigoCliente, '' AS Estatus, NULL AS idEstatusDeFactura, NULL AS idEstatusDeRemision, 0 AS folio, '' AS serie FROM dbo.VwReporteVentasPorArticulo AS RVPA JOIN dbo.ClasificacionesPorArticulo AS CPA ON RVPA.idArticulo = CPA.idArticulo AND CPA.idClasificacion = {0} WHERE RVPA.fecha >= '{1}' AND RVPA.fecha <= '{2}'", classification.idClasificacion, startDate.Date.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
                }
                else
                {
                    query = string.Format("SELECT RVPA.codigoArticulo, RVPA.idArticulo, RVPA.articulo, RVPA.idMonedaArticulo, SUM(RVPA.cantidad) AS cantidad, SUM(RVPA.importe) as importe, AVG(RVPA.precio) AS precio, RVPA.idMonedaTransaccion, RVPA.descripcionMoneda, 1.0 AS tipoDeCambio, 0.0 AS costoUnitario, '' AS nota, '' as folioTexto, cast('1900-01-01' AS datetime) AS fecha, '' AS cliente, '' AS codigoCliente, '' AS Estatus, NULL AS idEstatusDeFactura, NULL AS idEstatusDeRemision, 0 AS folio, '' AS serie FROM dbo.VwReporteVentasPorArticulo AS RVPA WHERE RVPA.fecha >= '{0}' AND RVPA.fecha <= '{1}'", startDate.Date.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
                }

                //Se agregan las condiciones
                if (!includeInvoices || !includeBillsOfSale || !includeCancelled)
                {
                    query += " AND ";
                }

                if (!includeInvoices)
                {
                    query += string.Format("RVPA.idEstatusDeFactura IS NULL{0}", (!includeBillsOfSale || !includeCancelled) ? " AND " : "");
                }

                if (!includeBillsOfSale)
                {
                    query += string.Format("RVPA.idEstatusDeRemision IS NULL{0}", (!includeCancelled) ? " AND " : "");
                }

                if (!includeCancelled)
                {
                    query += string.Format(
                        "((RVPA.idEstatusDeFactura != {0} AND RVPA.idEstatusDeFactura != {1}) OR RVPA.idEstatusDeFactura IS NULL) AND (RVPA.idEstatusDeRemision != {2} OR RVPA.idEstatusDeRemision IS NULL)",
                        (int)StatusDeFactura.Cancelada, (int)StatusDeFactura.Anulada,
                        (int)StatusDeRemision.Cancelada);
                }

                query += " GROUP BY	RVPA.idArticulo, RVPA.codigoArticulo, RVPA.articulo,RVPA.idMonedaArticulo, RVPA.idMonedaTransaccion, RVPA.descripcionMoneda";

                return _dbSet.SqlQuery(query).AsNoTracking().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
