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
    /// Interaction logic for UsersView.xaml
    /// </summary>
    public partial class UsersView : BaseView, IUsersView
    {
        public event Action Quit;
        public event Action New;
        public event Action Find;
        public event Action Delete;
        public event Action Save;
        public event Action Update;
        public event Action OpenList;
        public event Action OpenPrivileges;
        public event Action AddCommission;
        public event Action RemoveCommission;

        private int _idUser;

        public UsersView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            txtNombreDeUsuario.LostFocus += txtNombreDeUsuario_LostFocus;
            btnListarUsuarios.Click += btnListarUsuarios_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnNuevo.Click += btnNuevo_Click;
            btnPrivilegios.Click += btnPrivilegios_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnGuardar.Click += btnGuardar_Click;
            btnAgregarComision.Click += BtnAgregarComisionOnClick;
            dgComisiones.PreviewKeyUp += DgComisionesOnPreviewKeyUp;

            _idUser = -1;
        }

        private void DgComisionesOnPreviewKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key.Equals(Key.Delete) && RemoveCommission.isValid(AccesoRequerido.Ver_y_Agregar))
                RemoveCommission();
        }

        private void BtnAgregarComisionOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (AddCommission.isValid(AccesoRequerido.Ver_y_Agregar))
            {
                AddCommission();
            }
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

        private void btnPrivilegios_Click(object sender, RoutedEventArgs e)
        {
            if (OpenPrivileges.isValid(AccesoRequerido.Ver))
                OpenPrivileges();
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (New.isValid(AccesoRequerido.Ver_y_Agregar))
                New();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void btnListarUsuarios_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid(AccesoRequerido.Ver))
                OpenList();
        }

        private void txtNombreDeUsuario_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid(AccesoRequerido.Ver))
                Find();
        }

        public Usuario User
        {
            get
            {
                return new Usuario()
                {
                    idUsuario = _idUser,
                    nombreDeUsuario = txtNombreDeUsuario.Text,
                    contraseña = txtContraseña.Password,
                    nombreCompleto = txtNombreCompleto.Text,
                    descuento = txtDescuento.Text.ToDecimalOrDefault(),
                    ComisionesPorUsuarios = Comissions
                };
            }
        }

        public ComisionesPorUsuario CurrentCommission { get{return new ComisionesPorUsuario(){idTipoDeComision = cbComision.SelectedValue.ToIntOrDefault(),valor = txtPorcentajeComision.Text.ToDecimalOrDefault(), TiposDeComision = (TiposDeComision)cbComision.SelectedItem};} }

        public List<ComisionesPorUsuario> Comissions
        {
            get { return dgComisiones.Items.Cast<ComisionesPorUsuario>().ToList(); }
        }

        public ComisionesPorUsuario SelectedComission { get { return (ComisionesPorUsuario)dgComisiones.SelectedItem; } }

        public bool IsDirty
        {
            get { return _idUser.isValid(); }
        }

        public void Show(Usuario user)
        {
            txtNombreDeUsuario.Text = user.nombreDeUsuario;
            txtContraseña.Password = user.contraseña;
            txtNombreCompleto.Text = user.nombreCompleto;
            txtDescuento.Text = user.descuento.ToDecimalString();
            Show(user.ComisionesPorUsuarios.ToList());
            _idUser = user.idUsuario;
        }

        public void Show(List<ComisionesPorUsuario> userCommissions)
        {
            dgComisiones.ItemsSource = null;
            dgComisiones.ItemsSource = userCommissions;
        }

        public void FillCombos(List<TiposDeComision> commissions)
        {
            cbComision.ItemsSource = commissions;
            cbComision.SelectedValuePath = "idTipoDeComision";
            cbComision.DisplayMemberPath = "descripcion";
        }

        public void Clear()
        {
            txtNombreDeUsuario.Clear();
            txtContraseña.Clear();
            txtNombreCompleto.Clear();
            txtDescuento.Clear();
            _idUser = -1;
        }

        public void ClearCommission()
        {
            cbComision.SelectedIndex = -1;
            txtPorcentajeComision.Clear();
        }
    }
}
