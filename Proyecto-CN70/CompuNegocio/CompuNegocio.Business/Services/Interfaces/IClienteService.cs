using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IClienteService 
    {
        /// <summary>
        /// Agrega un nuevo cliente al catálogo de clientes
        /// </summary>
        /// <param name="client">Cliente a agregar</param>
        /// <returns>Cliente agregado con identificado autogenerado</returns>
        Cliente Add(Cliente client);

        /// <summary>
        /// Busca un cliente entre los existentes a partir de su id
        /// </summary>
        /// <param name="idClient">Identificador numerico del cliente que se busca</param>
        /// <returns>Cliente al que pertenece el identificador</returns>
        Cliente Find(int idClient);

        /// <summary>
        /// Busca un cliente entre los existentes a partir de su código
        /// </summary>
        /// <param name="code">Código del cliente que se busca</param>
        /// <returns>Cliente al que pertenece el código</returns>
        Cliente Find(string code);

        /// <summary>
        /// Lista de clientes activos
        /// </summary>
        /// <returns>Lista de clientes</returns>
        List<Cliente> List();

        /// <summary>
        /// Enlista los clientes que coinciden con el nombre que se busca ya sea en su codigo o nombre comercial
        /// </summary>
        /// <param name="name">Nombre con el que debe coincidir</param>
        /// <returns>Lista de clientes que coinciden</returns>
        List<Cliente> WithNameLike(string name);

        /// <summary>
        /// Actualiza la información existente de un cliente
        /// </summary>
        /// <param name="client">Cliente con la información actualizada</param>
        /// <returns>Cliente con los cambios efectuados</returns>
        Cliente Update(Cliente client);

        /// <summary>
        /// Valida si el cliente no tiene operaciones realizadas y entonces es posible eliminarlo
        /// </summary>
        /// <param name="client">Cliente que se desea validar</param>
        /// <returns>True si es posible eliminarlo</returns>
        bool CanDelete(Cliente client);

        /// <summary>
        /// Elimina un cliente del catálogo existente
        /// </summary>
        /// <param name="client">Cliente que se desea eliminar</param>
        void Delete(Cliente client);

        /// <summary>
        /// Verifica si el rfc que se le pasa ya existe
        /// </summary>
        /// <param name="rfc">Registro federal de contribuyente</param>
        /// <returns>True si existe</returns>
        bool Exist(string rfc);

        /// <summary>
        /// Importa los clientes del contenedor que se le pasa hacia la base de datos local
        /// </summary>
        /// <param name="dbcPath">Archivo contenedor de las tablas de VPF (Bdd.dbc)</param>
        void Import(string dbcPath);

        /// <summary>
        /// Obtiene los totales de facturas y remisiones
        /// </summary>
        /// <param name="client">Cliente del que se obtendran los totales</param>
        /// <returns>Totales del cliente proporcionado</returns>
        List<VwSaldosPorClientePorMoneda> GetTotals(Cliente client);

        /// <summary>
        /// Obtiene las facturas y remisiones activas del cliente
        /// </summary>
        /// <param name="client">Cliente del que se obtendran las transacciones</param>
        /// <param name="term">Valor para el filtro</param>
        /// <param name="withDebtOnly">Especifica si se mostraran solo las transacciones con saldo pendiente</param>
        /// <returns>Transacciones del cliente proporcionado</returns>
        List<VwVentasActivasPorCliente> GetActiveTransactions(Cliente client, string term, bool withDebtOnly);

        /// <summary>
        /// Verifica si un cliente tiene suficiente credito disponible para la cantidad indicada
        /// </summary>
        /// <param name="customer">cliente del que se verificara el credito</param>
        /// <param name="currency">Moneda a verificar</param>
        /// <param name="amount">cantidad a credito</param>
        /// <returns>true si el cliente tiene credito suficiente, false en caso contrarioÍU</returns>
        bool HasAvailableCredit(Cliente customer, Moneda currency, decimal amount);
    }
}
