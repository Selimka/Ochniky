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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.Models;

namespace WpfApp2.Views
{
    public partial class AuditoryCellView : UserControl
    {


        public Auditory Auditories
        {
            get { return (Auditory)GetValue(AuditoriesProperty); }
            set { SetValue(AuditoriesProperty, value); }
        }

        public static readonly DependencyProperty AuditoriesProperty =
            DependencyProperty.Register("Auditories", typeof(Auditory), typeof(AuditoryCellView), new PropertyMetadata(null));



        public AuditoryCellView()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
