using Aprovi.Business.ViewModels;
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
    /// Interaction logic for CreditNotePrintView.xaml
    /// </summary>
    public partial class CreditNotePrintView : BaseView, ICreditNotePrintView
    {
        public event Action FindLast;
        public event Action Find;
        public event Action OpenList;
        public event Action Quit;
        public event Action Preview;
        public event Action Print;
        private NotasDeCredito _creditNote;
        private bool _editable;

        public CreditNotePrintView()
        {
            InitializeComponent();
            BindComponents();

            _creditNote = new NotasDeCredito();
            _editable = true;
        }

        public CreditNotePrintView(NotasDeCredito creditNote)
        {
            InitializeComponent();
            BindComponents();

            _editable = false;
            Show(creditNote);
        }

        private void BindComponents()
        {
            txtSerie.LostFocus += txtSerie_LostFocus;
            txtFolio.LostFocus += txtFolio_LostFocus;
            btnListarNotasDeCredito.Click += btnListarNotasDeCredito_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;
        }

        void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (Print.isValid())
                Print();
            Mouse.OverrideCursor = null;
        }

        void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (Preview.isValid())
                Preview();
            Mouse.OverrideCursor = null;
        }

        void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        void btnListarNotasDeCredito_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        void txtFolio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid() && _editable)
                Find();
        }

        void txtSerie_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindLast.isValid() && _editable)
                FindLast();
        }

        public NotasDeCredito CreditNote
        {
            get
            {
                return _creditNote.idNotaDeCredito.isValid() && !_editable ? _creditNote : 
                    new NotasDeCredito() { serie = txtSerie.Text, folio = txtFolio.Text.ToIntOrDefault() };
            }
        }

        public void Show(NotasDeCredito creditNote)
        {
            txtSerie.Text = creditNote.serie;
            txtFolio.Text = creditNote.folio.ToStringOrDefault();

            txtSerie.IsReadOnly = creditNote.idNotaDeCredito.isValid() && !_editable;
            txtFolio.IsReadOnly = creditNote.idNotaDeCredito.isValid() && !_editable;
            btnListarNotasDeCredito.IsEnabled = !creditNote.idNotaDeCredito.isValid() || _editable;

            _creditNote = creditNote;
        }
    }
}
