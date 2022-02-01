using Aprovi.Data.Models;
using System;

namespace Aprovi.Views
{
    public interface ICustomsApplicationView : IBaseView
    {
        event Action Save;
        
        Pedimento CustomsApplication { get; }

        void Fill(Pedimento customsApplication);
    }
}
