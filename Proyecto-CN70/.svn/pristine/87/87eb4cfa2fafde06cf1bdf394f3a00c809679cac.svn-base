using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IStationsView : IBaseView
    {
        event Action Find;
        event Action OpenList;
        event Action Quit;
        event Action New;
        event Action Delete;
        event Action Save;
        event Action Update;
        event Action AssociateStation;
        event Action DissociateStation;
        event Action ListPorts;

        Estacione Station { get; }
        bool IsDirty { get; }
        bool IsStationSet { get; }
        Configuracion Configuration { get; }

        void Show(Estacione station, Configuracion configuration);
        void Clear();
        void FillCombos(List<string> printers, List<Empresa> businesses, List<StopBits> bits, List<Parity> parities);
        void FillPorts(List<string> ports);
    }
}
