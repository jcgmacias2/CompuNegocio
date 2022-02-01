using Aprovi.Data.Models;
using Aprovi.Application.Helpers;
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
    /// Interaction logic for TaxesView.xaml
    /// </summary>
    public partial class TaxesView : BaseView, ITaxesView
    {
        public event Action New;
        public event Action Delete;
        public event Action Save;
        public event Action Update;
        public event Action OpenList;
        public event Action Quit;

        private int _idTax;

        public TaxesView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnListarImpuestos.Click += btnListarImpuestos_Click;
            btnNuevo.Click += btnNuevo_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnGuardar.Click += btnGuardar_Click;

            _idTax = 0;
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (IsDirty)
            {
                if (Update.isValid(AccesoRequerido.Total))
                    Update();
            }
            else
            {
                if (Save.isValid(AccesoRequerido.Ver_y_Agregar))
                    Save();
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (Delete.isValid(AccesoRequerido.Total))
                Delete();
            
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (New.isValid(AccesoRequerido.Ver_y_Agregar))
                New();
        }

        private void btnListarImpuestos_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid(AccesoRequerido.Ver))
                OpenList();
        }

        public Impuesto Tax
        {
            
            get
            {
                return new Impuesto()
                {
                    idImpuesto = _idTax,
                    codigo = cmbImpuesto.SelectedValue.isValid() ? ((int)((Impuestos)cmbImpuesto.SelectedItem)).ToString("000") : string.Empty,
                    nombre = cmbImpuesto.SelectedValue.ToStringOrDefault(),
                    valor = txtValor.Text.ToDecimalOrDefault(),
                    idTipoDeImpuesto = cmbTiposDeImpuesto.SelectedValue.ToIntOrDefault(),
                    TiposDeImpuesto = !cmbTiposDeImpuesto.SelectedValue.isValid() ? null : (TiposDeImpuesto)cmbTiposDeImpuesto.SelectedItem,
                    idTipoFactor = cmbTiposDeFactor.SelectedValue.ToIntOrDefault(),
                    TiposFactor = !cmbTiposDeFactor.SelectedValue.isValid()? null : (TiposFactor)cmbTiposDeFactor.SelectedItem
                };
            }
        }

        public bool IsDirty
        {
            get { return _idTax.isValid(); }
        }

        public void Clear()
        {
            cmbImpuesto.SelectedIndex = -1;
            txtValor.Clear();
            cmbTiposDeImpuesto.SelectedIndex = -1;
            cmbTiposDeFactor.SelectedIndex = -1;
            _idTax = 0;

            cmbImpuesto.Focus();
        }

        public void Show(Impuesto tax)
        {
            cmbImpuesto.SelectedValue = (Impuestos)Enum.Parse(typeof(Impuestos), tax.nombre);
            txtValor.Text = tax.valor.ToDecimalString();
            cmbTiposDeImpuesto.SelectedItem = tax.TiposDeImpuesto;
            cmbTiposDeFactor.SelectedItem = tax.TiposFactor;
            _idTax = tax.idImpuesto;
        }

        public void FillCombos(List<TiposDeImpuesto> taxTypes, List<Impuestos> taxes, List<TiposFactor> factors)
        {


            cmbTiposDeImpuesto.ItemsSource = taxTypes;
            cmbTiposDeImpuesto.SelectedValuePath = "idTipoDeImpuesto";
            cmbTiposDeImpuesto.DisplayMemberPath = "descripcion";
            
            //Cuando el módulo de IEPS está activado le permite gestionar este tipo de impuestos
            if(!Modulos.IEPS.IsActive())
                taxes.Remove(Impuestos.IEPS);

            //Cuando el módulo de ISR está activado le permite gestioanr este tipo de impuestos
            if (!Modulos.ISR.IsActive())
                taxes.Remove(Impuestos.ISR);

            cmbImpuesto.ItemsSource = taxes;

            cmbTiposDeFactor.ItemsSource = factors;
            cmbTiposDeFactor.SelectedValuePath = "idTipoFactor";
            cmbTiposDeFactor.DisplayMemberPath = "codigo";
        }
    }
}
