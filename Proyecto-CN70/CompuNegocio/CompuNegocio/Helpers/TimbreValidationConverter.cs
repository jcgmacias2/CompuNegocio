using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Aprovi.Application.Helpers
{
    public class TimbreValidationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //Si tiene un TimbreDeAbonosDeFactura con UUID es SI
                if (values[0].isValid() && ((TimbresDeAbonosDeFactura)values[0]).UUID.isValid())
                    return "Si";

                //Si tiene un Pago válido aun cuando no tiene TimbreDeAbono corresponde aun Múltiple
                if (values[1].isValid() && ((Pago)values[1]).idPago.isValid())
                    return string.Format("{0}-{1}{2}", ((Pago)values[1]).TimbresDePago.isValid() ? "Si" : "No", ((Pago)values[1]).serie, ((Pago)values[1]).folio);
                
                //Si no encontró ninguna de las dos entonces es un No
                return "No";
            }
            catch (Exception)
            {
                throw new Exception("Falló la validación del timbre");
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
