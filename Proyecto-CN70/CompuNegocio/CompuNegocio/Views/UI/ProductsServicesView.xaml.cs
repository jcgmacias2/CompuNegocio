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
    /// Interaction logic for ProductsServicesView.xaml
    /// </summary>
    public partial class ProductsServicesView : BaseView, IProductsServicesView
    {
        public event Action Find;
        public event Action New;
        public event Action Delete;
        public event Action Save;
        public event Action Update;
        public event Action OpenList;
        public event Action Quit;

        private int _idProductService;

        public ProductsServicesView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            txtCodigo.LostFocus += txtCodigo_LostFocus;
            btnListarProductosServicios.Click += btnListarProductosServicios_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnGuardar.Click += btnGuardar_Click;
            btnNuevo.Click += btnNuevo_Click;
            _idProductService = -1;
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (New.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "ItemsPresenter", true))
                New();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (IsDirty)
            {
                if (Update.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "ItemsPresenter", true))
                    Update();
            }
            else
            {
                if (Save.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "ItemsPresenter", true))
                    Save();
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (Delete.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "ItemsPresenter", true))
                Delete();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void btnListarProductosServicios_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        private void txtCodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Ver, "ItemsPresenter", true))
                Find();
        }

        public ProductosServicio ProductService
        {
            get { return new ProductosServicio() { idProductoServicio = _idProductService, codigo = txtCodigo.Text, descripcion = txtDescripcion.Text }; }
        }

        public bool IsDirty
        {
            get { return _idProductService.isValid(); }
        }

        public void Clear()
        {
            txtCodigo.Clear();
            txtDescripcion.Clear();
            _idProductService = -1;
        }

        public void Show(ProductosServicio productService)
        {
            txtCodigo.Text = productService.codigo;
            txtDescripcion.Text = productService.descripcion;
            _idProductService = productService.idProductoServicio;
        }
    }
}
