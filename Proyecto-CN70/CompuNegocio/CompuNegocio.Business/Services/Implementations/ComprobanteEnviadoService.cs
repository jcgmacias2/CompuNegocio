using System;
using System.Collections.Generic;
using Aprovi.Data.Models;
using Aprovi.Data.Core;
using Aprovi.Data.Repositories;

namespace Aprovi.Business.Services
{
    public class ComprobanteEnviadoService : IComprobanteEnviadoService
    {
        private IUnitOfWork _UOW;
        private IComprobantesEnviadosRepository _sentReceipts;

        public ComprobanteEnviadoService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _sentReceipts = _UOW.ComprobantesEnviados;
        }

        public ComprobantesEnviado Find(int id)
        {
            try
            {
                return _sentReceipts.Find(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ComprobantesEnviado> List(bool onlyPending)
        {
            try
            {
                return _sentReceipts.List(onlyPending);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
