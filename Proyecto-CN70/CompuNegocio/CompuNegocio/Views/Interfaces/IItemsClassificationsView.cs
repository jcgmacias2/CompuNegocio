using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IItemsClassificationsView
    {
        event Action AddClassification;
        event Action DeleteClassification;

        Clasificacione CurrentClassification { get; }

        void Show(List<Clasificacione> clasifications);
    }
}
