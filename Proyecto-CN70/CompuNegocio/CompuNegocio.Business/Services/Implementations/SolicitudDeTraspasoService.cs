using Aprovi.Data.Core;
using Aprovi.Data.Repositories;
using Aprovi.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.Helpers;
using Aprovi.Data.Models;

namespace Aprovi.Business.Services
{
    public abstract class SolicitudDeTraspasoService : ISolicitudDeTraspasoService
    {
        private IUnitOfWork _UOW;
        private IEmpresaAsociadaService _associatedCompanies;
        private ISolicitudesDeTraspasoRepository _transferRequests;
        private IArticulosRepository _items;
        
        public SolicitudDeTraspasoService(IUnitOfWork unitOfWork, IEmpresaAsociadaService associatedCompanies)
        {
            _UOW = unitOfWork;
            _associatedCompanies = associatedCompanies;
            _transferRequests = _UOW.SolicitudesDeTraspaso;
            _items = _UOW.Articulos;
        }

        public int Next()
        {
            try
            {
                return _transferRequests.Next();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Last()
        {
            try
            {
                return _transferRequests.Last();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual SolicitudesDeTraspaso Add(Traspaso transfer)
        {
            try
            {
                var request = new SolicitudesDeTraspaso();

                //Se debe buscar la empresa asociada de la base de datos local
                request.idEmpresaAsociadaOrigen = _associatedCompanies.FindByDatabaseName(transfer.EmpresasAsociada1.baseDeDatos).idEmpresaAsociada;
                request.idTraspaso = transfer.idTraspaso;
                request.folio = transfer.folio;

                request.fechaHora = DateTime.Now;

                //Ahora si guardo
                _transferRequests.Add(request);
                _UOW.Save();

                return request;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SolicitudesDeTraspaso FindById(int idSourceAssociatedCompany, int idTransfer)
        {
            try
            {
                return _transferRequests.FindById(idSourceAssociatedCompany, idTransfer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SolicitudesDeTraspaso FindByFolio(int folio)
        {
            try
            {
                return _transferRequests.Find(folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SolicitudesDeTraspaso> List()
        {
            try
            {
                return _transferRequests.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SolicitudesDeTraspaso> WithFolioOrCompanyLike(string value)
        {
            try
            {
                return _transferRequests.WithFolioOrCompanyLike(value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(SolicitudesDeTraspaso transferRequest)
        {
            try
            {
                _transferRequests.Remove(transferRequest);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
