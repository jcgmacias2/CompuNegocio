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
    public partial class InvoiceNoteView : BaseView, IInvoiceNoteView
    {
        public event Action Quit;

        public InvoiceNoteView(bool readOnly = false)
        {
            InitializeComponent();
            BindComponents();

            txtNota.IsReadOnly = readOnly;
        }

        private void BindComponents()
        {
            btnGuardar.Click += BtnGuardarOnClick;
        }

        private void BtnGuardarOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Quit.isValid())
            {
                Quit();
            }
        }

        public void Show(DatosExtraPorFactura datos)
        {
            txtNota.Text = datos.valor;
        }

        public DatosExtraPorFactura Nota {
            get { return new DatosExtraPorFactura() {valor = txtNota.Text,dato = DatoExtra.Nota.ToString()}; }
        }
    }
}
