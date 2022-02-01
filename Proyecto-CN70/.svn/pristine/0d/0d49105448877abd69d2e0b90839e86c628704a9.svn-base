using Aprovi.Data.Models;
using System;
using System.Collections.Generic;


namespace Aprovi.Views
{
    public interface IGuardianManualSendView : IBaseView
    {
        event Action Load;
        event Action Quit;
        event Action Send;

        List<ComprobantesEnviado> Pending { get; }

        void Fill(List<ComprobantesEnviado> pending);
    }
}
