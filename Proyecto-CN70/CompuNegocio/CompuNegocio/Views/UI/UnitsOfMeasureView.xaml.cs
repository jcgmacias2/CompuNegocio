using Aprovi.Data.Models;
using Aprovi.Application.Helpers;
using System;
using System.Windows;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for UnitsOfMeasureView.xaml
    /// </summary>
    public partial class UnitsOfMeasureView : BaseView, IUnitsOfMeasureView
    {
        public event Action Find;
        public event Action New;
        public event Action Delete;
        public event Action Save;
        public event Action Update;
        public event Action OpenList;
        public event Action Quit;

        private int _idUnitOfMeasure;

        public UnitsOfMeasureView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            txtCodigo.LostFocus += txtCodigo_LostFocus;
            btnListarUnidades.Click += btnListarUnidades_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnGuardar.Click += btnGuardar_Click;
            btnNuevo.Click += btnNuevo_Click;
            _idUnitOfMeasure = -1;
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (New.isValid(AccesoRequerido.Ver_y_Agregar))
                New();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if(IsDirty)
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

        private void btnListarUnidades_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid(AccesoRequerido.Ver))
                OpenList();
        }

        private void txtCodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid(AccesoRequerido.Ver))
                Find();
        }

        public UnidadesDeMedida UnitOfMeasure
        {
            get { return new UnidadesDeMedida() { idUnidadDeMedida = _idUnitOfMeasure, codigo = txtCodigo.Text, descripcion = txtDescripcion.Text }; }
        }

        public bool IsDirty
        {
            get { return _idUnitOfMeasure.isValid(); }
        }

        public void Clear()
        {
            txtCodigo.Clear();
            txtDescripcion.Clear();
            _idUnitOfMeasure = -1;
        }

        public void Show(UnidadesDeMedida unitOfMeasure)
        {
            txtCodigo.Text = unitOfMeasure.codigo;
            txtDescripcion.Text = unitOfMeasure.descripcion;
            _idUnitOfMeasure = unitOfMeasure.idUnidadDeMedida;
        }
    }
}
