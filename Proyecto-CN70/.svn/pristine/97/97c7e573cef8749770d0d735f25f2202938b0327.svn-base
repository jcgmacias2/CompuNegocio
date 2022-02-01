using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ProveedoresRepository : BaseRepository<Proveedore>, IProveedoresRepository
    {
        public ProveedoresRepository(CNEntities context) : base(context) { }

        public Proveedore Find(string code)
        {
            try
            {
                return _dbSet.FirstOrDefault(p => p.codigo.Equals(code, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Proveedore> WithCodeLike(string code)
        {
            try
            {
                return _dbSet.Where(p => p.codigo.Contains(code) || p.razonSocial.Contains(code)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CanDelete(int idSupplier)
        {
            try
            {
                var supplier = _dbSet.FirstOrDefault(p => p.idProveedor.Equals(idSupplier));

                if (supplier.Compras.Count > 0)
                    return false;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Exist(string rfc)
        {
            try
            {
                var supplier = _dbSet.FirstOrDefault(c => c.rfc.Equals(rfc, StringComparison.InvariantCultureIgnoreCase));

                if (supplier.isValid() && supplier.idProveedor.isValid())
                    return true;

                supplier = _dbSet.Local.FirstOrDefault(c => c.rfc.Equals(rfc, StringComparison.InvariantCultureIgnoreCase));

                return supplier.isValid();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Proveedore> List(string dbcPath)
        {
            try
            {
                //La lista donde se agregaran los articulos
                var clients = new List<Proveedore>();

                //Inicializo el helper para la conexión
                var connection = new VFPConnection(dbcPath);
                //Pruebo la conexión
                connection.TestConnection(dbcPath);

                //Abro la conexión
                connection.Conexion.Open();

                var reader = connection.ExecuteReader("SELECT P.Num_prov, P.Nom_prov, P.Rfc_prov, P.Dir_prov, P.Numext, P.Numint, P.Colonia, P.Ciud_prov,  P.Edo_pro, P.Cp_prov FROM Proveedo AS P");
                if (!reader.HasRows)
                {
                    connection.Conexion.Close();
                    return clients;
                }

                while (reader.Read())
                {
                    var p = new Proveedore();

                    p.codigo = reader["Num_prov"].ToString().Trim();
                    p.nombreComercial = reader["Nom_prov"].ToString().Trim();
                    p.razonSocial = reader["Nom_prov"].ToString().Trim();
                    p.rfc = reader["Rfc_prov"].ToString().Trim();
                    p.Domicilio = new Domicilio();
                    p.Domicilio.calle = reader["Dir_prov"].ToString().Trim();
                    p.Domicilio.numeroExterior = reader["Numext"].ToString().Trim();
                    p.Domicilio.numeroInterior = reader["Numint"].ToString().Trim();
                    p.Domicilio.colonia = reader["Colonia"].ToString().Trim();
                    p.Domicilio.ciudad = reader["Ciud_prov"].ToString().Trim(); 
                    p.Domicilio.estado = reader["Edo_pro"].ToString().Trim();
                    p.Domicilio.idPais = (int)Paises.México;
                    p.Domicilio.codigoPostal = reader["Cp_prov"].ToString().Trim();

                    clients.Add(p);
                }

                //Cierro la conexión
                connection.Conexion.Close();

                return clients;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Proveedore SearchAll(string code)
        {
            try
            {
                //Lo busco en la base de datos
                var supplier = _dbSet.FirstOrDefault(u => u.codigo.Equals(code, StringComparison.InvariantCultureIgnoreCase));

                //Si existe lo regreso
                if (supplier.isValid())
                    return supplier;

                //Si no existe en la base de datos, reviso el store local
                supplier = _dbSet.Local.FirstOrDefault(u => u.codigo.Equals(code, StringComparison.InvariantCultureIgnoreCase));

                //Regreso lo que haya obtenido, ya sea un null o lo que encontré localmente
                return supplier;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
