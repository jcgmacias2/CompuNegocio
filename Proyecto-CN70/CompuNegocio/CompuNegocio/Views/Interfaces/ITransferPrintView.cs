﻿using Aprovi.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface ITransferPrintView : IBaseView
    {
        event Action FindLast;
        event Action Find;
        event Action OpenList;
        event Action Quit;
        event Action Preview;
        event Action Print;

        VMTraspaso Transfer { get; }

        void Show(VMTraspaso transfer);
    }
}
