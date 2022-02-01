using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for ItemsMigrationToolView.xaml
    /// </summary>
    public partial class ItemsMigrationToolView : BaseView, IItemsMigrationToolView
    {
        public event Action Quit;
        public event Action Migrate;

        private string _dbcPath;

        public ItemsMigrationToolView(string dbcPath)
        {
            InitializeComponent();
            _dbcPath = dbcPath;
            BindComponents();
        }

        private void BindComponents()
        {
            btnCerrar.Click += BtnCerrar_Click;
            btnProcesar.Click += BtnProcesar_Click;
        }

        private void BtnProcesar_Click(object sender, RoutedEventArgs e)
        {
            if (Migrate.isValid())
                Migrate();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public List<VMEquivalenciaUnidades> Units => dgUnidades.ItemsSource.Cast<VMEquivalenciaUnidades>().ToList();

        public List<VMEquivalenciaClasificacion> Classifications => dgClasificaciones.ItemsSource.Cast<VMEquivalenciaClasificacion>().ToList();

        public Impuesto VAT => cmbImpuestos.SelectedIndex >= 0 ? (Impuesto)cmbImpuestos.SelectedItem : null;

        public string dbcPath => _dbcPath;

        public void Fill(List<VMEquivalenciaUnidades> units, List<UnidadesDeMedida> unitsOfMeasure, List<VMEquivalenciaClasificacion> familiesAndDepartments, List<Clasificacione> classifications, List<Impuesto> taxes)
        {
            dgUnidades.ItemsSource = units;
            if (unitsOfMeasure.Count > 0)
                ((DataGridComboBoxColumn)dgUnidades.Columns[1]).ItemsSource = unitsOfMeasure;

            dgClasificaciones.ItemsSource = familiesAndDepartments;
            if (classifications.Count > 0)
                ((DataGridComboBoxColumn)dgClasificaciones.Columns[2]).ItemsSource = classifications;
             
            if (taxes.Count > 0)
                cmbImpuestos.ItemsSource = taxes;
        }
    }
}
