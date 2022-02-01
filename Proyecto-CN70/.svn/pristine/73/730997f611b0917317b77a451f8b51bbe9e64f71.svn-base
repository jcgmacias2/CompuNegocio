using System;
using System.Collections.Generic;
using Aprovi.Data.Models;
using Aprovi.Data.Core;

namespace Aprovi.Data.Repositories
{
    public class VFPDataExtractorRepository : IVFPDataExtractorRepository
    {
        public VFPDataExtractorRepository() { }

        public List<Articulo> GetArticulos(string dbcPath, out Dictionary<string, decimal> stock)
        {
            try
            {
                //La lista donde se agregaran los articulos
                var items = new List<Articulo>();
                stock = new Dictionary<string, decimal>();

                //Inicializo el helper para la conexión
                var connection = new VFPConnection(dbcPath);
                //Pruebo la conexión
                connection.TestConnection(dbcPath);

                //Abro la conexión
                connection.Conexion.Open();

                var reader = connection.ExecuteReader("SELECT A.Num_art, A.Desc, A.Dpto, A.Familia, A.Costo, A.UtilA, A.UtilB, A.UtilC, A.UtilD, A.Nacional, A.Unidades, A.Iva_paga, A.Ieps_paga, A.Existencia FROM Articulo AS A");
                if (!reader.HasRows)
                {
                    connection.Conexion.Close();
                    return items;
                }

                while (reader.Read())
                {
                    var i = new Articulo();

                    i.codigo = reader["Num_art"].ToString().Trim();
                    i.descripcion = reader["Desc"].ToString().Trim();
                    i.costoUnitario = reader["Costo"].ToDecimal();
                    i.idMoneda = reader["Nacional"].ToString().Equals("N") ? (int)Monedas.Pesos : (int)Monedas.Dólares;
                    i.UnidadesDeMedida = new UnidadesDeMedida() { descripcion = reader["Unidades"].ToString() };

                    //Le agrego las familias y departamentos como clasificaciones
                    if (reader["Dpto"].ToString().isValid())
                        i.Clasificaciones.Add(new Clasificacione() { descripcion = reader["Dpto"].ToString().Trim() });

                    if (reader["Familia"].ToString().isValid())
                        i.Clasificaciones.Add(new Clasificacione() { descripcion = reader["Familia"].ToString().Trim() });

                    //Le agrego los 4 precios
                    i.Precios.Add(new Precio() { idListaDePrecio = (int)List.A, utilidad = reader["Utila"].ToDecimal() });
                    i.Precios.Add(new Precio() { idListaDePrecio = (int)List.B, utilidad = reader["Utilb"].ToDecimal() });
                    i.Precios.Add(new Precio() { idListaDePrecio = (int)List.C, utilidad = reader["Utilc"].ToDecimal() });
                    i.Precios.Add(new Precio() { idListaDePrecio = (int)List.D, utilidad = reader["Utild"].ToDecimal() });

                    //Le agrego los impuestos
                    i.Impuestos = new List<Impuesto>();
                    //Si no es 3 = IEPS lleva iva
                    if (!reader["Iva_paga"].ToString().Equals("3"))
                        i.Impuestos.Add(new Impuesto() { idImpuesto = (int)Impuestos.IVA });

                    //La existencia par hacer el ajuste
                    if (stock.ContainsKey(i.codigo))
                        throw new Exception(string.Format("El artículo {0} esta duplicado", i.codigo));
                    stock.Add(i.codigo, reader["Existencia"].ToDecimal());

                    items.Add(i);
                }

                //Cierro la conexión
                connection.Conexion.Close();

                return items;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<string, string> GetFamiliasYDepartamentos(string dbcPath)
        {
            try
            {
                //La lista donde se agregaran las familias y departamentos
                var classes = new Dictionary<string, string>();

                //Inicializo el helper para la conexión
                var connection = new VFPConnection(dbcPath);
                //Pruebo la conexión
                connection.TestConnection(dbcPath);

                //Abro la conexión
                connection.Conexion.Open();

                var reader = connection.ExecuteReader("SELECT T.Tipo, T.Clave_tabl, T.Describe FROM Tabla AS T WHERE Tipo = 'D' OR Tipo = 'A'");
                if (!reader.HasRows)
                {
                    connection.Conexion.Close();
                    return classes;
                }

                while (reader.Read())
                {
                    classes.Add(string.Format("{0}-{1}", reader["Tipo"].ToString().Trim().Equals("D") ? "D" : "F", reader["Clave_tabl"].ToString().Trim()), reader["Describe"].ToString().Trim());
                }

                //Cierro la conexión
                connection.Conexion.Close();

                return classes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<string> GetUnidadesDeMedida(string dbcPath)
        {
            try
            {
                //La lista donde se agregaran las unidades
                var units = new List<string>();

                //Inicializo el helper para la conexión
                var connection = new VFPConnection(dbcPath);
                //Pruebo la conexión
                connection.TestConnection(dbcPath);

                //Abro la conexión
                connection.Conexion.Open();

                var reader = connection.ExecuteReader("SELECT DISTINCT(A.Unidades) AS Unidades FROM Articulo AS A");
                if (!reader.HasRows)
                {
                    connection.Conexion.Close();
                    return units;
                }

                while (reader.Read())
                {
                    units.Add(reader["Unidades"].ToString().Trim());
                }

                //Cierro la conexión
                connection.Conexion.Close();

                return units;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
