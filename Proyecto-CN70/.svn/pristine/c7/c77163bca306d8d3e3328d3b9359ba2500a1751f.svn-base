using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IGuardianConfigurationView
    {
        event Action AddAccount;
        event Action RemoveAccount;

        CuentasGuardian Account { get; }
        CuentasGuardian Selected { get; }

        void Fill(List<CuentasGuardian> accounts);
    }
}
