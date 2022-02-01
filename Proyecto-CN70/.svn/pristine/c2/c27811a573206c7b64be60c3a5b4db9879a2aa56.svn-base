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
    /// Interaction logic for ItemCommentView.xaml
    /// </summary>
    public partial class ItemCommentView : BaseView, IItemCommentView
    {
        public event Action Save;

        public ItemCommentView()
        {
            InitializeComponent();
            BindComponent();
        }

        public ItemCommentView(string comment)
        {
            InitializeComponent();
            BindComponent();
            txtComentario.Text = comment;
        }

        private void BindComponent()
        {
            btnGuardar.Click += BtnGuardar_Click;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (Save.isValid())
                Save();
        }

        public string Comment => txtComentario.Text;

        public void Fill(string comment)
        {
            txtComentario.Text = comment;
        }
    }
}
