using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for Adjustments.xaml
    /// </summary>
    public partial class AdjustmentsView : BaseView, IAdjustmentsView
    {
        public event Action Find;
        public event Action OpenList;
        public event Action FindItem;
        public event Action OpenItemsList;
        public event Action AddItem;
        public event Action RemoveItem;
        public event Action SelectItem;
        public event Action New;
        public event Action Quit;
        public event Action Print;
        public event Action Save;
        
        private int _idAdjustment;
        private Articulo _currentItem;

        public AdjustmentsView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            txtFolio.LostFocus += txtFolio_LostFocus;
            btnListarAjustes.Click += btnListarAjustes_Click;
            txtArticuloCodigo.LostFocus += txtArticuloCodigo_LostFocus;
            btnListarArticulos.Click += btnListarArticulos_Click;
            txtArticuloCantidad.LostFocus += txtArticuloCantidad_LostFocus;
            dgDetalle.PreviewKeyUp += dgDetalle_PreviewKeyUp;
            dgDetalle.MouseDoubleClick += dgDetalle_MouseDoubleClick;
            btnNuevo.Click += btnNuevo_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnImprimir.Click += btnImprimir_Click;
            btnRegistrar.Click += btnRegistrar_Click;
        }

        void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (Save.isValid())
                Save();
        }

        void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (Print.isValid())
                Print();
        }

        void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (New.isValid())
                New();
        }

        void dgDetalle_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectItem.isValid())
                SelectItem();
        }

        void dgDetalle_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Delete) && RemoveItem.isValid())
                RemoveItem();
        }

        void txtArticuloCantidad_LostFocus(object sender, RoutedEventArgs e)
        {
            if (AddItem.isValid())
                AddItem();
        }

        void btnListarArticulos_Click(object sender, RoutedEventArgs e)
        {
            if (OpenItemsList.isValid())
                OpenItemsList();
        }

        void txtArticuloCodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindItem.isValid())
                FindItem();
        }

        void btnListarAjustes_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        void txtFolio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid())
                Find();
        }

        public Ajuste Adjustment
        {
            get
            {
                return new Ajuste()
                {
                    folio = txtFolio.Text,
                    idAjuste = _idAdjustment,
                    idTipoDeAjuste = cmbTipo.SelectedValue.ToIntOrDefault(),
                    descripcion = txtMotivo.Text,
                    TiposDeAjuste = cmbTipo.SelectedIndex >= 0 ? (TiposDeAjuste)cmbTipo.SelectedItem : null,
                    fechaHora = dpFecha.SelectedDate.Value,
                    DetallesDeAjustes = dgDetalle.Items.Cast<DetallesDeAjuste>().ToList()
                };
            }
        }

        public bool IsDirty
        {
            get { return _idAdjustment.isValid(); }
        }

        public DetallesDeAjuste CurrentItem
        {
            get
            {
                _currentItem.codigo = txtArticuloCodigo.Text;
                return new DetallesDeAjuste()
                {
                    idArticulo = _currentItem.idArticulo,
                    Articulo = _currentItem,
                    cantidad = txtArticuloCantidad.Text.ToDecimalOrDefault(),
                };
            }
        }

        public DetallesDeAjuste SelectedItem
        {
            get { return dgDetalle.SelectedIndex >= 0 ? (DetallesDeAjuste)dgDetalle.SelectedItem : null; }
        }

        public void Show(Ajuste adjustment)
        {
            txtFolio.Text = adjustment.folio;
            cmbTipo.SelectedItem = adjustment.TiposDeAjuste;
            dpFecha.SelectedDate = adjustment.fechaHora;
            txtMotivo.Text = adjustment.descripcion;
            dgDetalle.ItemsSource = adjustment.DetallesDeAjustes;

            txtArticuloCodigo.Clear();
            txtArticuloDescripcion.Clear();
            txtArticuloUnidad.Clear();
            txtArticuloCantidad.Clear();
            _currentItem = new Articulo();

            cmbTipo.SelectedItem = adjustment.TiposDeAjuste;

            _idAdjustment = adjustment.idAjuste;

        }

        public void ShowStock(decimal stock)
        {
            lblExistencia.Content = stock.ToDecimalString();
        }

        public void Clear()
        {
            txtFolio.Clear();
            txtMotivo.Clear();
            dgDetalle.ItemsSource = null;
            lblExistencia.Content = "";

            ClearItem();

            _idAdjustment = -1;
        }

        public void FillCombo(List<TiposDeAjuste> adjustmentTypes)
        {
            cmbTipo.ItemsSource = adjustmentTypes;
            cmbTipo.DisplayMemberPath = "descripcion";
            cmbTipo.SelectedValuePath = "idTipoDeAjuste";
        }

        public void Show(DetallesDeAjuste detail)
        {
            txtArticuloCodigo.Text = detail.Articulo.codigo;
            txtArticuloDescripcion.Text = detail.Articulo.descripcion;
            txtArticuloUnidad.Text = detail.Articulo.UnidadesDeMedida.descripcion;
            txtArticuloCantidad.Text = detail.cantidad.ToDecimalString();

            _currentItem = detail.Articulo;
            txtArticuloCantidad.Focus();
        }

        public void ClearItem()
        {
            txtArticuloCodigo.Clear();
            txtArticuloDescripcion.Clear();
            txtArticuloUnidad.Clear();
            txtArticuloCantidad.Clear();
            lblExistencia.Content = "";
            _currentItem = new Articulo();
        }
    }
}
