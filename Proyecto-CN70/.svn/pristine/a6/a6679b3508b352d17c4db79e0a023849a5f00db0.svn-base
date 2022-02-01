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
    public partial class BillOfSaleNoteView : BaseView, IBillOfSaleNoteView
    {
        public event Action Quit;

        private int _idRemision;

        public BillOfSaleNoteView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnGuardar.Click += BtnGuardar_Click;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public void Show(DatosExtraPorRemision datos)
        {
            _idRemision = datos.idRemision;

            txtNota.Text = datos.valor;
        }

        public DatosExtraPorRemision Nota {
            get { return new DatosExtraPorRemision() {idRemision = _idRemision,valor = txtNota.Text,dato = DatoExtra.Nota.ToString()}; }
        }
    }
}
