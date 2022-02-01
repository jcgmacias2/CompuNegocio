using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Aprovi.Data.Models;
using Aprovi.Application.Helpers;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for PropertyAccountView.xaml
    /// </summary>
    public partial class AddendaComercialMexicanaView : BaseView, IAddendaComercialMexicanaView
    {
        public event Action Close;

        public AddendaComercialMexicanaView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnCerrar.Click += BtnCerrar_Click;
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Close.isValid())
                Close();
        }

        public void FillCombos(List<Directorio> directorios,List<Seccione> secciones)
        {
            cbSucursales.ItemsSource = directorios;
            cbSucursales.DisplayMemberPath = "nombre";
            cbSucursales.SelectedValuePath = "idDirectorio";

            cbSecciones.ItemsSource = secciones;
            cbSecciones.DisplayMemberPath = "nombre";
            cbSecciones.SelectedValuePath = "idSeccion";
        }

        public List<DatosExtraPorFactura> DatosExtra
        {
            get { return new List<DatosExtraPorFactura>() {new DatosExtraPorFactura() { valor = dpFechaEntrega.SelectedDate.ToDateStringOrDefault(), dato = DatoExtra.FechaDeEntrega.ToString()}, new DatosExtraPorFactura() { valor = cbSucursales.SelectedValue.ToStringOrDefault(), dato = DatoExtra.Sucursal.ToString() }, new DatosExtraPorFactura() { valor = cbSecciones.SelectedValue.ToStringOrDefault(), dato = DatoExtra.Seccion.ToString() } }; }
        }
    }
}
