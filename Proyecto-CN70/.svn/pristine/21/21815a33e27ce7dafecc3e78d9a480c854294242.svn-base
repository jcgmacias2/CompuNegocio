using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IConfigurationRegimeView : IBaseView
    {
        event Action AddRegime;
        event Action DeleteRegime;

        Regimene Regime { get; }
        Regimene CurrentRegime { get; }

        void Show(List<Regimene> regimes);
        void ClearRegime();
    }
}
