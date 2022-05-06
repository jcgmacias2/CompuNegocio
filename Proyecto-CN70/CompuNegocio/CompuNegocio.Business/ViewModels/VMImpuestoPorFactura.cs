using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMImpuestoPorFactura
    {
        public VMImpuestoPorFactura() { }

        public VMImpuestoPorFactura(ImpuestoPorFactura impuesto, AbonosDeFactura abono, string tipo, decimal base_imp)
        {
            this.Tipo = tipo;
            this.Base = base_imp.ToStringRoundedCurrency(abono.Moneda);
            this.Impuesto = impuesto.codigoImpuesto;
            this.Tipo_Factor = impuesto.codigoTipoFactor;
            this.Tasa_Cuota = Math.Abs(impuesto.valorTasaOCuaota).ToTdCFDI_Importe();
            this.Importe = (base_imp * impuesto.importe).ToStringRoundedCurrency(abono.Moneda);
        }

        public string Tipo { get; set; }
        public string Base { get; set; }
        public string Impuesto { get; set; }
        public string Tipo_Factor { get; set; }
        public string Tasa_Cuota { get; set; }
        public string Importe { get; set; }
    }
}
