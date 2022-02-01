using System;
using Aprovi.Data.Models;

namespace Aprovi.Views
{
    public interface INoteView : IBaseView
    {
        event Action Quit;

        string Nota { get; }

        void Show(string nota);
    }
}