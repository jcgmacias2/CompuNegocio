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
    /// Interaction logic for CustomsApplicationView.xaml
    /// </summary>
    public partial class CustomsApplicationView : BaseView, ICustomsApplicationView
    {
        public event Action Save;
        private int _idCustomsApplication;

        public CustomsApplicationView()
        {
            InitializeComponent();
            BindComponents();

            _idCustomsApplication = -1;
        }

        public CustomsApplicationView(Pedimento customsApplication)
        {
            InitializeComponent();
            BindComponents();

            Fill(customsApplication);
        }

        private void BindComponents()
        {
            btnGuardar.Click += BtnGuardar_Click;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (Save.isValid())
                Save();
        }

        public Pedimento CustomsApplication => new Pedimento() { idPedimento = _idCustomsApplication, añoOperacion = txtAñoOperacion.Text, aduana = txtAduana.Text, patente = txtPatente.Text, añoEnCurso = txtAñoEnCurso.Text, progresivo = txtProgesivo.Text, fecha = dpFecha.SelectedDate.Value };

        public void Fill(Pedimento customsApplication)
        {
            txtAñoOperacion.Text = customsApplication.añoOperacion;
            txtAduana.Text = customsApplication.aduana;
            txtPatente.Text = customsApplication.patente;
            txtAñoEnCurso.Text = customsApplication.añoEnCurso;
            txtProgesivo.Text = customsApplication.progresivo;
            dpFecha.SelectedDate = customsApplication.fecha;

            _idCustomsApplication = customsApplication.idPedimento;
        }
    }
}
