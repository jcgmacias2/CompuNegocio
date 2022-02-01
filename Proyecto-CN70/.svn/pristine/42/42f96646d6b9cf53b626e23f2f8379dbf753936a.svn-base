using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface ITraspasoService
    {
        /// <summary>
        /// Proporciona el siguiente folio de traspaso
        /// </summary>
        /// <returns>Folio asignado</returns>
        int Next();

        /// <summary>
        /// Proporciona el último folio utilizado en traspasos
        /// </summary>
        /// <returns>Folio utilizado</returns>
        int Last();

        /// <summary>
        /// Agrega un traspaso a la colección de traspasos existentes
        /// </summary>
        /// <param name="transfer">Traspaso a registrar</param>
        /// <returns>Traspaso registrado</returns>
        Traspaso Add(Traspaso transfer);

        /// <summary>
        /// Busca un traspaso a partir de su identificador numérico
        /// </summary>
        /// <param name="idTransfer">Identificador numérico del traspaso</param>
        /// <returns>Traspaso que corresponde al identificador</returns>
        Traspaso FindById(int idTransfer);

        /// <summary>
        /// Busca un traspaso a partir de su identificador numero, precargando todos los datos necesarios para un traspaso
        /// </summary>
        /// <param name="idTransfer">Identificador numero del traspaso</param>
        /// <returns>Traspaso que corresponde al identificador</returns>
        Traspaso FindByIdForTransfer(int idTransfer);

        /// <summary>
        /// Busca un traspaso a partir de su  folio
        /// </summary>
        /// <param name="folio">Folio del traspaso que se busca</param>
        /// <returns>Traspaso que corresponda con el folio</returns>
        Traspaso FindByFolio(int folio);

        /// <summary>
        /// Enlista todos los traspasos existentes
        /// </summary>
        /// <returns>Lista de traspasos</returns>
        List<Traspaso> List();

        /// <summary>
        /// Enlista todos los traspasos que coinciden total o parcialmente en su folio, o nombre de la empresa asociada, con el valor que se especifíca
        /// </summary>
        /// <param name="value">Valor a buscar en coincidencia</param>
        /// <returns>Lista de traspasos que coincidan con la búsqueda</returns>
        List<Traspaso> WithFolioOrCompanyLike(string value);

        /// <summary>
        /// Rechaza un traspaso
        /// </summary>
        /// <param name="traspaso">Traspaso a cancelar</param>
        /// <returns>Traspaso cancelado</returns>
        Traspaso Reject(VMTraspaso transfer);

        /// <summary>
        /// Actualiza el detalle de un traspaso
        /// </summary>
        /// <param name="transfer">Traspaso a actualizar</param>
        /// <param name="detail">Detalle nuevo</param>
        /// <returns>Traspaso actualizado</returns>
        Traspaso Update(Traspaso transfer, List<DetallesDeTraspaso> detail);

        /// <summary>
        /// Actualiza un traspaso
        /// </summary>
        /// <param name="transfer">Traspaso a actualizar</param>
        /// <returns>Traspaso actualizado</returns>
        Traspaso Update(Traspaso transfer);

        /// <summary>
        /// Elimina un detalle de traspaso
        /// </summary>
        /// <param name="transferDetail">Detalle de traspaso a eliminar</param>
        void DeleteDetail(DetallesDeTraspaso transferDetail);

        /// <summary>
        /// Aprueba un traspaso remoto
        /// </summary>
        /// <param name="remoteTransfer">Traspaso a aprobar</param>
        /// <returns>Traspaso registrado localmente</returns>
        Traspaso Approve(Traspaso remoteTransfer);

        /// <summary>
        /// Aplica los cambios al traspaso local que aplican a un traspaso entrante
        /// </summary>
        /// <param name="remoteTransfer">traspaso remoto</param>
        /// <param name="localDetail">Detalle de traspaso a integrar</param>
        /// <returns>Traspaso efectuado</returns>
        Traspaso EndRemoteIntegration(Traspaso localTransfer, Traspaso remoteTransfer);

        /// <summary>
        /// Aplica los cambios al traspaso local que aplican a un traspaso saliente
        /// </summary>
        /// <param name="remoteTransfer">traspaso remoto</param>
        /// <param name="localDetail">Detalle de traspaso a integrar</param>
        /// <returns>Traspaso efectuado</returns>
        Traspaso EndLocalIntegration(Traspaso localTransfer, Traspaso remoteTransfer);

        /// <summary>
        /// Enlista traspasos que cumplan con los filtros proporcionados
        /// </summary>
        /// <param name="startDate">Fecha de inicio</param>
        /// <param name="endDate">Fecha fin</param>
        /// <param name="originCompany">Empresa asociada origen</param>
        /// <param name="destinationCompany">Empresa asociada destino</param>
        /// <returns>traspasos filtrados</returns>
        List<VMRTraspaso> ListForReport(DateTime startDate, DateTime endDate, EmpresasAsociada originCompany, EmpresasAsociada destinationCompany);
    }
}
