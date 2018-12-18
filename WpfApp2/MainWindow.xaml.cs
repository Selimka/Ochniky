using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
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
using WpfApp2.Views;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        
        SqlConnection connection = new SqlConnection
        {
            ConnectionString = new SqlConnectionStringBuilder
            {
                DataSource = "(local)",
                InitialCatalog = "IrNITU",
                IntegratedSecurity = true,
            }.ToString()
        };

        public ObservableCollection<Auditory> Auditories 
        {
            get { return (ObservableCollection<Auditory>)GetValue(AuditoriesProperty); }
            set { SetValue(AuditoriesProperty, value); }
        }

        public static readonly DependencyProperty AuditoriesProperty =
            DependencyProperty.Register("Auditories", typeof(ObservableCollection<Auditory>), typeof(MainWindow), new PropertyMetadata(null));

       

        void LoadAudInfo (string audTitle)
        {
            string sqlExpression = string.Format("SELECT day, everyweek, para," +
                " rtrim(p.preps) as prep," +
                "rtrim(vp.pred) as pred," +
                "rtrim(nt.repvrnt) as nt," +
                "rtrim(coalesce(pl.konts, kg.obozn, kk.obozn)) as kont," +
                "coalesce(pl.stud, kg.students, kk.stud) as stud " +
                "FROM raspis r " +
                "LEFT JOIN auditories a ON r.aud = a.id_60 " +
                "LEFT JOIN raspnagr rn ON rn.id_51 = r.raspnagr " +
                "LEFT JOIN prepods p ON rn.prep = p.id_61 " +
                "LEFT JOIN vacpred vp ON rn.pred = vp.id_15 " +
                "LEFT JOIN kontgrp kg ON kg.id_7 = rn.kontid " +
                "LEFT JOIN kontkurs kk ON kk.id_1 = rn.kont " +
                "LEFT JOIN potoklist pl ON pl.op = rn.op " +
                "LEFT JOIN normtime nt ON nt.id_40 = rn.nt " +
                $"WHERE rtrim(a.obozn) = '{audTitle}' " +
                "ORDER BY day, para ");


            var sqlCommand = new SqlCommand(sqlExpression, connection);

            using (var dataAdapter = new SqlDataAdapter(sqlCommand))
            {
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                var fetchedPersons = dt.AsEnumerable()
                    .Select(x => new Auditory
                    {
                        Group = x.Field<String>("kont"),
                        Pair = x.Field<Int16>("para"),
                        Discipline = x.Field<String>("pred"),
                        Day = x.Field<Int16>("day"),
                        Count = x.Field<int>("stud"),
                        DayPeople = (x.Field<Int16>("day") - 1) % 7 + 1,
                        Type = x.Field<String>("nt"),
                        Teacher = x.Field<String>("prep"),
                        Is_everyweek =x.Field<Int16>("everyweek")==2
                    });

                this.Auditories = new ObservableCollection<Auditory>(fetchedPersons);
            }



            var groupAud = from a in Auditories
                           group a by new
                           {
                               a.DayPeople,
                               a.Pair,
                               a.Is_everyweek
                           };

            var groupList = new List<FullAuditory>();
           
            foreach (var g in groupAud )
            {
                var list = g.ToList();
                var fAud = new FullAuditory
                {
                    Pair = g.Key.Pair,
                    DayPeople = g.Key.DayPeople,
                    Is_everyweek = g.Key.Is_everyweek
                };

                if (list[0].Day >= 8)
                {
                    fAud.Even = list[0];
                }
                else
                {
                    fAud.Odd = list[0];
                }

                if (g.Count() == 2)
                {
                    if (list[1].Day >= 8)
                    {
                        fAud.Even = list[1];
                    }
                    else
                    {
                        fAud.Odd = list[1];
                    }
                }
                groupList.Add(fAud);
            }
      
            foreach (var aud in groupList)
            {
                AuditoryRowVaiew rowControl = this.FindName($"Row{aud.DayPeople}") as AuditoryRowVaiew;
                if (rowControl != null)
                {
                    var property = rowControl.GetType().GetProperty($"AudCell{aud.Pair - 1}");
                    property.SetValue(rowControl, aud);
                }
            }
        }

        public ObservableCollection<String> ListAud
        {
            get { return (ObservableCollection<String>)GetValue(ListAudProperty); }
            set { SetValue(ListAudProperty, value); }
        }

        public static readonly DependencyProperty ListAudProperty =
            DependencyProperty.Register("ListAud", typeof(ObservableCollection<String>), typeof(MainWindow), new PropertyMetadata(null));

       
                

        void LoadListAud()
        {
            string query = "SELECT rtrim(obozn) as obozn " +
                "From auditories " +
                "Order by obozn";

            var sqlCommand = new SqlCommand(query, connection);

            using (var dataAdapter = new SqlDataAdapter(sqlCommand))
            {
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                var items = dt.AsEnumerable()
                    .Select(x => x.Field<string>("obozn"));                         
                ListAud = new ObservableCollection<String>(items);
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            Auditories = new ObservableCollection<Auditory>();

            foreach (var aud in Auditories)
            {
                AuditoryRowVaiew rowControl = this.FindName($"Row{aud.DayPeople}") as AuditoryRowVaiew;
                if (rowControl != null)
                {
                    var property = rowControl.GetType().GetProperty($"AudCell{aud.Pair - 1}");
                    property.SetValue(rowControl, aud);
                }
            }
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            connection.Open();            
            LoadListAud();
            DataContext = this;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;

            foreach (var cl in Enumerable.Range(1, 7))
            {
                if (this.FindName($"Row{cl}") is AuditoryRowVaiew rowControl)
                {
                    rowControl.Clear();
                }
            }
            LoadAudInfo(cmb.SelectedItem as String);
        }

        void ComboboxTextChanged(object sender, RoutedEventArgs e)
        {
           CmBx.IsDropDownOpen = true;
            CollectionView pick = (CollectionView)CollectionViewSource.GetDefaultView(CmBx.ItemsSource);
            pick.Filter = s =>
                ((string)s).IndexOf(CmBx.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
    }
}
