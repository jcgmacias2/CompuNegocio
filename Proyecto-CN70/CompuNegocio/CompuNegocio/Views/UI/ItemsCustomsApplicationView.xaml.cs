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
using Aprovi.Business.ViewModels;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for ItemsCustomsApplicationView.xaml
    /// </summary>
    public partial class ItemsCustomsApplicationView : BaseView, IItemsCustomsApplicationView
    {
        public event Action Add;
        public event Action Remove;
        public event Action Save;

        private VMArticulo _item;

        public ItemsCustomsApplicationView(VMArticulo item)
        {
            InitializeComponent();
            BindComponents();
            txtUnidades.Text = item.Existencia.ToDecimalString();
            _item = item;
        }

        private void BindComponents()
        {
            btnAddCustomApplication.Click += BtnAddCustomApplication_Click;
            dgPedimentos.PreviewKeyUp += DgPedimentos_PreviewKeyUp;
            btnGuardar.Click += BtnGuardar_Click;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (Save.isValid())
                Save();
        }

        private void DgPedimentos_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Delete) && Remove.isValid())
                Remove();
        }

        private void BtnAddCustomApplication_Click(object sender, RoutedEventArgs e)
        {
            if (Add.isValid())
                Add();
        }

        public List<VMPedimento> CustomsApplications => dgPedimentos.ItemsSource.isValid() ? dgPedimentos.ItemsSource.Cast<VMPedimento>().ToList() : new List<VMPedimento>();

        public VMPedimento Current => new VMPedimento()
        {
            añoOperacion = txtAñoOperacion.Text,
            aduana = txtAduana.Text,
            patente = txtPatente.Text,
            añoEnCurso = txtAñoEnCurso.Text,
            progresivo = txtProgesivo.Text,
            Unidades = txtUnidades.Text.ToDecimalOrDefault(),
            fecha = dpFecha.SelectedDate.Value
        };

        public VMPedimento Selected => dgPedimentos.SelectedIndex >= 0 ? (VMPedimento)dgPedimentos.SelectedItem : null;

        public VMArticulo Item => _item;

        public void Clear()
        {
            txtAñoOperacion.Clear();
            txtAduana.Clear();
            txtPatente.Clear();
            txtAñoEnCurso.Clear();
            txtProgesivo.Clear();
            txtUnidades.Clear();            
        }

        public void Show(List<VMPedimento> customsApplications)
        {
            dgPedimentos.ItemsSource = customsApplications;
        }
    }
}
