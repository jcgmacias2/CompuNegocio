using System;
using System.Collections.Generic;
using System.Linq;
using Aprovi.Data.Models;
using Aprovi.Data.Core;
using Aprovi.Data.Repositories;

namespace Aprovi.Business.Services
{
    public class PedimentoService : IPedimentoService
    {
        private IUnitOfWork _UOW;
        private IViewPedimentosRepository _pedimentosPorArticulo;
        private IPedimentosRepository _pedimentos;

        public PedimentoService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _pedimentosPorArticulo = _UOW.PedimentosPorArticulo;
            _pedimentos = _UOW.Pedimentos;
        }

        public VwExistenciasConPedimento Find(int idArticulo, int idPedimento)
        {
            try
            {
                return _pedimentosPorArticulo.Find(idArticulo, idPedimento);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Pedimento Find(int idPedimento)
        {
            try
            {
                return _pedimentos.Find(idPedimento);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Pedimento FindByDetails(Pedimento customsApproval)
        {
            try
            {
                return _pedimentos.FindByDetails(customsApproval);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Pedimento Add(Pedimento customsApproval)
        {
            try
            {
                var approval = _pedimentos.Add(customsApproval);
                _UOW.Save();
                return approval;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwExistenciasConPedimento> List(int idArticulo)
        {
            try
            {
                return _pedimentosPorArticulo.List(idArticulo);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
