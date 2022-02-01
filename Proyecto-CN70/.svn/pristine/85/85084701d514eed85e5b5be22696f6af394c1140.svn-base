using Aprovi.Business.Helpers;
using System;
using System.Windows.Data;
using System.Collections.Generic;
using Aprovi.Data.Models;
using System.Linq;

namespace Aprovi.Application.Helpers
{
    public class PrecioValueConverter : IMultiValueConverter

    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return Operations.CalculatePriceWithoutTaxes(decimal.Parse(values[0].ToString()), ((ICollection<Precio>)values[1]).First().utilidad);
            }
            catch (Exception)
            {
                throw new Exception("Alguno de los artículos no contiene todos los precios definidos");
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
