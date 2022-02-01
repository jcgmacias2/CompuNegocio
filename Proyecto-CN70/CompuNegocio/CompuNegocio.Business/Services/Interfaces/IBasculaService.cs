using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IBasculaService
    {
        /// <summary>
        /// Inicializa la báscula default según la estación configurada
        /// </summary>
        /// <returns>Báscula lista para utilizarse</returns>
        Bascula GetDefault(Estacione station);

        /// <summary>
        /// Actualiza la configuración de una báscula
        /// </summary>
        /// <param name="scale">Báscula con datos actualizados</param>
        /// <returns>Báscula registrada</returns>
        Bascula Update(Bascula scale);

        /// <summary>
        /// Actualiza la lista de PLU's en una báscula
        /// </summary>
        /// <param name="scale">Báscula a utilizar</param>
        /// <param name="items">Lista de articulos a actualizar</param>
        void UpdatePLUs(Bascula scale, List<Articulo> items);

        /// <summary>
        /// Guarda una lista de PLU's en una báscula
        /// </summary>
        /// <param name="scale">Báscula a utilizar</param>
        /// <param name="items">Lista de artículos a actualizar</param>
        string WritePLUs(Bascula scale, List<Articulo> items);

        /// <summary>
        /// Tests the communication to the scale using the scale settings provided
        /// </summary>
        /// <param name="scale">Scale settings to be used</param>
        /// <returns>True if communication is ready</returns>
        bool IsReady(Bascula scale);
    }
}
