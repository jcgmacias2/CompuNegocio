using Aprovi.Data.Models;
using System;
using System.Data;
using System.Data.OleDb;

namespace Aprovi.Data.Core
{
    public class VFPConnection
    {

        public VFPConnection(string dbcPath)
        {
            try
            {
                _conexion = new OleDbConnection(string.Format(@"Provider=VFPOLEDB.1 ;Data Source={0}", dbcPath));
            }
            catch (OleDbException)
            {
                throw;
            }
        }

        #region Campos

        private OleDbConnection _conexion;

        #endregion

        #region Propiedades

        /// <summary>
        /// Proporciona la conexión inicializada hacia el container de la base de datos de VFP
        /// </summary>
        public OleDbConnection Conexion
        {
            get { return _conexion; }
        }

        #endregion

        #region Métodos 

        /// <summary>
        /// Crea commando
        /// </summary>
        /// <param name="commandText">comando texto</param>
        /// <param name="connection">Conexion</param>
        /// <returns>Comando</returns>
        private OleDbCommand CreateCommand(string commandText, OleDbConnection connection)
        {
            OleDbCommand cmd;

            cmd = connection.CreateCommand();
            cmd = new OleDbCommand(commandText, connection);
            //cmd.CommandText = commandText;

            return cmd;
        }

        /// <summary>
        /// Ejecuta un Query
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>Numero de datos insertados</returns>
        public int ExecuteNonQuery(string query)
        {
            OleDbCommand command;
            int result = 0;
            try
            {
                command = CreateCommand(query, Conexion);
                result = command.ExecuteNonQuery();
                command.Dispose();

            }
            catch (OleDbException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public OleDbDataReader ExecuteReader(string query)
        {
            OleDbCommand command;
            OleDbDataReader result = null;

            try
            {
                command = CreateCommand(query, Conexion);
                result = command.ExecuteReader();
            }
            catch (OleDbException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public object ExecuteScalar(string query)
        {
            OleDbCommand command;
            object result = string.Empty;

            try
            {
                command = CreateCommand(query, Conexion);
                result = command.ExecuteScalar();
            }
            catch (OleDbException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public string ExecuteStoredProcedure(string spName, params Parameter[] parameters)
        {
            OleDbCommand command;
            var result = new DataTable();
            string folio;
            try
            {
                command = new OleDbCommand();
                command.Connection = Conexion;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = spName;
                foreach (Parameter p in parameters)
                {
                    command.Parameters.Add(new OleDbParameter(p.Name, p.Value));
                }
                var adapter = new OleDbDataAdapter(command);
                adapter.Fill(result);
                folio = result.Rows[0][0].ToString();
                command.Dispose();
            }
            catch (OleDbException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return folio;
        }

        public bool TestConnection(string dbcPath)
        {
            try
            {
                _conexion = new OleDbConnection(string.Format(@"Provider=VFPOLEDB.1 ;Data Source={0}", dbcPath));
                Conexion.Open();
                Conexion.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
