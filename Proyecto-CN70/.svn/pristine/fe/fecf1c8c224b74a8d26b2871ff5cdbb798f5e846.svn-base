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
    /// Interaction logic for PrivilegesView.xaml
    /// </summary>
    public partial class PrivilegesView : BaseView, IPrivilegesView
    {
        public event Action Quit;
        public event Action Add;
        public event Action Delete;
        private Usuario _user;

        public PrivilegesView(Usuario user)
        {
            InitializeComponent();
            BindComponents();
            if (user.Privilegios == null)
                user.Privilegios = new List<Privilegio>();

            _user = user;
        }

        private void BindComponents()
        {
            btnAgregarPrivilegio.Click += btnAgregarPrivilegio_Click;
            btnCerrar.Click += btnCerrar_Click;
            dgPrivilegios.PreviewKeyUp += dgPrivilegios_PreviewKeyUp;
        }

        private void dgPrivilegios_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            //La autorización se hace en base a los privilegios asignados a Usuarios
            if (e.Key.Equals(Key.Delete) && Delete.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "UsersPresenter", true))
                Delete();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void btnAgregarPrivilegio_Click(object sender, RoutedEventArgs e)
        {
            //La autorización se hace en base a los privilegios asignados a Usuarios
            if (Add.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "UsersPresenter", true))
                Add();
        }

        public Usuario User
        {
            get { return _user; }
        }

        public Privilegio Privilege
        {
            get
            {
                return new Privilegio()
                {
                    idUsuario = _user.idUsuario,
                    idPermiso = cmbPermisos.SelectedValue == null ? -1 : cmbPermisos.SelectedValue.ToInt(),
                    idPantalla = cmbPantallas.SelectedValue == null ? -1 : cmbPantallas.SelectedValue.ToInt()
                };
            }
        }

        public int CurrentRecord
        {
            get { return dgPrivilegios.SelectedIndex; }
        }

        public Privilegio CurrentPrivilege
        {
            get { return CurrentRecord >= 0 ? (Privilegio)dgPrivilegios.SelectedItem : null; }
        }

        public void Show(List<Privilegio> privileges)
        {
            dgPrivilegios.ItemsSource = privileges;
        }

        public void FillCombos(List<Pantalla> views, List<Permiso> permits)
        {
            cmbPantallas.ItemsSource = views;
            cmbPantallas.SelectedValuePath = "idPantalla";
            cmbPantallas.DisplayMemberPath = "nombre";

            cmbPermisos.ItemsSource = permits;
            cmbPermisos.SelectedValuePath = "idPermiso";
            cmbPermisos.DisplayMemberPath = "descripcion";
        }

        public void Clear()
        {
            cmbPermisos.SelectedIndex = -1;
            cmbPantallas.SelectedIndex = -1;
        }
    }
}
