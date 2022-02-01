using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IProveedorService
    {
        /// <summary>
        /// Agrega un nuevo proveedor al catálogo
        /// </summary>
        /// <param name="supplier">Proveedor que se desea agregar</param>
        /// <returns>Proveedor registrado</returns>
        Proveedore Add(Proveedore supplier);

        /// <summary>
        /// Enlista los proveedores activos
        /// </summary>
        /// <returns>Lista de proveedores activos</returns>
        List<Proveedore> List();

        /// <summary>
        /// Enlista los proveedores que coinciden con el código que se busca
        /// </summary>
        /// <param name="code">Código que se desea buscar</param>
        /// <returns>Lista de proveedores en coincidencia</returns>
        List<Proveedore> WithCodeLike(string code);

        /// <summary>
        /// Busca un proveedor a partir de su identificador numérico
        /// </summary>
        /// <param name="idSupplier">Identficador numérico único perteneciente al proveedor</param>
        /// <returns>Proveedor al que corresponde el identificador</returns>
        Proveedore Find(int idSupplier);

        /// <summary>
        /// Busca un proveedor a partir de su código único
        /// </summary>
        /// <param name="code">Código único del proveedor que se busca</param>
        /// <returns>Proveedor al que pertenece el código buscado</returns>
        Proveedore Find(string code);

        /// <summary>
        /// Actualiza los datos de un proveedor
        /// </summary>
        /// <param name="supplier">Proveedor con los datos a modificar</param>
        /// <returns>Proveedor con cambios registrados</returns>
        Proveedore Update(Proveedore supplier);

        /// <summary>
        /// Valida si es posible eliminar un proveedor
        /// </summary>
        /// <param name="supplier">Proveedor a validar</param>
        /// <returns>True si es posible eliminarlo</returns>
        bool CanDelete(Proveedore supplier);

        /// <summary>
        /// Elimina un proveedor registrado
        /// </summary>
        /// <param name="supplier">Proveedor a eliminar</param>
        void Delete(Proveedore supplier);

        /// <summary>
        /// Verifica si el rfc que se le pasa ya existe
        /// </summary>
        /// <param name="rfc">Registro federal de contribuyente</param>
        /// <returns>True si existe</returns>
        bool Exist(string rfc);

        /// <summary>
        /// Importa los proveedores del contenedor que se le pasa hacia la base de datos local
        /// </summary>
        /// <param name="dbcPath">Archivo contenedor de las tablas de VPF (Bdd.dbc)</param>
        void Import(string dbcPath);
    }
}
