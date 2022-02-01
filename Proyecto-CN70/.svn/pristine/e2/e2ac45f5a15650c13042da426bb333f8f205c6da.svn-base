using Aprovi.Application.Helpers;
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
    /// Interaction logic for EquivalenciesView.xaml
    /// </summary>
    public partial class EquivalenciesView : BaseView, IEquivalenciesView
    {
        public event Action Quit;
        public event Action Add;
        public event Action Delete;

        private Articulo _item;

        public EquivalenciesView(Articulo item)
        {
            InitializeComponent();
            BindComponents();
            if (item.Equivalencias == null)
                item.Equivalencias = new List<Equivalencia>();

            _item = item;
            txtUnidadMinima.Text = item.UnidadesDeMedida.descripcion;
        }

        private void BindComponents()
        {
            btnAgregarEquivalencia.Click += btnAgregarEquivalencia_Click;
            btnCerrar.Click += btnCerrar_Click;
            dgEquivalencias.PreviewKeyUp += dgEquivalencias_PreviewKeyUp;
        }

        private void dgEquivalencias_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            //Los permisos de Equivalencias son en base a los que tenga sobre artículos, por lo que la autenticación no es asi:
            //if (e.Key.Equals(Key.Delete) && Delete.isValid(AccesoRequerido.Total))
            //Sino asi:
            if (e.Key.Equals(Key.Delete) && Delete.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "ItemsPresenter", true))
                Delete();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void btnAgregarEquivalencia_Click(object sender, RoutedEventArgs e)
        {
            //La autorización se hace en base a los privilegios asignados a Artículos
            if (Add.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "ItemsPresenter", true)) 
                Add();
        }

        public Articulo Item
        {
            get { return _item; }
        }

        public Equivalencia Equivalency
        {
            get
            {
                return new Equivalencia()
                {
                    idArticulo = _item.idArticulo,
                    Articulo = _item,
                    idUnidadDeMedida = cmbUnidadesDeMedida.SelectedValue == null ? -1 : cmbUnidadesDeMedida.SelectedValue.ToInt(),
                    unidades = txtUnidades.Text.ToDecimalOrDefault()
                };
            }
        }

        public int CurrentRecord
        {
            get { return dgEquivalencias.SelectedIndex; }
        }

        public Equivalencia CurrentEquivalency
        {
            get { return CurrentRecord >= 0 ? (Equivalencia)dgEquivalencias.SelectedItem : null; }
        }

        public void Show(List<Equivalencia> equivalencies)
        {
            dgEquivalencias.ItemsSource = equivalencies;
        }

        public void Clear()
        {
            txtUnidades.Clear();
            cmbUnidadesDeMedida.SelectedIndex = -1;
        }

        public void FillCombos(List<UnidadesDeMedida> unitsOfMeasure)
        {
            cmbUnidadesDeMedida.ItemsSource = unitsOfMeasure;
            cmbUnidadesDeMedida.DisplayMemberPath = "descripcion";
            cmbUnidadesDeMedida.SelectedValuePath = "idUnidadDeMedida";
        }
    }
}
