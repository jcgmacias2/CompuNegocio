using Aprovi.Data.Models;
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

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for ReceiptTypeView.xaml
    /// </summary>
    public partial class ReceiptTypeView : BaseView, IReceiptTypeView
    {
        public event Action Save;
        public event Action Quit;

        public ReceiptTypeView(Series serie)
        {
            InitializeComponent();
            Show(serie);
            BindComponents();
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnGuardar.Click += btnGuardar_Click;
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (Save.isValid())
                Save();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public TiposDeComprobante ReceiptType
        {
            get { return (TiposDeComprobante)cmbTiposDeComprobante.SelectedItem; }
        }

        public bool Selected
        {
            get;
            set;
        }

        public void Show(Series serie)
        {
            txtSerie.Text = serie.identificador;
        }

        public void FillCombo(List<TiposDeComprobante> receiptTypes)
        {
            cmbTiposDeComprobante.ItemsSource = receiptTypes;
            cmbTiposDeComprobante.SelectedValuePath = "idTipoDeComprobante";
            cmbTiposDeComprobante.DisplayMemberPath = "descripcion";
        }
    }
}
