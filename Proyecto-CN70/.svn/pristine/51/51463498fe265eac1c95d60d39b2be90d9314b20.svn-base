using System;

namespace Aprovi.Views
{
    public interface IItemsHomologationToolView : IBaseView
    {
        event Action Quit;
        event Action ProcessItems;
        event Action OpenFindXls;

        string ExcelFile { get; }
        string CNStartCell { get; }
        string SATStartCell { get; }

        void Fill(string excelFile);
    }
}
