using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IPrivilegesView : IBaseView
    {
        event Action Quit;
        event Action Add;
        event Action Delete;

        Usuario User { get; }
        Privilegio Privilege { get; }

        int CurrentRecord { get; }
        Privilegio CurrentPrivilege { get; }

        void Show(List<Privilegio> privileges);
        void Clear();
        void FillCombos(List<Pantalla> views, List<Permiso> permits);
    }
}
