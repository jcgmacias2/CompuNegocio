using Aprovi.Data.Models;
using System.Windows;

namespace Aprovi.Application.Helpers
{
    public static class Session
    {
        //Cargado en el inicio de sesión
        public static Usuario LoggedUser { get; set; }
        public static bool IsUserLogged { get { return LoggedUser != null; } }
        public static Configuracion Configuration { get; set; }
        public static Estacione Station { get { return Configuration.Estacion; } }
        public static Series SerieFacturas { get; set; }
        public static Series SerieParcialidades { get; set; }
        public static Series SerieNotasDeCredito { get; set; }
    }
}
