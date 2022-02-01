using Aprovi.Application.ViewModels;
using Aprovi.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IAuthenticationView : IBaseView
    {
        event Action Quit;
        event Action SignIn;
        event Action AuthorizeOnAPI;

        bool ApiAuthentication { get; }
        VMCredencial Credentials { get; }

        void Show(VMCredencial credentials);
    }
}
