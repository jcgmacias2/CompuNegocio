using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;

namespace Aprovi.Views
{
    public interface ICompanyStatusReportView : IBaseView
    {
        event Action Quit;
        event Action Print;
        event Action Preview;
        event Action Load;
        event Action FilterChanged;

        VMEstadoDeLaEmpresa Report { get; }

        void Show(VMEstadoDeLaEmpresa vm);
    }
}
