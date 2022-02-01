using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface ISolicitudDeTraspasoService
    {
        /// <summary>
        /// Proporciona el siguiente folio de solicitud de traspaso
        /// </summary>
        /// <returns>Folio asignado</returns>
        int Next();

        /// <summary>
        /// Proporciona el último folio utilizado en solicitudes de traspasos
        /// </summary>
        /// <returns>Folio utilizado</returns>
        int Last();

        /// <summary>
        /// Agrega una solicitud de traspaso a la colección de solicitudes de traspasos existentes
        /// </summary>
        /// <param name="transferRequest">Traspaso al que se le generara la solicitud de traspaso</param>
        /// <returns>Solicitud de traspaso registrada</returns>
        SolicitudesDeTraspaso Add(Traspaso transfer);

        /// <summary>
        /// Busca una solicitud de traspaso a partir de su identificador numérico
        /// </summary>
        /// <param name="idTransfer">Identificador numérico del traspaso remoto</param>
        /// <param name="idSourceAssociatedCompany">Identificador numérico la empresa asociada remota</param>
        /// <returns>Solicitud de traspaso que corresponde al identificador</returns>
        SolicitudesDeTraspaso FindById(int idSourceAssociatedCompany, int idTransfer);

        /// <summary>
        /// Busca una solicitud de traspaso a partir de su folio
        /// </summary>
        /// <param name="folio">Folio de la solicitud de traspaso que se busca</param>
        /// <returns>Solicitud de traspaso que corresponda con el folio</returns>
        SolicitudesDeTraspaso FindByFolio(int folio);

        /// <summary>
        /// Enlista todas las solicitudes de traspasos existentes
        /// </summary>
        /// <returns>Lista de traspasos existentes</returns>
        List<SolicitudesDeTraspaso> List();

        /// <summary>
        /// Enlista todos las solicitudes de traspaso que coinciden total o parcialmente en su folio, o nombre de la empresa asociada, con el valor que se especifíca
        /// </summary>
        /// <param name="value">Valor a buscar en coincidencia</param>
        /// <returns>Lista de solicitudes de traspaso que coincidan con la búsqueda</returns>
        List<SolicitudesDeTraspaso> WithFolioOrCompanyLike(string value);

        /// <summary>
        /// Elimina una solicitud de traspaso
        /// </summary>
        /// <param name="transferRequest">solicitud de traspaso a eliminar</param>
        void Delete(SolicitudesDeTraspaso transferRequest);
    }
}
