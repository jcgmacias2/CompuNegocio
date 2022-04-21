using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IConfigurationView : IBaseView, IConfigurationRegimeView, IConfigurationSerieView, IConfigurationFilesView, IGuardianConfigurationView, IConfigurationFormatosView
    {
        event Action Quit;
        event Action Save;
        event Action Load;
        event Action OpenConfigurationPAC;
        event Action OpenConfigurationCSD;
        event Action SelectLogoFile;

        Configuracion Configuration { get; }

        void Show(Configuracion configuration);
        void ShowImage(Uri imagePath);
        void FillCombo(List<Pais> countries, List<object> opcionesCosto, List<Reporte> reportes, List<Periodicidad> periodicidads);
        
    }
}
