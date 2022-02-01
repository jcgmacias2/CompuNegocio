using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IConfigurationFormatosView
    {
        event Action FilterFormats;
        event Action AddOrUpdateFormat;

        Reporte SelectedReport { get; }
        FormatosPorConfiguracion Format { get; }

        void FillCombo(List<Formato> formats);
        void Show(List<FormatosPorConfiguracion> formats);
    }
}
