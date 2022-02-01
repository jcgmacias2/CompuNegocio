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
    /// Interaction logic for ClassificationsView.xaml
    /// </summary>
    public partial class ClassificationsView : BaseView, IClassificationsView
    {
        public event Action Find;
        public event Action New;
        public event Action Delete;
        public event Action Save;
        public event Action Update;
        public event Action OpenList;
        public event Action Quit;

        private int _idClassification;

        public ClassificationsView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            txtClasificacion.LostFocus += txtClasificacion_LostFocus;
            btnListarClasificaciones.Click += btnListarClasificaciones_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnGuardar.Click += btnGuardar_Click;
            btnNuevo.Click += btnNuevo_Click;

            _idClassification = -1;
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

        private void btnListarClasificaciones_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid(AccesoRequerido.Ver))
                OpenList();
        }

        private void txtClasificacion_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid(AccesoRequerido.Ver))
                Find();
        }

        public Clasificacione Classification
        {
            get
            {
                return new Clasificacione()
                {
                    idClasificacion = _idClassification,
                    descripcion = txtClasificacion.Text
                };
            }
        }

        public bool IsDirty
        {
            get { return _idClassification.isValid(); }
        }

        public void Clear()
        {
            txtClasificacion.Clear();
            _idClassification = -1;
        }

        public void Show(Clasificacione classification)
        {
            txtClasificacion.Text = classification.descripcion;
            _idClassification = classification.idClasificacion;
        }
    }
}
