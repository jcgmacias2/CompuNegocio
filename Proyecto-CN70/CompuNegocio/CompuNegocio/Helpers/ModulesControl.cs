using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Aprovi.Application.Helpers
{
    public static class ModulesControl
    {
        /// <summary>
        /// Establece la visibilidad de las opciones del menú en base al código del sistema cargado en sesión
        /// </summary>
        /// <param name="items">Lista de opciones a evaluar su visibilidad</param>
        public static void SetVisibility(IEnumerable<MenuItem> items)
        {
            foreach (var mni in items)
            {

                //Si el tag esta vacío y no tiene items me salgo
                if ((!mni.Tag.isValid() || !mni.Tag.ToString().isValid()) && !mni.HasItems)
                    continue;

                //Si el tag esta vacío pero tiene hijos se manda a llamar a si mismo y después se sale
                if ((!mni.Tag.isValid() || !mni.Tag.ToString().isValid()) && mni.HasItems)
                { 
                    SetVisibility(mni.Items.Cast<MenuItem>());
                    continue;
                }

                //Si llega aquí el tag tiene valor, lo paso como string
                var tag = mni.Tag.ToString();

                //Lo comparo con el valor del sistema.
                if (tag.Equals(Session.Configuration.Sistema.ToString()))
                    mni.Visibility = Visibility.Visible; 
                else
                    mni.Visibility = Visibility.Collapsed;

                //También debo compararlo con los módulos activos del sistema
                foreach (var module in Session.Configuration.Modulos)
                {
                    if(tag.Equals(module.ToString()))
                    { 
                        mni.Visibility = Visibility.Visible;
                        break;
                    }
                    else
                        mni.Visibility = Visibility.Collapsed;
                }

            }
        }
    }
}
