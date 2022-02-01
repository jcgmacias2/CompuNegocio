using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IConfigurationPACView : IBaseView
    {
        event Action Quit;
        event Action Save;

        Configuracion Config { get; }
    }
}
