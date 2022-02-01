using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;

namespace Aprovi.Business.Services
{
    public interface IImpuestoService
    {
        /// <summary>
        /// Agrega un nuevo impuesto a la colección existente
        /// </summary>
        /// <param name="tax">Impuesto a agregar</param>
        /// <returns>Impuesto agregado con identificador autogenerado</returns>
        Impuesto Add(Impuesto tax);

        /// <summary>
        /// Enlista los impuestos existentes y puede incluir los inactivos si includeInactive = True
        /// </summary>
        /// <param name="includeInactive">Cuando es true, incluye los impuestos inactivos</param>
        /// <returns>Lista de impuestos existentes</returns>
        List<Impuesto> List(bool includeInactive);

        /// <summary>
        /// Enlista los impuestos que tienen coincidencia con el nombre que se les pasa
        /// </summary>
        /// <param name="name">Nombre en coincidencia a buscar</param>
        /// <returns>Lista de impuesto con similitud</returns>
        List<Impuesto> WithNameLike(string name);

        /// <summary>
        /// Busca un impuesto a partir de su identificador unico
        /// </summary>
        /// <param name="id">Identificador unico del impuesto</param>
        /// <returns>Impuesto que coincide con el identificador</returns>
        Impuesto Find(int id);

        /// <summary>
        /// Busca un impuesto que coincida con la tasa y el tipo de impuesto
        /// </summary>
        /// <param name="code">Código del impuesto según el catálogo</param>
        /// <param name="rate">Tasa del impuesto</param>
        /// <param name="type">Tipo de impuesto</param>
        /// <returns>Impuesto si lo encuentra, null si no existe</returns>
        Impuesto Find(string code, decimal rate, TiposDeImpuesto type);

        /// <summary>
        /// Actualiza los datos de un impuesto existente, la tasa de impuesto no es editable, si contiene un cambio, será ignorado
        /// </summary>
        /// <param name="tax">Impuesto con información actualizada</param>
        /// <returns>Impuesto modificado</returns>
        Impuesto Update(Impuesto tax);

        /// <summary>
        /// Elimina un impuesto existente
        /// </summary>
        /// <param name="tax">Impuesto a eliminar</param>
        void Delete(Impuesto tax);

        /// <summary>
        /// Valida si un impuesto puede ser eliminado, basandose en si ya esta relacionado o no
        /// </summary>
        /// <param name="tax">Impuesto a validar</param>
        /// <returns>True si puede ser eliminado</returns>
        bool CanDelete(Impuesto tax);

        /// <summary>
        /// Regresa los totales de impuestos de facturas y notas de credito por periodo
        /// </summary>
        /// <param name="startDate">Fecha de inicio</param>
        /// <param name="endDate">Fecha fin</param>
        /// <returns>Reporte de impuestos por periodo</returns>
        VMRImpuestosPorPeriodo ListTaxesPerPeriod(DateTime startDate, DateTime endDate);
    }
}
