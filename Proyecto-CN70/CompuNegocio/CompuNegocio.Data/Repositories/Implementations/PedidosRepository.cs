using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class PedidosRepository : BaseRepository<Pedido>, IPedidosRepository
    {
        public PedidosRepository(CNEntities context) : base(context) { }

        public int Next()
        {
            try
            {
                var pedido = _dbSet.OrderByDescending(r => r.folio).FirstOrDefault();
                if(pedido.isValid())
                    return pedido.folio + 1;
                else
                    return 1;
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
                var pedido = _dbSet.OrderByDescending(r => r.folio).FirstOrDefault();
                if (pedido.isValid())
                    return pedido.folio;
                else
                    return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Pedido Find(int folio)
        {
            try
            {
                return _dbSet.FirstOrDefault(r => r.folio.Equals(folio));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Pedido FindById(int id)
        {
            try
            {
                return _dbSet.FirstOrDefault(r => r.idPedido.Equals(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Pedido> List(int? idEstatus)
        {
            try
            {
                if (idEstatus.HasValue)
                    return _dbSet.Where(r => r.idEstatusDePedido.Equals(idEstatus)).ToList();
                else
                    return _dbSet.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Pedido> WithFolioOrClientLike(string value, int? idEstatus)
        {
            try
            {
                if (idEstatus.HasValue)
                    return _dbSet.Where(r => r.idEstatusDePedido.Equals(idEstatus)).Where(r => r.folio.ToString().Contains(value) || r.Cliente.razonSocial.Contains(value)).ToList();
                else
                    return _dbSet.Where(r => r.folio.ToString().Contains(value) || r.Cliente.razonSocial.Contains(value)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Pedido> WithStatus(StatusDePedido status)
        {
            try
            {
                return _dbSet.Where(x => x.idEstatusDePedido == (int) status).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Pedido> WithCustomer(Cliente customer)
        {
            try
            {
                return _dbSet.Where(x => x.idCliente == customer.idCliente).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDetail(DetallesDePedido detail)
        {
            var detailDb = _context.DetallesDePedidoes.Find(detail.idDetalleDePedido);

            //Si tiene impuestos, se eliminan
            var imp = detailDb.Impuestos.ToList();
            foreach (var i in imp)
            {
                detailDb.Impuestos.Remove(i);
            }

            _context.DetallesDePedidoes.Remove(detailDb);
        }
    }
}
