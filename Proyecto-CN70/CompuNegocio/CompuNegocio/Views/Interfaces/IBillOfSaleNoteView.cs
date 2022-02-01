using System;
using Aprovi.Data.Models;

namespace Aprovi.Views
{
    public interface IBillOfSaleNoteView : IBaseView
    {
        event Action Quit;

        DatosExtraPorRemision Nota { get; }

        void Show(DatosExtraPorRemision nota);
    }
}