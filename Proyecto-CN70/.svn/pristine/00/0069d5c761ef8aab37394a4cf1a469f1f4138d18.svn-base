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

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for CFDIUsesView.xaml
    /// </summary>
    public partial class CFDIUsesView : BaseView, ICFDIUsesView
    {
        public event Action Find;
        public event Action New;
        public event Action Deactivate;
        public event Action Update;
        public event Action OpenList;
        public event Action Quit;

        private int _idUse;

        public CFDIUsesView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            txtCodigo.LostFocus += TxtCodigo_LostFocus;
            btnListarUsosCFDI.Click += BtnListarUsosCFDI_Click;
            btnNuevo.Click += BtnNuevo_Click;
            btnInactivar.Click += BtnInactivar_Click;
            btnGuardar.Click += BtnGuardar_Click;
            btnCerrar.Click += BtnCerrar_Click;

            _idUse = -1;
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (IsDirty && Update.isValid())
                Update();
        }

        private void BtnInactivar_Click(object sender, RoutedEventArgs e)
        {
            if (Deactivate.isValid())
                Deactivate();
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (New.isValid())
                New();
        }

        private void BtnListarUsosCFDI_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        private void TxtCodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid())
                Find();
        }

        public UsosCFDI Use
        {
            get { return new UsosCFDI() { idUsoCFDI = _idUse, codigo = txtCodigo.Text, descripcion = txtDescripcion.Text }; }
        }

        public bool IsDirty
        {
            get { return _idUse.isValid(); }
        }

        public void Clear()
        {
            txtCodigo.Clear();
            txtDescripcion.Clear();
            _idUse = -1;
        }

        public void Show(UsosCFDI use)
        {
            txtCodigo.Text = use.codigo;
            txtDescripcion.Text = use.descripcion;
            _idUse = use.idUsoCFDI;
        }
    }
}
