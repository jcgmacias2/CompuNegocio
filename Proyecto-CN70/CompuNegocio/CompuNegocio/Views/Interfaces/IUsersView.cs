using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IUsersView : IBaseView
    {

        event Action Quit;
        event Action New;
        event Action Find;
        event Action Delete;
        event Action Save;
        event Action Update;
        event Action OpenList;
        event Action OpenPrivileges;
        event Action AddCommission;
        event Action RemoveCommission;

        Usuario User { get; }
        ComisionesPorUsuario CurrentCommission { get; }
        List<ComisionesPorUsuario> Comissions { get; }
        ComisionesPorUsuario SelectedComission { get; }
        bool IsDirty { get; }

        void Show(Usuario user);
        void Show(List<ComisionesPorUsuario> userCommissions);
        void FillCombos(List<TiposDeComision> commissions);
        void Clear();
        void ClearCommission();
    }
}
