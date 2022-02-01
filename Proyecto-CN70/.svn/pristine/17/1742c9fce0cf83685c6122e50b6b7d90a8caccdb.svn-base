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
    public partial class AddendaJardinesView : BaseView, IAddendaJardinesView
    {
        public event Action Close;

        public AddendaJardinesView()
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

        public List<DatosExtraPorFactura> DatosExtra
        {
            get { return new List<DatosExtraPorFactura>() {new DatosExtraPorFactura() { valor = txtNumeroRM.Text, dato = DatoExtra.NumeroRM.ToString()}}; }
        }
    }
}
