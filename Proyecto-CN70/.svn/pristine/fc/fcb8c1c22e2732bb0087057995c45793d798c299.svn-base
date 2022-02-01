using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ClientesRepository : BaseRepository<Cliente>, IClientesRepository
    {
        public ClientesRepository(CNEntities context) : base(context) { }

        public Cliente Find(string code)
        {
            try
            {
                return _dbSet.FirstOrDefault(c => c.codigo.Equals(code, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CanDelete(int idClient)
        {
            try
            {
                var local = _dbSet.FirstOrDefault(c => c.idCliente.Equals(idClient));

                if (local.Facturas.Count > 0)
                    return false;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Cliente> WithNameLike(string name)
        {
            try
            {
                return _dbSet.Where(c => c.codigo.Contains(name) || c.nombreComercial.Contains(name) || c.razonSocial.Contains(name)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Cliente Exist(string rfc)
        {
            try
            {
                var client = _dbSet.FirstOrDefault(c => c.rfc.Equals(rfc, StringComparison.InvariantCultureIgnoreCase));

                if (client.isValid() && client.idCliente.isValid())
                    return client;

                client = _dbSet.Local.FirstOrDefault(c => c.rfc.Equals(rfc, StringComparison.InvariantCultureIgnoreCase));

                return client.isValid() ? client : null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Cliente> List(string dbcPath)
        {
            try
            {
                //La lista donde se agregaran los articulos
                var clients = new List<Cliente>();

                //Inicializo el helper para la conexión
                var connection = new VFPConnection(dbcPath);
                //Pruebo la conexión
                connection.TestConnection(dbcPath);

                //Abro la conexión
                connection.Conexion.Open();

                var reader = connection.ExecuteReader("SELECT C.Num_cli, C.Nom_cli, C.Rfc_cli, C.Tel_cli, C.Email_cli, C.Contac_cli, C.Dir_cli, C.Num_ext, C.Num_int, C.Colon_cli, C.Ciud_cli, C.Estado_cli, C.Pais, C.Cp_cli, C.iva_cli, C.Guardianem, C.Precio_cli  FROM Cliente AS C");
                if (!reader.HasRows)
                {
                    connection.Conexion.Close();
                    return clients;
                }

                while (reader.Read())
                {
                    var c = new Cliente();

                    c.codigo = reader["Num_cli"].ToString().Trim();
                    c.nombreComercial = reader["Nom_cli"].ToString().Trim();
                    c.razonSocial = reader["Nom_cli"].ToString().Trim();
                    c.rfc = reader["Rfc_cli"].ToString().Trim();
                    c.telefono = reader["Tel_cli"].ToString().Trim();
                    c.correoElectronico = reader["Email_cli"].ToString().Trim();
                    c.contacto = reader["Contac_cli"].ToString().Trim();
                    var lista = reader["Precio_cli"].ToString().Trim();
                    c.idListaDePrecio = lista.isValid() ? (int)(Enum.Parse(typeof(List), lista)) : (int)Models.List.A;
                    c.Domicilio = new Domicilio();
                    c.Domicilio.calle = reader["Dir_cli"].ToString().Trim();
                    c.Domicilio.numeroExterior = reader["Num_ext"].ToString().Trim();
                    c.Domicilio.numeroInterior = reader["Num_int"].ToString().Trim();
                    c.Domicilio.colonia = reader["Colon_cli"].ToString().Trim();
                    c.Domicilio.ciudad = reader["Ciud_cli"].ToString().Trim();
                    c.Domicilio.estado = reader["Estado_cli"].ToString().Trim();
                    c.Domicilio.idPais = reader["Pais"].ToString().Substring(0,1).Equals("M")? 1 : 2;
                    c.Domicilio.codigoPostal = reader["Cp_cli"].ToString().Trim();

                    //Verifico si trae cuentas de correo para migrar
                    var cuentasConcatenadas = reader["Guardianem"].ToString();
                    if(cuentasConcatenadas.Trim().isValid())
                    {
                        c.CuentasDeCorreos = new List<CuentasDeCorreo>();
                        foreach (var cuenta in cuentasConcatenadas.Split(';'))
                        {
                            //La cuenta no debe estar repetida para el mismo cliente
                            if(cuenta.Trim().isValid() && !c.CuentasDeCorreos.FirstOrDefault(e => e.cuenta.Equals(cuenta.Trim())).isValid())
                                c.CuentasDeCorreos.Add(new CuentasDeCorreo() { cuenta = cuenta.Trim() });
                        }
                    }

                    clients.Add(c);
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

        public Cliente SearchAll(string code)
        {
            try
            {
                //Lo busco en la base de datos
                var client = _dbSet.FirstOrDefault(u => u.codigo.Equals(code, StringComparison.InvariantCultureIgnoreCase));

                //Si existe lo regreso
                if (client.isValid())
                    return client;

                //Si no existe en la base de datos, reviso el store local
                client = _dbSet.Local.FirstOrDefault(u => u.codigo.Equals(code, StringComparison.InvariantCultureIgnoreCase));

                //Regreso lo que haya obtenido, ya sea un null o lo que encontré localmente
                return client;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
