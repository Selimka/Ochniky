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
    public partial class FullAudCellView : UserControl
    {


        public FullAuditory FullAud
        {
            get { return (FullAuditory)GetValue(FullAudProperty); }
            set {
              
                SetValue(FullAudProperty, value);
            }
        }

        public static readonly DependencyProperty FullAudProperty =
            DependencyProperty.Register("FullAud", typeof(FullAuditory), typeof(FullAudCellView), new PropertyMetadata(null, new PropertyChangedCallback((sender, args) =>
            {
                var cellView = sender as FullAudCellView;
                if (cellView.FullAud.Is_everyweek)
                {
                    cellView.secondRow.Height = new GridLength(0);
                    cellView.firstRow.Height = new GridLength(220);
                }
                else
                {
                    cellView.secondRow.Height = new GridLength(110);
                    cellView.firstRow.Height = new GridLength(110);
                }
            })));

        public FullAudCellView()
        {
            InitializeComponent();
            DataContext = this;            
        }
    }
}
