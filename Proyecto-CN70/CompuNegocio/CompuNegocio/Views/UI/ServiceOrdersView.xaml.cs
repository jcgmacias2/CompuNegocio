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
    /// Interaction logic for ServiceOrdersView.xaml
    /// </summary>
    public partial class ServiceOrdersView : BaseView
    {
        public ServiceOrdersView()
        {
            InitializeComponent();
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            TechnicianSelectionView view = new TechnicianSelectionView();
            view.ShowDialog();
        }
    }
}
