using Aprovi.Data.Models;
using Aprovi.Application.Helpers;
using System;
using System.Windows;
using System.Windows.Input;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for RegistersView.xaml
    /// </summary>
    public partial class BusinessesView : BaseView, IBusinessesView
    {
        public event Action Quit;
        public event Action New;
        public event Action Save;
        public event Action Find;
        public event Action Update;
        public event Action Delete;
        public event Action OpenList;

        private int _idBusiness;

        public BusinessesView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            txtDescripcion.LostFocus += txtDescripcion_LostFocus;
            btnCerrar.Click += btnCerrar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnNuevo.Click += btnNuevo_Click;
            btnGuardar.Click += btnGuardar_Click;
            btnListarCajas.Click += btnListarCajas_Click;
        }

        void btnListarCajas_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
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
            Mouse.OverrideCursor = null;
        }

        void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (New.isValid(AccesoRequerido.Ver_y_Agregar))
                New();
        }

        void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (Delete.isValid(AccesoRequerido.Total))
                Delete();
        }

        void txtDescripcion_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid(AccesoRequerido.Ver_y_Agregar))
                Find();
        }

        void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public Empresa Business
        {
            get { return new Empresa() { idEmpresa = _idBusiness, descripcion = txtDescripcion.Text, licencia = txtLicencia.Text }; }
        }

        public bool IsDirty
        {
            get { return _idBusiness.isValid(); }
        }

        public void Show(Empresa business)
        {
            txtDescripcion.Text = business.descripcion;
            txtLicencia.Text = business.licencia;
            _idBusiness = business.idEmpresa;
        }

        public void Clear()
        {
            txtDescripcion.Clear();
            txtLicencia.Clear();
            _idBusiness = 0;
        }
    }
}
