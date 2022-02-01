using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IAplicacionesRepository
    {
        object ReadSetting(string key);

        void UpdateSetting(string key, string value);

        string GetComputerCode();

        void UpdateConnectionString(string name, string value);
    }
}
