using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IPedimentoService
    {
        Pedimento Add(Pedimento customsApproval);

        List<VwExistenciasConPedimento> List(int idArticulo);

        VwExistenciasConPedimento Find(int idArticulo, int idPedimento);

        Pedimento Find(int idPedimento);

        Pedimento FindByDetails(Pedimento customsApproval);
    }
}
