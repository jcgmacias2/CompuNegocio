using Aprovi.Application.Helpers;
using Aprovi.Data.Models;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for BanksView.xaml
    /// </summary>
    public partial class BanksView : BaseView, IBanksView
    {
        public event Action Find;
        public event Action New;
        public event Action Delete;
        public event Action Save;
        public event Action OpenList;
        public event Action Quit;

        private int _idBank;

        public BanksView()
        {
            InitializeComponent();
            BindComponents();
            _idBank = -1;
        }

        private void BindComponents()
        {
            txtNombre.LostFocus += TxtNombre_LostFocus;
            btnListarBancos.Click += BtnListarBancos_Click;
            btnNuevo.Click += BtnNuevo_Click;
            btnCerrar.Click += BtnCerrar_Click;
            btnGuardar.Click += BtnGuardar_Click;
            btnEliminar.Click += BtnEliminar_Click;
        }

        private void BtnListarBancos_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (Delete.isValid(AccesoRequerido.Total))
                Delete();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (Save.isValid(AccesoRequerido.Ver_y_Agregar))
                Save();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (New.isValid())
                New();
        }

        private void TxtNombre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid())
                Find();
        }

        public Banco Bank
        {
            get { return new Banco() { idBanco = _idBank, nombre = txtNombre.Text }; }
        }

        public bool IsDirty
        {
            get { return _idBank.isValid(); }
        }

        public void Clear()
        {
            txtNombre.Clear();
            _idBank = -1;
        }

        public void Show(Banco bank)
        {
            txtNombre.Text = bank.nombre;
            _idBank = bank.idBanco;
        }
    }
}
