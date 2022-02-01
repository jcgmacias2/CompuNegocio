using Aprovi.Business.ViewModels;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface ICustomsApplicationsExitView : IBaseView
    {
        event Action Save;
        event Action Add;
        event Action AutoFill;
        event Action Remove;
        event Action ShowAvailable;

        List<VMArticulosConPedimento> Items { get; }

        VMArticulosConPedimento Selected { get; }

        VMPedimentoDisponible AvailableSelected { get; }

        List<VMPedimentoDisponible> Availables { get; }

        VMPedimentoAsociado AssociatedSelected { get; }

        void Show(List<VMArticulosConPedimento> items);
        void Show(List<VMPedimentoDisponible> available);
        void Show(List<VMPedimentoAsociado> associated);
        void Show(VMArticulosConPedimento item);
    }
}
