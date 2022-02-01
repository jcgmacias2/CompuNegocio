using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IConfigurationSerieView
    {
        event Action AddSerie;
        event Action SelectSerie;
        event Action UpdateSerie;

        Series Serie { get; }
        Series CurrentSerie { get; }
        bool IsDirtySerie { get; }
        int CambioSerie { get; }

        void Show(List<Series> series);
        void Show(Series serie);
        void ClearSerie();
        void SetEdition(bool visible);
    }
}
