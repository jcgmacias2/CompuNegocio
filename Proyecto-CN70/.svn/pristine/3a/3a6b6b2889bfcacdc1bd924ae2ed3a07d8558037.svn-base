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
    public partial class CancellationView : BaseView, ICancellationView
    {
        public event Action Quit;

        public CancellationView()
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

        public string Reason {
            get { return txtMotivo.Text; }
        }
    }
}
