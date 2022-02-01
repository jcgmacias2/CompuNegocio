using System;

namespace Aprovi.Views
{
    public interface IMigrationToolsView : IBaseView
    {
        event Action Quit;
        event Action Process;
        event Action OpenFindDbc;

        bool Items { get; }
        bool Clients { get; }
        bool Suppliers { get; }
        string DbcPath { get; }

        void Show(string dbcPath);

    }
}
