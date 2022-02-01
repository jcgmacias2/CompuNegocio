using Aprovi.Data.Models;

namespace Aprovi.Business.Services
{
    public interface ICodigoDeArticuloPorProveedorService
    {
        CodigosDeArticuloPorProveedor Add(CodigosDeArticuloPorProveedor code);

        CodigosDeArticuloPorProveedor Update(CodigosDeArticuloPorProveedor code);

        void Remove(CodigosDeArticuloPorProveedor code);

    }
}
