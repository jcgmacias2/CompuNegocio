using Aprovi.Application.ViewModels;
using Aprovi.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IConfigurationCSDView : IBaseView
    {
        event Action Quit;
        event Action CreateAndSave;
        event Action Save;

        event Action OpenFindCertificate;
        event Action OpenFindPrivateKey;
        event Action OpenPfxFolder;

        event Action OpenFindPfx;

        VMCertificado Certificate { get; }

        void Show(VMCertificado certificate);

    }
}
