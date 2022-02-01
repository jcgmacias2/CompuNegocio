using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IGuardianClientView
    {
        event Action AddAccount;
        event Action RemoveAccount;

        CuentasDeCorreo Account { get; }
        CuentasDeCorreo Selected { get; }

        void Fill(List<CuentasDeCorreo> accounts);
    }
}
