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

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for GuardianManualSendView.xaml
    /// </summary>
    public partial class GuardianManualSendView : BaseView, IGuardianManualSendView
    {
        public event Action Load;
        public event Action Quit;
        public event Action Send;

        public GuardianManualSendView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            this.Loaded += GuardianManualSendView_Loaded;
            btnCerrar.Click += BtnCerrar_Click;
            btnEnviar.Click += BtnEnviar_Click;
        }

        private void GuardianManualSendView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Load.isValid())
                Load();
        }

        private void BtnEnviar_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (Send.isValid())
                Send();
            Mouse.OverrideCursor = null;
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public List<ComprobantesEnviado> Pending => dgPendientes.Items.Count > 0 ? dgPendientes.ItemsSource.Cast<ComprobantesEnviado>().ToList() : null;

        public void Fill(List<ComprobantesEnviado> pending)
        {
            dgPendientes.ItemsSource = pending;
        }
    }
}
