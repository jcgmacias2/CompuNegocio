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
    /// Interaction logic for KardexReportView.xaml
    /// </summary>
    public partial class KardexReportView : BaseView, IKardexReportView
    {
        public event Action FindItem;
        public event Action OpenItemsList;
        public event Action Quit;
        public event Action Print;
        public event Action Preview;
        private Articulo _item;

        public KardexReportView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            txtArticulo.LostFocus += txtArticulo_LostFocus;
            btnListarArticulos.Click += btnListarArticulos_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;
            _item = new Articulo();
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            if (Print.isValid())
                Print();

            Mouse.OverrideCursor = null;
        }

        private void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            if (Preview.isValid())
                Preview();
            Mouse.OverrideCursor = null;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void btnListarArticulos_Click(object sender, RoutedEventArgs e)
        {
            if (OpenItemsList.isValid())
                OpenItemsList();
        }

        private void txtArticulo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindItem.isValid())
                FindItem();
        }

        public Articulo Item
        {
            get { _item.codigo = txtArticulo.Text; return _item; }
        }

        public DateTime Start
        {
            get { return dpFechaInicio.SelectedDate.GetValueOrDefault(DateTime.Now); }
        }

        public DateTime End
        {
            get { return dpFechaFinal.SelectedDate.GetValueOrDefault(DateTime.Now); }
        }

        public void Show(Articulo item)
        {
            txtArticulo.Text = item.codigo;
            _item = item;
        }
    }
}
