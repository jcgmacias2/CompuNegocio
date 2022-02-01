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
using Aprovi.Data.Models;
using Aprovi.Application.Helpers;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for AssociateCompanyView.xaml
    /// </summary>
    public partial class AssociatedCompaniesView : BaseView, IAssociatedCompaniesView
    {
        public event Action Find;
        public event Action FindCompany;
        public event Action OpenList;
        public event Action OpenCompaniesList;
        public event Action Quit;
        public event Action Save;
        public event Action Update;
        public event Action Delete;
        public event Action New;

        private int _idAssociatedCompany;
        private Empresa _company;


        public AssociatedCompaniesView()
        {
            InitializeComponent();
            BindComponents();

            _idAssociatedCompany = -1;
            _company = new Empresa();
        }

        private void BindComponents()
        {
            txtDescripcion.LostFocus += TxtDescripcion_LostFocus;
            txtEmpresa.LostFocus += TxtEmpresaOnLostFocus;
            btnListarEmpresasAsociadas.Click += BtnListarEmpresasAsociadas_Click;
            btnListarEmpresas.Click += BtnListarEmpresas_Click;
            btnCerrar.Click += BtnCerrar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnGuardar.Click += BtnGuardar_Click;
            btnNuevo.Click += BtnNuevo_Click;
        }

        private void TxtEmpresaOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (FindCompany.isValid())
            {
                FindCompany();
            }
        }

        private void BtnListarEmpresas_Click(object sender, RoutedEventArgs e)
        {
            if (OpenCompaniesList.isValid())
            {
                OpenCompaniesList();
            }
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (New.isValid())
                New();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (_idAssociatedCompany.isValid())
            {
                if (Update.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "AssociatedCompaniesPresenter", true))
                    Update();
            }
            else
            {
                if (Save.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "AssociatedCompaniesPresenter", true))
                    Save();
            }    
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (Delete.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "AssociatedCompaniesPresenter", true))
                Delete();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void BtnListarEmpresasAsociadas_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        private void TxtDescripcion_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid())
                Find();
        }

        public EmpresasAsociada AssociatedCompany
        {
            get
            {
                return new EmpresasAsociada()
                {
                    usuario = txtUsuario.Text,
                    baseDeDatos = txtBaseDeDatos.Text,
                    Empresa = new Empresa() { descripcion = txtEmpresa.Text },
                    contrasena = txtContraseña.Text,
                    idEmpresaLocal = _company.isValid() && _company.idEmpresa.isValid() ? _company.idEmpresa : (int?)null,
                    nombre = txtDescripcion.Text,
                    servidor = txtServidor.Text,
                    idEmpresaAsociada = _idAssociatedCompany
                };
            }
        }

        public void Clear()
        {
            txtDescripcion.Clear();
            txtContraseña.Clear();
            txtServidor.Clear();
            txtBaseDeDatos.Clear();
            txtEmpresa.Clear();
            txtUsuario.Clear();

            _idAssociatedCompany = -1;
            _company = new Empresa();
        }

        public void Show(EmpresasAsociada associatedCompany)
        {
            _idAssociatedCompany = associatedCompany.idEmpresaAsociada;

            txtDescripcion.Text = associatedCompany.nombre;
            txtContraseña.Text = associatedCompany.contrasena;
            txtServidor.Text = associatedCompany.servidor;
            txtBaseDeDatos.Text = associatedCompany.baseDeDatos;
            txtUsuario.Text = associatedCompany.usuario;

            if (associatedCompany.Empresa.isValid() && associatedCompany.Empresa.idEmpresa.isValid())
            {
                txtEmpresa.Text = associatedCompany.Empresa.descripcion;
            }

            _company = associatedCompany.Empresa;
        }
    }
}
