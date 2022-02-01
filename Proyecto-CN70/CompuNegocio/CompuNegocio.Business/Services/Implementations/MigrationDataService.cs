using System;
using System.Collections.Generic;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using Aprovi.Data.Core;
using System.IO;
using OfficeOpenXml;

namespace Aprovi.Business.Services
{
    public class MigrationDataService : IMigrationDataService
    {
        private IVFPDataExtractorRepository _vfpData;
        private IArticulosRepository _items;
        private IUnitOfWork _UOW;
        private IProductosServiciosRepository _satCatalog;

        public MigrationDataService(IUnitOfWork UnitOfWork)
        {
            _UOW = UnitOfWork;
            _vfpData = _UOW.VFPDataExtractor;
            _items = _UOW.Articulos;
            _satCatalog = _UOW.ProductosYServicios;
        }

        public List<Articulo> GetArticulos(string dbcPath, out Dictionary<string, decimal> stock)
        {
            try
            {
                return _vfpData.GetArticulos(dbcPath, out stock);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMEquivalenciaClasificacion> GetClasificaciones(string dbcPath)
        {
            try
            {
                var classes = new List<VMEquivalenciaClasificacion>();

                var originalClassifications = _vfpData.GetFamiliasYDepartamentos(dbcPath);

                foreach (var oc in originalClassifications)
                {
                    classes.Add(new VMEquivalenciaClasificacion(oc.Key, oc.Value));
                }

                return classes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMEquivalenciaUnidades> GetUnidadesDeMedida(string dbcPath)
        {
            try
            {
                var units = new List<VMEquivalenciaUnidades>();

                var originalUnits = _vfpData.GetUnidadesDeMedida(dbcPath);

                foreach (var ou in originalUnits)
                {
                    units.Add(new VMEquivalenciaUnidades(ou));
                }

                return units;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Homologate(string excelFile, string CNStartCell, string SATStartCell)
        {
            try
            {
                var excel = new FileInfo(excelFile);

                //Creo el archivo en memoria
                using (ExcelPackage file = new ExcelPackage(excel, true))
                {
                    //Se obtiene la hoja de excel
                    ExcelWorksheet sheet = file.Workbook.Worksheets[1];

                    //Obtengo columna y renglon donde comienzan los códigos
                    var rCN = 0;
                    var cCN = "";
                    var rSAT = 0;
                    var cSAT = "";
                    var migrated = 0;

                    //Celda CompuNegocio
                    cCN = CNStartCell.Substring(0, CNStartCell.IndexOf('-'));
                    rCN = CNStartCell.Substring(CNStartCell.IndexOf('-')+1).ToInt();

                    //celda SAT
                    cSAT = SATStartCell.Substring(0, SATStartCell.IndexOf('-'));
                    rSAT = SATStartCell.Substring(SATStartCell.IndexOf('-')+1).ToInt();

                    var hasData = false;
                    do
                    {
                        //Obtengo el código del artículo en CompuNegocio
                        var codigoCN = sheet.Cells[string.Format("{0}{1}", cCN, rCN)].Value.ToString().Trim();

                        //Obtengo el código homologado del SAT
                        var codigoSAT = sheet.Cells[string.Format("{0}{1}", cSAT, rSAT)].Value.ToString().Trim();

                        var item = _items.Find(codigoCN);

                        //Si no lo encuentra me lo brinco
                        if (!item.isValid())
                        {
                            rCN++;
                            rSAT++;
                            //Reviso la evaluacion del siguiente renglón para ver si llegué al final
                            hasData = sheet.Cells[string.Format("{0}{1}", cCN, rCN)].Value.isValid();
                            continue;
                        }

                        //Si lo encuentro le cambio el código del SAT
                        var satItem = _satCatalog.Find(codigoSAT);

                        //Si no lo encuentro en el catálogo registrado me lo brinco
                        if (!satItem.isValid())
                        {
                            rCN++;
                            rSAT++;
                            //Reviso la evaluacion del siguiente renglón para ver si llegué al final
                            hasData = sheet.Cells[string.Format("{0}{1}", cCN, rCN)].Value.isValid();
                            continue;
                        }

                        //Como encontré ambos entonces hago el cambio y el conteo de registros migrados
                        item.idProductoServicio = satItem.idProductoServicio;
                        item.ProductosServicio = satItem;
                        migrated++;

                        //Aumento el conteo para avanzar
                        rCN++;
                        rSAT++;

                        //Reviso la evaluacion del siguiente renglón para ver si llegué al final
                        hasData = sheet.Cells[string.Format("{0}{1}", cCN, rCN)].Value.isValid();

                    } while (hasData);

                    //Guardo de forma permanente los cambios en la base de datos
                    _UOW.Save();

                    return migrated;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
