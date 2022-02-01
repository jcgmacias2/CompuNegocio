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
using Aprovi.Application.Helpers;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for ScaleItemsTransferView.xaml
    /// </summary>
    public partial class ScaleItemsTransferView : BaseView, IScaleItemsTransferView
    {
        public event Action Quit;
        public event Action Transfer;

        public ScaleItemsTransferView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnTransferir.Click += btnTransferir_Click;
        }

        void btnTransferir_Click(object sender, RoutedEventArgs e)
        {
            if (Transfer.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "ItemsPresenter", true))
                Transfer();
        }

        void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public Clasificacione Classification
        {
            get { return cmbClasificaciones.SelectedIndex >= 0 ? (Clasificacione)cmbClasificaciones.SelectedItem : new Clasificacione(); }
        }

        public void FillCombo(List<Clasificacione> classifications)
        {
            cmbClasificaciones.Items.Clear();
            cmbClasificaciones.ItemsSource = classifications;
            cmbClasificaciones.DisplayMemberPath = "descripcion";
            cmbClasificaciones.SelectedValuePath = "idClasificacion";
        }
    }
}
