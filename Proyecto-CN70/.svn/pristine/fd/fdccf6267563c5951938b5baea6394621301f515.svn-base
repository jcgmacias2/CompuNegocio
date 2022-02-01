using Aprovi.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;

namespace Aprovi.Views
{
    public interface IQuotePrintView : IBaseView
    {
        event Action FindLast;
        event Action Find;
        event Action OpenList;
        event Action Quit;
        event Action Preview;
        event Action Print;
        event Action SendEmail;

        VMCotizacion Quote { get; }
        Opciones_Envio_Correo EmailOption { get; }
        string GivenEmail { get; }

        void Show(VMCotizacion quote);
    }
}
