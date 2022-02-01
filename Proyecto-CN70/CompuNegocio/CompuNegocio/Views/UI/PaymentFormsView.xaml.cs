using Aprovi.Data.Models;
using Aprovi.Application.Helpers;
using System;
using System.Windows;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for PaymentMethodsView.xaml
    /// </summary>
    public partial class PaymentFormsView : BaseView, IPaymentFormsView
    {
        public event Action Find;
        public event Action New;
        public event Action Deactivate;
        public event Action Update;
        public event Action OpenList;
        public event Action Quit;

        private FormasPago _paymentForm;

        public PaymentFormsView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            txtDescripcion.LostFocus += txtDescripcion_LostFocus;
            btnListarFormasPago.Click += btnListarFormasPago_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnNuevo.Click += btnNuevo_Click;
            btnInactivar.Click += btnInactivar_Click;
            btnGuardar.Click += btnGuardar_Click;

            _paymentForm = new FormasPago();
        }

        private void btnListarFormasPago_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid(AccesoRequerido.Ver))
                OpenList();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (IsDirty)
            {
                if (Update.isValid(AccesoRequerido.Total))
                    Update();
            }
        }

        private void btnInactivar_Click(object sender, RoutedEventArgs e)
        {
            if (Deactivate.isValid(AccesoRequerido.Total))
                Deactivate();
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

        private void txtDescripcion_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid(AccesoRequerido.Ver))
                Find();
        }

        public FormasPago PaymentForm
        {
            get { _paymentForm.codigo = txtCodigo.Text; return _paymentForm; }
        }

        public bool IsDirty
        {
            get { return _paymentForm.idFormaPago.isValid(); }
        }

        public void Clear()
        {
            txtDescripcion.Clear();
            txtCodigo.Clear();
            _paymentForm = new FormasPago();
        }

        public void Show(FormasPago paymentForm)
        {
            txtDescripcion.Text = paymentForm.descripcion;
            txtCodigo.Text = paymentForm.codigo;
            _paymentForm = paymentForm;
        }
    }
}
