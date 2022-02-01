using Aprovi.Data.Models;
using Aprovi.Application.Helpers;
using System;
using System.Windows;
using System.Collections.Generic;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for SuppliersView.xaml
    /// </summary>
    public partial class SuppliersView : BaseView, ISuppliersView
    {
        public event Action Quit;
        public event Action New;
        public event Action Delete;
        public event Action Save;
        public event Action Find;
        public event Action Update;
        public event Action OpenList;

        private int _idSupplier;

        public SuppliersView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            txtCodigo.LostFocus += txtCodigo_LostFocus;
            btnListarProveedores.Click += btnListarProveedores_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnNuevo.Click += btnNuevo_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnGuardar.Click += btnGuardar_Click;

            _idSupplier = -1;
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

        private void btnListarProveedores_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid(AccesoRequerido.Ver))
                OpenList();
        }

        private void txtCodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid(AccesoRequerido.Ver))
                Find();
        }

        public Proveedore Supplier
        {
            get
            {
                return new Proveedore()
                {
                    idProveedor = _idSupplier,
                    codigo = txtCodigo.Text,
                    nombreComercial = txtNombreComercial.Text,
                    razonSocial = txtRazonSocial.Text,
                    rfc = txtRfc.Text,
                    telefono = txtTelefono.Text,
                    Domicilio = new Domicilio()
                    {
                        calle = txtCalle.Text,
                        numeroExterior = txtNumeroExterior.Text,
                        numeroInterior = txtNumeroInterior.Text,
                        colonia = txtColonia.Text,
                        ciudad = txtColonia.Text,
                        estado = txtEstado.Text,
                        idPais = cmbPais.SelectedValue.ToIntOrDefault(),
                        codigoPostal = txtCodigoPostal.Text
                    }
                };
            }
        }

        public bool IsDirty
        {
            get { return _idSupplier.isValid(); }
        }

        public void Show(Proveedore supplier)
        {
            txtCodigo.Text = supplier.codigo;
            txtNombreComercial.Text = supplier.nombreComercial;
            txtRazonSocial.Text = supplier.razonSocial;
            txtRfc.Text = supplier.rfc;
            txtCalle.Text = supplier.Domicilio.calle;
            txtNumeroExterior.Text = supplier.Domicilio.numeroExterior;
            txtNumeroInterior.Text = supplier.Domicilio.numeroInterior;
            txtColonia.Text = supplier.Domicilio.colonia;
            txtCiudad.Text = supplier.Domicilio.ciudad;
            txtEstado.Text = supplier.Domicilio.estado;
            cmbPais.SelectedValue = supplier.Domicilio.idPais;
            txtCodigoPostal.Text = supplier.Domicilio.codigoPostal;
            txtTelefono.Text = supplier.telefono;

            _idSupplier = supplier.idProveedor;
        }

        public void Clear()
        {
            txtCodigo.Clear();
            txtNombreComercial.Clear();
            txtRazonSocial.Clear();
            txtRfc.Clear();
            txtCalle.Clear();
            txtNumeroExterior.Clear();
            txtNumeroInterior.Clear();
            txtColonia.Clear();
            txtCiudad.Clear();
            txtEstado.Clear();
            cmbPais.SelectedIndex = -1;
            txtCodigoPostal.Clear();
            txtTelefono.Clear();

            _idSupplier = -1;
        }

        public void FillCombo(List<Pais> countries)
        {
            cmbPais.ItemsSource = countries;
            cmbPais.DisplayMemberPath = "descripcion";
            cmbPais.SelectedValuePath = "idPais";
        }
    }
}
