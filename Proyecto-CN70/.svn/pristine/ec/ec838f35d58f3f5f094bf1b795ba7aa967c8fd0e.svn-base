using Aprovi.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for CustomsApplicationsExitView.xaml
    /// </summary>
    public partial class CustomsApplicationsExitView : BaseView, ICustomsApplicationsExitView
    {
        public event Action Save;
        public event Action Add;
        public event Action AutoFill;
        public event Action Remove;
        public event Action ShowAvailable;

        public CustomsApplicationsExitView(List<VMArticulosConPedimento> items)
        {
            InitializeComponent();
            BindComponents();
            Show(items);
        }

        private void BindComponents()
        {
            btnAsociar.Click += BtnAsociar_Click;
            btnAsociarTodo.Click += BtnAsociarTodo_Click;
            btnDesasociar.Click += BtnDesasociar_Click;
            btnGuardar.Click += BtnGuardar_Click;
            dgArticulos.MouseDoubleClick += DgArticulos_MouseDoubleClick;
        }

        private void DgArticulos_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ShowAvailable.isValid())
                ShowAvailable();
        }

        private void BtnGuardar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Save.isValid())
                Save();
        }

        private void BtnDesasociar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Remove.isValid())
                Remove();
        }

        private void BtnAsociarTodo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (AutoFill.isValid())
                AutoFill();
        }

        private void BtnAsociar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Add.isValid())
                Add();
        }

        public List<VMArticulosConPedimento> Items => dgArticulos.ItemsSource.Cast<VMArticulosConPedimento>().ToList();

        public VMArticulosConPedimento Selected => dgArticulos.SelectedIndex >= 0 ? (VMArticulosConPedimento)dgArticulos.SelectedItem : null;

        public VMPedimentoDisponible AvailableSelected => dgPedimentos.SelectedIndex >= 0 ? (VMPedimentoDisponible)dgPedimentos.SelectedItem : null;

        public List<VMPedimentoDisponible> Availables => dgPedimentos.ItemsSource.Cast<VMPedimentoDisponible>().ToList();

        public VMPedimentoAsociado AssociatedSelected => dgAsociaciones.SelectedIndex >= 0 ? (VMPedimentoAsociado)dgAsociaciones.SelectedItem : null;

        public void Show(List<VMArticulosConPedimento> items)
        {
            dgArticulos.ItemsSource = items;
        }

        public void Show(List<VMPedimentoDisponible> available)
        {
            dgPedimentos.ItemsSource = available;
        }

        public void Show(List<VMPedimentoAsociado> associated)
        {
            dgAsociaciones.ItemsSource = associated;
        }

        public void Show(VMArticulosConPedimento item)
        {
            var index = dgArticulos.SelectedIndex;
            var updatedItems = dgArticulos.ItemsSource.Cast<VMArticulosConPedimento>();
            dgArticulos.ItemsSource = null;
            dgArticulos.ItemsSource = updatedItems;
            dgArticulos.SelectedIndex = index;
            dgAsociaciones.ItemsSource = null;
            dgAsociaciones.ItemsSource = item.Pedimentos;
        }
    }
}
